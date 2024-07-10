using UnityProject.Scripts.Gameplay.Model;
using UnityProject.Scripts.Gameplay.View;
using VContainer;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class EnemyController 
    {
        [Inject] private RoomController _roomController;
        [Inject] private CharacterSpawner _characterSpawner;

        private Character _enemy;
        private EnemyData _enemyData;

        public void Initialize()
        {
            _enemy = _characterSpawner.InstantiateEnemy(_roomController.Room.EnemySpawnPoints[0].position);
        }
    }
}