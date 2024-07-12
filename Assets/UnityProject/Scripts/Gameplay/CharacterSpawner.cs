using UnityEngine;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.Gameplay.View;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts.Gameplay
{
    public class CharacterSpawner
    {
        [Inject] private readonly PrefabDataBase _prefabDataBase;
        [Inject] private readonly IObjectResolver _container;

        public Character InstantiatePlayer(Vector3 position)
        {
            return _container.Instantiate(_prefabDataBase.Player, position, Quaternion.identity);
        }

        public Enemy InstantiateEnemy(Vector3 position)
        {
            return _container.Instantiate(_prefabDataBase.Enemy, position, Quaternion.identity);
        }
    }
}