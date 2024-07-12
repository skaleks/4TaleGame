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
        private Card _chosenCard;

        public bool TurnButtonEnable
        {
            private get => _gameplayUI.EndTurnButton.enabled;
            set => _gameplayUI.EndTurnButton.enabled = value;
        }

        public event Action<Card> OnCardInteract;
        public event Action<Card> OnCardActivate;
        public event Action OnEndTurnButtonClick;

        public void Initialize()
        {
            _cardPlaceholderLayout = _gameplayUI.CardPlaceHolder.GetComponent<HorizontalLayoutGroup>();
            _gameplayUI.BackToMainMenuButton.onClick.AddListener(BackToMainMenu);
            _gameplayUI.EndTurnButton.onClick.AddListener(EndTurn);
        }

        public void Dispose()
        {
            _gameplayUI.BackToMainMenuButton.onClick.RemoveListener(BackToMainMenu);
            _gameplayUI.EndTurnButton.onClick.RemoveListener(EndTurn);
            
            foreach (var card in _deck)
            {
                card.CardButton.onClick.RemoveListener(() => OnCardInteract?.Invoke(card));
            }
        }

        public void RegisterCards(Queue<Card> deck)
        {
            _deck = deck;
            
            foreach (var card in deck)
            {
                card.CardButton.onClick.AddListener(() => OnCardInteract?.Invoke(card));
            }
        }

        public void SetEnergyCount(float value) => _gameplayUI.ActionPointsText.text = value.ToString();
        
        public void SetDeckCount(int value) => _gameplayUI.DeckCount.text = value.ToString();

        public void SetDiscardCount(int value)
        {
            _discardCount = value;
            _gameplayUI.DiscardCount.text = value.ToString();
        }

        public void ShowHand(List<Card> hand)
        {
            foreach (var card in hand)
            {
                card.RectTransform.SetParent(_gameplayUI.CardPlaceHolder);
                card.gameObject.SetActive(true);
                _cardPlaceholderLayout.spacing = (MaxCardsInHand - hand.Count) * -120; // Захардкожено, включая использование HorizontalLayoutGroup
            }
        }

        public void HideHand(List<Card> hand)
        {
            foreach (var card in hand)
            {
                card.RectTransform.SetParent(null);
                card.gameObject.SetActive(false);
            }
        }

        public void ActivateCard(Card card)
        {
            card.gameObject.SetActive(false);
            card.transform.SetParent(null);
            SetDiscardCount(++_discardCount);
            
            OnCardActivate?.Invoke(card);
        }

        public void HighlightCard(Card card)
        {
            _chosenCard = card;
            card.RectTransform.SetParent(_gameplayUI.transform);
            card.RectTransform.anchorMin = card.RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            card.RectTransform.anchoredPosition = Vector2.zero;
        }

        public void UnHighlightCard()
        {
            if (_chosenCard == null) return;
            
            _chosenCard.IsChoosen = false;
            _chosenCard.RectTransform.SetParent(_gameplayUI.CardPlaceHolder);
        }

        public void ShowGameOver()
        {
            _gameplayUI.GameOverText.enabled = true;
        }

        private void BackToMainMenu() => _sceneSwitcher.Switch(SceneType.MainMenu);

        private void EndTurn() => OnEndTurnButtonClick?.Invoke();
    }
}