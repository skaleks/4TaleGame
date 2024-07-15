using UnityProject.Scripts.Gameplay.View;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public interface IAnimationHandler
    {
        void SetAnimation(string name, bool loop, Character character = null);
    }
}