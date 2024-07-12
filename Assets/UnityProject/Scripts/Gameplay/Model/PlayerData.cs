using System;

namespace UnityProject.Scripts.Gameplay.Model
{
    [Serializable]
    public sealed class PlayerData
    {
        public float Health;
        public float MaxHealth;
        public float Energy;
        public float Armor;
        public int DeckCapacity;
    }
}