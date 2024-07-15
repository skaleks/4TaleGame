using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityProject.Scripts.Common;
using UnityProject.Scripts.Common.Enums;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.Enums;
using VContainer;
using Object = UnityEngine.Object;

namespace UnityProject.Scripts.UI
{
    public sealed class GameplayUIPresenter : IDisposable
    {
        [Inject] private SceneSwitcher _sceneSwitcher;
        [Inject] private GameplayUI _gameplayUI;
        [Inject] private PrefabDataBase _prefabDataBase;

        private HorizontalLayoutGroup _cardPlaceholderLayout;
        private const int MaxCardsInHand = 10;
        private int _discardCount;
        private CancellationTokenSource _cancellationTokenSource;
        private Arrow _arrow;

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
            _arrow = Object.Instantiate(_prefabDataBase.Arrow, _gameplayUI.transform);
            _arrow.gameObject.SetActive(false);
            _cancellationTokenSource = new CancellationTokenSource();
            _cardPlaceholderLayout = _gameplayUI.CardPlaceHolder.GetComponent<HorizontalLayoutGroup>();
            _gameplayUI.BackToMainMenuButton.onClick.AddListener(BackToMainMenu);
            _gameplayUI.EndTurnButton.onClick.AddListener(EndTurn);
        }

        public void Dispose()
        {
            _gameplayUI.BackToMainMenuButton.onClick.RemoveListener(BackToMainMenu);
            _gameplayUI.EndTurnButton.onClick.RemoveListener(EndTurn);

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public void RegisterCards(Queue<Card> deck)
        {
            foreach (var card in deck)
            {
                card.OnEndDragAction += OnEndDragAction;
                card.OnPointDownAction += OnPointDownAction;
                card.OnPointUpAction += OnPointUpAction;
            }
        }

        public void ShowGameOver() => _gameplayUI.GameOverText.enabled = true;

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
            card.RectTransform.SetParent(_gameplayUI.transform);
            card.RectTransform.anchorMin = card.RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            card.RectTransform.anchoredPosition = Vector2.zero;
        }

        public void EnableArrow(bool value)
        {
            _arrow.gameObject.SetActive(value);
        }

        private void OnPointUpAction(Card card)
        {
            _cardPlaceholderLayout.enabled = true;
        }

        private void OnPointDownAction(Card card)
        {
            _cardPlaceholderLayout.enabled = false;
        }

        private void OnEndDragAction(Card card)
        {
            OnCardInteract?.Invoke(card);
        }

        private void BackToMainMenu() => _sceneSwitcher.Switch(SceneType.MainMenu);

        private void EndTurn() => OnEndTurnButtonClick?.Invoke();
    }
}