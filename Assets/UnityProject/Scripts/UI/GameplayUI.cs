using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityProject.Scripts.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private Button _backToMainMenuButton;
        [SerializeField] private Button _endTurnButton;
        [SerializeField] private TMP_Text _discardCount;
        [SerializeField] private TMP_Text _deckCount;
        [SerializeField] private TMP_Text _actionPointsText;
        [SerializeField] private TMP_Text _gameOverText;
        [SerializeField] private TMP_Text _chooseTarget;
        [SerializeField] private RectTransform _cardPlaceHolder;

        public Button BackToMainMenuButton => _backToMainMenuButton;
        public Button EndTurnButton => _endTurnButton;
        public TMP_Text DiscardCount => _discardCount;
        public TMP_Text DeckCount => _deckCount;
        public TMP_Text ActionPointsText => _actionPointsText;
        public RectTransform CardPlaceHolder => _cardPlaceHolder;
        public TMP_Text GameOverText => _gameOverText;
        public TMP_Text ChooseTargetText => _chooseTarget;
    }
}