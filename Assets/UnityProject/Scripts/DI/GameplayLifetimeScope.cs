using UnityEngine;
using UnityProject.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts.DI
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameplayUI _gameplayUI;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayEntryPoint>();
            builder.Register<GameplayUIPresenter>(Lifetime.Singleton);
            builder.RegisterComponent(_gameplayUI);
        }
    }
}