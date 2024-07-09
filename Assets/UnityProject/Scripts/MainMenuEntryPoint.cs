using UnityProject.Scripts.UI;
using VContainer.Unity;

namespace UnityProject.Scripts
{
    public class MainMenuEntryPoint : IStartable
    {
        private MainMenuUI _menuUI;
        private MainMenuService _mainMenuService;

        public MainMenuEntryPoint(MainMenuUI menuUI, MainMenuService mainMenuService)
        {
            _menuUI = menuUI;
            _mainMenuService = mainMenuService;
        }

        public void Start()
        {
            _menuUI.StartGameButton.onClick.AddListener(() => _mainMenuService.StartGame());
        }
    }
}