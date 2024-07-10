﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityProject.Scripts.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private Button _backToMainMenuButton;
        [SerializeField] private TMP_Text _discardCount;
        [SerializeField] private TMP_Text _deckCount;
        [SerializeField] private TMP_Text _actionPointsText;
        [SerializeField] private Image _actionPointsImage;
        [SerializeField] private RectTransform _cardPlaceHolder;

        public Button BackToMainMenuButton => _backToMainMenuButton;
        public TMP_Text DiscardCount => _discardCount;
        public TMP_Text DeckCount => _deckCount;
        public TMP_Text ActionPointsText => _actionPointsText;
        public Image ActionPointsImage => _actionPointsImage;
        public RectTransform CardPlaceHolder => _cardPlaceHolder;
    }
}