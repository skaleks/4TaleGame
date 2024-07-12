using System.Collections.Generic;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.UI;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class DeckController
    {
        [Inject] private DefaultProfile _defaultProfile;
        [Inject] private PrefabDataBase _prefabDataBase;
        [Inject] private CardDataBase _cardDataBase;
        [Inject] private IObjectResolver _container;

        public Queue<Card> Deck { get; private set; } = new();
        public List<Card> Hand { get; private set; } = new();
        public List<Card> Discard { get; private set; } = new();

        
        public void InstantiateDeck()
        {
            for (int i = 0; i < _defaultProfile.PlayerData.DeckCapacity; i++)
            {
                Create();
            }
        }

        public void Clear()
        {
            Deck.Clear();
            Hand.Clear();
            Discard.Clear();
        }
        
        public void Shuffle()
        {
            foreach (var card in Discard)
            {
                Deck.Enqueue(card);
            }
            
            Discard.Clear();
        }

        public void GetHand(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if(!Deck.TryDequeue(out var card)) break;
                Hand.Add(card);
            }
        }
        
        public void DiscardCard(Card card)
        {
            Hand.Remove(card);
            Discard.Add(card);
        }

        public void DiscardAllHand()
        {
            foreach (var card in Hand)
            {
                Discard.Add(card);
            }
            
            Hand.Clear();
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
            
            Deck.Enqueue(card);
        }
    }
}