using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityProject.Scripts.UI
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private Button _cardButton;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private TMP_Text _typeText;

        public Button CardButton => _cardButton;
        public TMP_Text CostText => _costText;
        public TMP_Text ValueText => _valueText;
        public TMP_Text TypeText => _typeText;
    }
}