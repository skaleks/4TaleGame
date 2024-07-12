using UnityEngine;

namespace UnityProject.Scripts.Gameplay.View
{
    public class Character : MonoBehaviour, IInteractable
    {
        [SerializeField] protected SpriteRenderer _healthBar;
        [SerializeField] protected SpriteRenderer _armorBar;
        
        public SpriteRenderer HealthBar => _healthBar;
        public SpriteRenderer ArmorBar => _armorBar;
    }
}