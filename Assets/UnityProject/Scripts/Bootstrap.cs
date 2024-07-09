using UnityEngine;
using UnityProject.Scripts.Common;
using UnityProject.Scripts.Common.Enums;
using VContainer;

namespace UnityProject.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private SceneSwitcher _sceneSwitcher;
        
        public void Start()
        {
            _sceneSwitcher.Switch(SceneType.MainMenu);
        }
    }
}