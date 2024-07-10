using UnityEngine;
using UnityEngine.Pool;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class DeckController
    {
        [Inject] private PrefabDataBase _prefabDataBase;
        [Inject] private CardDataBase _cardDataBase;
        [Inject] private IObjectResolver _container;
        [Inject] private GameplayUIPresenter _gameplayUI;

        private ObjectPool<Card> _deckPool;

        public void InstantiateDeck()
        {
            _deckPool = new ObjectPool<Card>(CreateFunc, actionOnGet: Get, actionOnRelease: Release);
            GetNewCard(5);
        }

        public void GetNewCard(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var card = _deckPool.Get();
                _gameplayUI.AddCard(card);
            }
        }

        private void Get(Card obj)
        {
            var cardData = _cardDataBase.Cards[Random.Range(0, _cardDataBase.Cards.Count)];
            
            obj.CostText.text = cardData.Cost.ToString();
            obj.TypeText.text = cardData.CardType.ToString();
            obj.ValueText.text = cardData.Value.ToString();
            obj.gameObject.SetActive(true);
        }

        private void Release(Card obj)
        {
            obj.gameObject.SetActive(false);
        }

        private Card CreateFunc()
        {
            return _container.Instantiate(_prefabDataBase.Card);
        }
    }
}