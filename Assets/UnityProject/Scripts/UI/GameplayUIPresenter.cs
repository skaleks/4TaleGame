using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityProject.Scripts.Common;
using UnityProject.Scripts.Common.Enums;
using VContainer;

namespace UnityProject.Scripts.UI
{
    public sealed class GameplayUIPresenter : IDisposable
    {
        [Inject] private SceneSwitcher _sceneSwitcher;
        [Inject] private GameplayUI _gameplayUI;

        private HorizontalLayoutGroup _cardPlaceholderLayout;
        private const int MaxCardsInHand = 10;
        private int _discardCount;
        private Queue<Card> _deck;

        public event Action<Card> OnCardInteract;
        public event Action<Card> OnCardActivate;

        public void Initialize()
        {
            _cardPlaceholderLayout = _gameplayUI.CardPlaceHolder.GetComponent<HorizontalLayoutGroup>();
            _gameplayUI.BackToMainMenuButton.onClick.AddListener(BackToMainMenu);
        }

        public void Dispose()
        {
            _gameplayUI.BackToMainMenuButton.onClick.RemoveListener(BackToMainMenu);
            
            foreach (var card in _deck)
            {
                card.CardButton.onClick.RemoveListener(() => OnCardClick(card));
            }
        }

        private void OnCardClick(Card card)
        {
            OnCardInteract?.Invoke(card);
        }

        public void RegisterCards(Queue<Card> deck)
        {
            _deck = deck;
            
            foreach (var card in deck)
            {
                card.CardButton.onClick.AddListener(() => OnCardInteract?.Invoke(card));
            }
        }

        public void SetActionPoints(int value) => _gameplayUI.ActionPointsText.text = value.ToString();
        
        public void SetDeckCount(int value) => _gameplayUI.DeckCount.text = value.ToString();
        
        public void SetDiscardCount(int value) => _gameplayUI.DiscardCount.text = value.ToString();

        public void ShowHand(List<Card> hand)
        {
            foreach (var card in hand)
            {
                card.RectTransform.SetParent(_gameplayUI.CardPlaceHolder);
                card.gameObject.SetActive(true);
                _cardPlaceholderLayout.spacing = (MaxCardsInHand - hand.Count) * -120; // Захардкожено, включая использование HorizontalLayoutGroup
                SetDeckCount(_deck.Count);
            }
        }

        public void ActivateCard(Card card)
        {
            OnCardActivate?.Invoke(card);
            card.gameObject.SetActive(false);
            card.transform.SetParent(null);
            SetDiscardCount(++_discardCount);
            // включить карты, но в другом месте по логике
        }
        
        public void HighlightCard(Card card)
        {
            card.RectTransform.SetParent(_gameplayUI.transform);
            card.RectTransform.anchorMin = card.RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            card.RectTransform.anchoredPosition = Vector2.zero;
            // выключить карты, но в другом месте по логике
        }
        
        private void BackToMainMenu() => _sceneSwitcher.Switch(SceneType.MainMenu);
    }
}