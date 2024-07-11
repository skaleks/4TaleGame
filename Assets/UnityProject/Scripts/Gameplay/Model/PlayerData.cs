using System;

namespace UnityProject.Scripts.Gameplay.Model
{
    [Serializable]
    public sealed class PlayerData
    {
        public int Health;
        public int MaxHealth;
        public int Energy;
        public int Armor;
        public int DeckCapacity;
    }
}