using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        private Queue<Room> _rooms = new ();
        public Room CurrentRoom { get; private set; }

        public void Initialize()
        {
            foreach (var roomPrefab in _prefabDataBase.Rooms)
            {
                var room = _container.Instantiate(roomPrefab);
                _rooms.Enqueue(room);
            }

            CurrentRoom = _rooms.Dequeue();
            CurrentRoom.transform.position = Vector3.zero;
        }

        public async UniTask<bool> NextRoom()
        {
            if (!_rooms.TryDequeue(out var currentRoom))
            {
                return false;
            }

            var previousRoom = CurrentRoom;
            previousRoom.BackGround.sortingOrder--;
            CurrentRoom = currentRoom;
            CurrentRoom.transform.position = new Vector3(50f, 0); // magic number)

            await CurrentRoom.transform.DOMove(Vector3.zero, 2).AsyncWaitForCompletion().AsUniTask();
            
            Object.Destroy(previousRoom);
            return true;
        }
    }
}