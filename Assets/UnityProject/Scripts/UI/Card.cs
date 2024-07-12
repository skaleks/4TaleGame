using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityProject.Scripts.Gameplay.Model;
using UnityProject.Scripts.Gameplay.View;

namespace UnityProject.Scripts.UI
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button _cardButton;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private TMP_Text _typeText;
        
        [HideInInspector] public CardData CardData;
        [HideInInspector] public bool IsChoosen;
        [HideInInspector] public Character Target;

        public RectTransform RectTransform => _rectTransform;
        public Button CardButton => _cardButton;
        public TMP_Text CostText => _costText;
        public TMP_Text ValueText => _valueText;
        public TMP_Text TypeText => _typeText;
    }
}