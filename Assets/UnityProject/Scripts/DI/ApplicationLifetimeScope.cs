using UnityEngine;
using UnityProject.Scripts.Common;
using UnityProject.Scripts.Data;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts.DI
{
    public class ApplicationLifetimeScope : LifetimeScope
    {
        [SerializeField] private PrefabDataBase _prefabDataBase;
        [SerializeField] private CardDataBase _cardDataBase;
        [SerializeField] private DefaultProfile _defaultProfile;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneSwitcher>(Lifetime.Singleton);
            builder.RegisterInstance(_defaultProfile);
            builder.RegisterInstance(_prefabDataBase);
            builder.RegisterInstance(_cardDataBase);
        }
    }
}