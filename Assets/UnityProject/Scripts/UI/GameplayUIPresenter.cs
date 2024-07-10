using System.Collections.Generic;
using UnityEngine;
using UnityProject.Scripts.Common;
using UnityProject.Scripts.Common.Enums;
using VContainer;

namespace UnityProject.Scripts.UI
{
    public class GameplayUIPresenter
    {
        [Inject] private SceneSwitcher _sceneSwitcher;
        [Inject] private GameplayUI _gameplayUI;

        private List<Card> _cards = new ();

        public void Initialize() => _gameplayUI.BackToMainMenuButton.onClick.AddListener(BackToMainMenu);
        public void ChangeActionPoints(int value) => _gameplayUI.ActionPointsText.text = value.ToString();
        public void ChangeDeckCount(int value) => _gameplayUI.DeckCount.text = value.ToString();
        public void ChangeDiscardCount(int value) => _gameplayUI.DiscardCount.text = value.ToString();

        public void AddCard(Card card)
        {
            var center =_gameplayUI.CardPlaceHolder.rect.center;
            
            card.transform.SetParent(_gameplayUI.CardPlaceHolder);
            card.transform.localPosition = new Vector2(center.x + _cards.Count * 100, 0);
            _cards.Add(card);
        }

        private void BackToMainMenu() => _sceneSwitcher.Switch(SceneType.MainMenu);
    }
}