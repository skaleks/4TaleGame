using UnityEngine.SceneManagement;
using UnityProject.Scripts.Common.Enums;

namespace UnityProject.Scripts.Common
{
    public class SceneSwitcher
    {
        public void Switch(SceneType sceneType)
        {
            SceneManager.LoadScene(sceneType.ToString());
        }
    }
}