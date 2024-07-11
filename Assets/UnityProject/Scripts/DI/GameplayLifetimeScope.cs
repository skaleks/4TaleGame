using UnityEngine;
using UnityProject.Scripts.Gameplay;
using UnityProject.Scripts.Gameplay.Controller;
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
            builder.RegisterComponent(_gameplayUI);
            
            builder.Register<GameplayUIPresenter>(Lifetime.Singleton);
            builder.Register<CharacterSpawner>(Lifetime.Singleton);
            builder.Register<RoomController>(Lifetime.Singleton);
            builder.Register<PlayerController>(Lifetime.Singleton);
            builder.Register<EnemyController>(Lifetime.Singleton);
            builder.Register<DeckController>(Lifetime.Singleton);
            builder.Register<BattleController>(Lifetime.Singleton);
        }
    }
}