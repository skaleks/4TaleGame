﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityProject.Scripts.Common;
using UnityProject.Scripts.Common.Enums;
using UnityProject.Scripts.Enums;
using UnityProject.Scripts.Gameplay.View;
using UnityProject.Scripts.UI;
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

        private CancellationTokenSource _cancellationTokenSource;
        private Turn TurnOrder { get; set; } = Turn.Player;

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _playerController.OnPlayerDead += async () => await Finish();
            _enemyController.OnAllEnemiesDead += async () => await Finish();
            _gameplayUI.OnCardInteract += OnCardInteract;
            _gameplayUI.OnEndTurnButtonClick += SwitchTurn;
            StartBattle();
        }

        public void Dispose()
        {
            _gameplayUI.OnCardInteract -= OnCardInteract;
            _gameplayUI.OnEndTurnButtonClick -= SwitchTurn;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void StartBattle()
        {
            _deckController.InstantiateDeck();
            PrepareUI();
        }

        private void PrepareUI()
        {
            _gameplayUI.RegisterCards(_deckController.Deck);
            _gameplayUI.SetEnergyCount(_playerController.GetEnergy());
            _gameplayUI.SetDeckCount(_playerController.GetDeckCapacity());
            _gameplayUI.SetDiscardCount(0);
            _deckController.GetHand(5);
            _gameplayUI.ShowHand(_deckController.Hand);
            _gameplayUI.SetDeckCount(_deckController.Deck.Count);
        }

        private async UniTask Finish()
        {
            if (_playerController.GetHealth() <= 0)
            {
                _gameplayUI.ShowGameOver();
                await UniTask.WaitForSeconds(2, cancellationToken: _cancellationTokenSource.Token);
                _deckController.Clear();
                _sceneSwitcher.Switch(SceneType.MainMenu);
                return;
            }
            
            _roomController.InstantiateRoom();
        }

        private void SwitchTurn()
        {
            TurnOrder = TurnOrder == Turn.Player ? Turn.Enemy : Turn.Player;
            
            if (TurnOrder == Turn.Enemy)
            {
                _gameplayUI.TurnButtonEnable = false;
                _gameplayUI.HideHand(_deckController.Hand);
                _deckController.DiscardAllHand();
                _gameplayUI.SetDiscardCount(_deckController.Discard.Count);
                EnemyTurn(_enemyController.GetEnemyActions());
            }
            else
            {
                TryShuffle();
                _deckController.GetHand(5);
                _playerController.ResetEnergy();
                _gameplayUI.ShowHand(_deckController.Hand);
                _gameplayUI.SetDeckCount(_deckController.Deck.Count);
                _gameplayUI.SetEnergyCount(_playerController.GetEnergy());
                _gameplayUI.TurnButtonEnable = true;
            }
        }

        private void TryShuffle()
        {
            if (_deckController.Deck.Count >= 5) return;
            
            _deckController.Shuffle();
            _gameplayUI.SetDiscardCount(_deckController.Discard.Count);
        }

        private void OnCardInteract(Card card)
        {
            if(!CheckPlayerEnergy(card)) return;
            
            OnCardInteractAsync(card).Forget();
        }
        
        private async UniTask OnCardInteractAsync(Card card)
        {
            if (card.ActionType == ActionType.Attack)
            {
                _gameplayUI.HighlightCard(card);
                _gameplayUI.EnableArrow(true);
                
                await UniTask.WaitUntil(() => card.Target != null);
                
                _gameplayUI.EnableArrow(false);
                ActivateCard(card);
                return;
            }
            
            _gameplayUI.HighlightCard(card);
            await UniTask.WaitForSeconds(1f);
            ActivateCard(card);
        }

        private void ActivateCard(Card card)
        {
            switch (card.ActionType)
            {
                case ActionType.Attack:
                    _playerController.SetAnimation("Attack", false);
                    _enemyController.Damage(-card.Value, card.Target);
                    _playerController.SetAnimation("Idle", false);
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

            if (CheckSwitchTurnConditions())
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
                        _enemyController.SetAnimation("Attack", false, action.Enemy);
                        _playerController.Damage(-action.Value);
                        _enemyController.SetAnimation("Idle", true, action.Enemy);
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
        
        private bool CheckPlayerEnergy(Card card)
        {
            return _playerController.GetEnergy() >= card.Cost;
        }

        private bool CheckEmptyHand()
        {
            return _deckController.Hand.Count <= 0;
        }

        private bool CheckSwitchTurnConditions()
        {
            return CheckEmptyHand() || _playerController.GetEnergy() <= 0;
        }
    }
}