using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityProject.Scripts.Common;
using UnityProject.Scripts.Common.Enums;
using UnityProject.Scripts.Enums;
using UnityProject.Scripts.UI;
using UnityProject.Scripts.Util;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class BattleController : IInitializable, IDisposable
    {
        [Inject] private RoomController _roomController;
        [Inject] private PlayerController _playerController;
        [Inject] private EnemyController _enemyController;
        [Inject] private DeckController _deckController;
        [Inject] private GameplayUIPresenter _gameplayUI;
        [Inject] private SceneSwitcher _sceneSwitcher;

        private Turn _turnOrder = Turn.Player;
        private CancellationTokenSource Cts { get; set; }

        public void Initialize()
        {
            Cts = new CancellationTokenSource();
            _playerController.OnPlayerDead += async () => await Finish();
            _enemyController.OnAllEnemiesDead += async () => await Finish();
            _gameplayUI.OnCardInteract += OnCardInteract;
            _gameplayUI.OnEndTurnButtonClick += SwitchTurn;
            _gameplayUI.OnStartNewButtonClick += async () => await OnStartNewButtonClickHandler();
            _gameplayUI.RegisterCards(_deckController.Deck);
            StartBattle();
        }

        public void Dispose()
        {
            _gameplayUI.OnCardInteract -= OnCardInteract;
            _gameplayUI.OnEndTurnButtonClick -= SwitchTurn;
            _gameplayUI.OnStartNewButtonClick -= StartBattle;
            Cts.Cancel();
            Cts.Dispose();
        }

        private async UniTask OnStartNewButtonClickHandler()
        {
            if (!await _roomController.NextRoom())
            {
                await UniTask.WaitForSeconds(1, cancellationToken: Cts.Token);
                _sceneSwitcher.Switch(SceneType.MainMenu);
                return;
            }

            await _playerController.StartBattleMove();
            _enemyController.InstantiateEnemies();
            StartBattle();
        }

        private void StartBattle()
        {
            Prepare();
        }

        private void Prepare()
        {
            _gameplayUI.SetEnergyCount(_playerController.GetEnergy());
            _gameplayUI.SetDiscardCount(0);
            _deckController.GetHand(5);
            _gameplayUI.ShowHand(_deckController.Hand);
            _gameplayUI.SetDeckCount(_deckController.Deck.Count);
            _gameplayUI.EnableBattleUI(true);
        }

        private async UniTask Finish()
        {
            if (_playerController.GetHealth() <= 0)
            {
                _gameplayUI.ShowGameOver();
                await UniTask.WaitForSeconds(2, cancellationToken: Cts.Token);
                _sceneSwitcher.Switch(SceneType.MainMenu);
                _deckController.Clear();
                return;
            }

            await _playerController.FinishBattleMove();
            
            _playerController.ResetEnergy();
            _gameplayUI.HideHand(_deckController.Hand);
            _gameplayUI.EnableBattleUI(false);
            _deckController.DiscardAllHand();
            _deckController.Shuffle();
        }

        private void SwitchTurn()
        {
            _turnOrder = _turnOrder == Turn.Player ? Turn.Enemy : Turn.Player;
            
            if (_turnOrder == Turn.Enemy)
            {
                _gameplayUI.TurnButtonEnable = false;
                _gameplayUI.HideHand(_deckController.Hand);
                _deckController.DiscardAllHand();
                _gameplayUI.SetDiscardCount(_deckController.Discard.Count);
                EnemyTurn(_enemyController.GetEnemyActions());
            }
            else
            {
                TryShuffleDiscard();
                _deckController.GetHand(5);
                _playerController.ResetEnergy();
                _gameplayUI.ShowHand(_deckController.Hand);
                _gameplayUI.SetDeckCount(_deckController.Deck.Count);
                _gameplayUI.SetEnergyCount(_playerController.GetEnergy());
                _gameplayUI.TurnButtonEnable = true;
            }
        }

        private void TryShuffleDiscard()
        {
            if (_deckController.Deck.Count >= 5) return;
            
            _deckController.Shuffle();
            _gameplayUI.SetDiscardCount(_deckController.Discard.Count);
        }

        private void OnCardInteract(Card card)
        {
            if(Comparator.Less(_playerController.GetEnergy(),card.Cost)) return;
            
            OnCardInteractAsync(card).Forget();
        }
        
        private async UniTask OnCardInteractAsync(Card card)
        {
            if (card.ActionType == ActionType.Attack)
            {
                _gameplayUI.HighlightCard(card);
                _gameplayUI.EnableArrow(true);
                
                await UniTask.WaitUntil(() => card.Target != null, cancellationToken: Cts.Token);
                
                _gameplayUI.EnableArrow(false);
                ActivateCard(card);
                return;
            }
            
            _gameplayUI.HighlightCard(card);
            await UniTask.WaitForSeconds(0.5f);
            ActivateCard(card);
        }

        private void ActivateCard(Card card)
        {
            switch (card.ActionType)
            {
                case ActionType.Attack:
                    _playerController.PlayAnimation("Attack", true, false);
                    _enemyController.Damage(-card.Value, card.Target);
                    _playerController.PlayAnimation("Idle", false, true);
                    card.Target = null;
                    break;
                case ActionType.Defense:
                    _playerController.ChangeArmor(card.Value);
                    break;
                case ActionType.Healing:
                    _playerController.ChangeHealth(card.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _deckController.DiscardCard(card);
            _gameplayUI.ActivateCard(card);
            _playerController.ChangeEnergy(-card.Cost);
            _gameplayUI.SetEnergyCount(_playerController.GetEnergy());

            if (Comparator.LessOrEquals(_playerController.GetEnergy(), 0) || 
                Comparator.LessOrEquals(_deckController.Hand.Count, 0))
            {
                SwitchTurn();
            }
        }

        private void EnemyTurn(List<EnemyAction> enemyAction)
        {
            foreach (var action in enemyAction)
            {
                switch (action.ActionType)
                {
                    case ActionType.Attack:
                        _enemyController.PlayAnimation("Attack", true,false, action.Enemy);
                        _playerController.Damage(-action.Value);
                        _enemyController.PlayAnimation("Idle", false, true, action.Enemy);
                        break;
                    case ActionType.Defense:
                        _enemyController.ChangeArmor(action.Enemy, action.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            SwitchTurn();
        }
    }
}