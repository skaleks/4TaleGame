using System;
using UnityProject.Scripts.Enums;

namespace UnityProject.Scripts.Gameplay.Model
{
    [Serializable]
    public sealed class CardData
    {
        public ActionType CardType;
        public int Cost;
        public int Value;
    }
}