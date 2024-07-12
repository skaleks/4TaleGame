using System;
using UnityProject.Scripts.Enums;
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

        private bool _cardChoosen;
        private Turn TurnOrder { get; set; } = Turn.Player;
        
        public void Initialize()
        {
            _gameplayUI.OnCardInteract += OnCardInteract;
            _gameplayUI.OnCardActivate += OnCardActivate;
            _gameplayUI.OnEndTurnButtonClick += SwitchTurn;
            StartBattle();
        }

        public void Dispose()
        {
            _gameplayUI.OnCardInteract -= OnCardInteract;
            _gameplayUI.OnCardActivate -= OnCardActivate;
            _gameplayUI.OnEndTurnButtonClick -= SwitchTurn;
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

        private void Finish()
        {
            _roomController.InstantiateRoom();
        }

        private void SwitchTurn()
        {
            TurnOrder = TurnOrder == Turn.Player ? Turn.Enemy : Turn.Player;
            
            if (TurnOrder == Turn.Enemy)
            {
                _gameplayUI.TurnButtonEnable = false;
                EnemyAction(_enemyController.Action());
            }
            else
            {
                TryShuffle();
                _deckController.GetHand(5);
                _playerController.ChangeEnergy(5);
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
            
            if (card.IsChoosen)
            {
                _deckController.DiscardCard(card);
                _gameplayUI.ActivateCard(card);
                card.IsChoosen = false;
                return;
            }
            
            _gameplayUI.UnHighlightCard();
            _gameplayUI.HighlightCard(card);
            card.IsChoosen = true;
        }

        private void OnCardActivate(Card card)
        {
            var data = card.CardData;
            
            switch (data.CardType)
            {
                case ActionType.Attack:
                    _enemyController.ChangeHealth(-data.Value);
                    break;
                case ActionType.Defense:
                    _playerController.ChangeArmor(data.Value);
                    break;
                case ActionType.Healing:
                    _playerController.ChangeHealth(data.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _playerController.ChangeEnergy(-data.Cost);
            _gameplayUI.SetEnergyCount(_playerController.GetEnergy());

            if (CheckSwitchTurnConditions())
            {
                SwitchTurn();
            }
        }

        private void EnemyAction(EnemyAction enemyAction)
        {
            switch (enemyAction.ActionType)
            {
                case ActionType.Attack:
                    _playerController.Damage(-enemyAction.Value);
                    break;
                case ActionType.Defense:
                    _enemyController.ChangeArmor(enemyAction.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            SwitchTurn();
        }
        
        private bool CheckPlayerEnergy(Card card)
        {
            return _playerController.GetEnergy() >= card.CardData.Cost;
        }

        private bool CheckEmptyHand()
        {
            return _deckController.Hand.Count <= 0;
        }
        
        private bool CheckEnemiesDefeated()
        {
            return _enemyController.AllEnemiesDefeated;
        }

        private bool CheckSwitchTurnConditions()
        {
            return CheckEmptyHand() || CheckEnemiesDefeated() || _playerController.GetEnergy() <= 0;
        }
    }
}