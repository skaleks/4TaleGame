using UnityEngine;

namespace UnityProject.Scripts.Gameplay.View
{
    public class Character : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _body;
        [SerializeField] protected SpriteRenderer _healthBar;
        [SerializeField] protected SpriteRenderer _armorBar;

        public SpriteRenderer Body => _body;
        public SpriteRenderer HealthBar => _healthBar;
        public SpriteRenderer ArmorBar => _armorBar;
    }
}