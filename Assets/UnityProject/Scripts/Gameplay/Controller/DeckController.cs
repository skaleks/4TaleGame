using System;
using System.Collections.Generic;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.UI;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class DeckController : IDisposable
    {
        [Inject] private DefaultProfile _defaultProfile;
        [Inject] private PrefabDataBase _prefabDataBase;
        [Inject] private CardDataBase _cardDataBase;
        [Inject] private IObjectResolver _container;
        [Inject] private GameplayUIPresenter _gameplayUI;
        
        private Queue<Card> _deck = new();
        private List<Card> _hand = new();
        private List<Card> _discard = new();
        private bool _cardChoosen;

        public void Initialize()
        {
            _gameplayUI.OnCardInteract += DoAction;
            InstantiateDeck();
            PrepareUI();
            PrepareHand(5);
        }

        public void Dispose()
        {
            _gameplayUI.OnCardInteract -= DoAction;
        }
        
        public void InstantiateDeck()
        {
            for (int i = 0; i < _defaultProfile.PlayerData.DeckCapacity; i++)
            {
                Create();
            }
        }

        private void PrepareUI()
        {
            _gameplayUI.RegisterCards(_deck);
        }
        
        public void Clear()
        {
            _deck.Clear();
            _discard.Clear();
        }

        private void PrepareHand(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var card = _deck.Dequeue();
                _hand.Add(card);
            }
            _gameplayUI.ShowHand(_hand);
        }

        public void Shuffle()
        {
            foreach (var card in _discard)
            {
                _deck.Enqueue(card);
            }
            
            _discard.Clear();
        }

        private void Create()
        {
            var cardData = _cardDataBase.Cards[Random.Range(0, _cardDataBase.Cards.Count)];
            
            var card = _container.Instantiate(_prefabDataBase.Card);
            card.gameObject.SetActive(false);
            card.CostText.text = cardData.Cost.ToString();
            card.TypeText.text = cardData.CardType.ToString();
            card.ValueText.text = cardData.Value.ToString();
            card.CardData = cardData;
            
            _deck.Enqueue(card);
        }
        
        private void DoAction(Card card)
        {
            if (_cardChoosen)
            {
                ActivateCard(card);
                return;
            }
            
            _gameplayUI.HighlightCard(card);
            _cardChoosen = true;
        }

        private void ActivateCard(Card card)
        {
            _gameplayUI.ActivateCard(card);
            DiscardCard(card);
            _cardChoosen = false;
        }

        private void DiscardCard(Card card)
        {
            _hand.Remove(card);
            _discard.Add(card);
        }
    }
}