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
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SceneSwitcher>(Lifetime.Singleton);
            builder.Register<PrefabDataBase>(Lifetime.Singleton);
        }
    }
}