using UnityProject.Scripts.Data;
using UnityProject.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts
{
    public class GameplayEntryPoint : IStartable
    {
        private GameplayUI _gameplayUI;
        private GameplayUIPresenter _gameplayUIPresenter;
        private PrefabDataBase _prefabDataBase;
        private IObjectResolver _container;

        public GameplayEntryPoint(IObjectResolver container, PrefabDataBase prefabDataBase)
        {
            _prefabDataBase = prefabDataBase;
        }

        public void Start()
        {
            
        }
    }
}