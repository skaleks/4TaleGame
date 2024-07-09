using UnityProject.Scripts.Common;
using UnityProject.Scripts.Common.Enums;
using VContainer;

namespace UnityProject.Scripts
{
    public class MainMenuService
    {
        [Inject] private SceneSwitcher _sceneSwitcher;
        
        public void StartGame()
        {
            _sceneSwitcher.Switch(SceneType.Gameplay);
        }
    }
}