using UnityEngine;
using UnityEngine.UI;

namespace UnityProject.Scripts.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;

        public Button StartGameButton => _startGameButton;
    }
}