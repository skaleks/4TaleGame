using UnityEngine;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.Gameplay.View;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class RoomController
    {
        [Inject] private readonly PrefabDataBase _prefabDataBase;
        [Inject] private readonly IObjectResolver _container;
        
        public Room Room { get; private set; }

        public void InstantiateRoom()
        {
            Room = _container.Instantiate(_prefabDataBase.Rooms[0], Vector3.zero, Quaternion.identity);
        }
    }
}