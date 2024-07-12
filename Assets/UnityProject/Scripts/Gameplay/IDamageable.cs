using UnityProject.Scripts.Gameplay.View;

namespace UnityProject.Scripts.Gameplay
{
    public interface IDamageHandler
    {
        void Damage(float value, Character character = null);
    }
}