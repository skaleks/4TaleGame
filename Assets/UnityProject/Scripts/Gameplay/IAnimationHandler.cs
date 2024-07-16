using UnityProject.Scripts.Gameplay.View;

namespace UnityProject.Scripts.Gameplay
{
    public interface IAnimationHandler
    {
        void PlayAnimation(string name, bool replace ,bool loop, Character character = null);
    }
}