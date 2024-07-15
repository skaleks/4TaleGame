using System;
using UnityEngine;
using UnityProject.Scripts.Enums;

namespace UnityProject.Scripts.Gameplay.Model
{
    [Serializable]
    public sealed class CardData
    {
        [SerializeField] private ActionType _actionType;
        [SerializeField] private int _cost;
        [SerializeField] private int _value;
        [SerializeField] private Sprite _typeImage;

        public ActionType ActionType => _actionType;
        public int Cost => _cost;
        public int Value => _value;
        public Sprite TypeImage => _typeImage;
    }
}