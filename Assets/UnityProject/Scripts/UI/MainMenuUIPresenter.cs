using UnityEngine;
using UnityProject.Scripts.Common;
using UnityProject.Scripts.Common.Enums;
using VContainer;

namespace UnityProject.Scripts.UI
{
    public class MainMenuUIPresenter
    {
        [Inject] private SceneSwitcher _sceneSwitcher;
        
        public void StartGame()
        {
            _sceneSwitcher.Switch(SceneType.Gameplay);
        }

        public void QuitGame()
        {
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
            
        }
    }
}