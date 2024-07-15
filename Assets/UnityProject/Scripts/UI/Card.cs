using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityProject.Scripts.Enums;
using UnityProject.Scripts.Gameplay.Model;
using UnityProject.Scripts.Gameplay.View;

namespace UnityProject.Scripts.UI
{
    public class Card : MonoBehaviour, IEndDragHandler, IDragHandler, IPointerDownHandler, 
        IPointerUpHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _background;
        [SerializeField] private Image _typeImage;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _valueText;

        private CardModel _cardModel;
        
        public RectTransform RectTransform => _rectTransform;
        public ActionType ActionType => _cardModel.CardData.ActionType;
        public int Cost => _cardModel.CardData.Cost;
        public float Value => _cardModel.CardData.Value;
        public Character Target { get; set; }
        
        public Action<Card> OnPointDownAction;
        public Action<Card> OnPointUpAction;
        public Action<Card> OnEndDragAction;

        public void Initialize(CardModel cardModel)
        {
            _cardModel = cardModel;
            gameObject.SetActive(false);
            
            _costText.text = cardModel.CardData.Cost.ToString();
            _valueText.text = cardModel.CardData.Value.ToString();
            _typeImage.sprite = cardModel.CardData.TypeImage;
            
            _background.color = ActionType switch
            {
                ActionType.Attack => Color.red,
                ActionType.Defense => Color.yellow,
                ActionType.Healing => Color.green,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Vector2.Distance(eventData.pressPosition, eventData.position) > RectTransform.rect.height / 2)
            {
                OnEndDragAction?.Invoke(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform.position = new Vector3(RectTransform.position.x, eventData.position.y);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointDownAction?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointUpAction?.Invoke(this);
        }
    }
}