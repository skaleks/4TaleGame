using UnityProject.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts
{
    public class MainMenuEntryPoint : IStartable
    {
        [Inject] private readonly MainMenuUI _menuUI;
        [Inject] private readonly MainMenuUIPresenter _mainMenuUIPresenter;

        public void Start()
        {
            _menuUI.StartGameButton.onClick.AddListener(() => _mainMenuUIPresenter.StartGame());
            _menuUI.QuitGameButton.onClick.AddListener(() => _mainMenuUIPresenter.QuitGame());
        }
    }
}