using UnityEngine;
using UnityEngine.UI;

namespace UnityProject.Scripts.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _quitGameButton;

        public Button StartGameButton => _startGameButton;
        public Button QuitGameButton => _quitGameButton;
    }
}