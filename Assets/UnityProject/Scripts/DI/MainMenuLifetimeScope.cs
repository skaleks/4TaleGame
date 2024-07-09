using UnityEngine;
using UnityProject.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts.DI
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainMenuUI _mainMenuUI;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<MainMenuEntryPoint>();
            builder.Register<MainMenuService>(Lifetime.Singleton);
            builder.RegisterComponent(_mainMenuUI);
        }
    }
}