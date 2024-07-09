using UnityProject.Scripts.UI;
using VContainer.Unity;

namespace UnityProject.Scripts
{
    public class MainMenuEntryPoint : IStartable
    {
        private MainMenuUI _menuUI;
        private MainMenuUIPresenter _mainMenuService;

        public MainMenuEntryPoint(MainMenuUI menuUI, MainMenuUIPresenter mainMenuService)
        {
            _menuUI = menuUI;
            _mainMenuService = mainMenuService;
        }

        public void Start()
        {
            _menuUI.StartGameButton.onClick.AddListener(() => _mainMenuService.StartGame());
            _menuUI.QuitGameButton.onClick.AddListener(() => _mainMenuService.QuitGame());
        }
    }
}