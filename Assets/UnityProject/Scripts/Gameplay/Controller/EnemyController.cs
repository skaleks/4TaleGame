using UnityEngine;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.Enums;
using UnityProject.Scripts.Gameplay.Model;
using UnityProject.Scripts.Gameplay.View;
using VContainer;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class EnemyController 
    {
        [Inject] private RoomController _roomController;
        [Inject] private CharacterSpawner _characterSpawner;
        [Inject] private EnemyDataBase _enemyDataBase;

        private Character _enemy;
        private EnemyData _enemyData;
        public bool AllEnemiesDefeated { get; private set; }

        public void Initialize()
        {
            _enemy = _characterSpawner.InstantiateEnemy(_roomController.Room.EnemySpawnPoints[0].position);
            
            var enemyData = _enemyDataBase.EnemyDataList[Random.Range(0, _enemyDataBase.EnemyDataList.Count)];
            
            _enemyData = new EnemyData()
            {
                Armor = enemyData.Armor,
                Health = enemyData.Health,
                MaxHealth = enemyData.MaxHealth
            };
        }

        public EnemyAction Action()
        {
            return new EnemyAction(ActionType.Attack, 10);
        }

        public void ChangeArmor(int value)
        {
            _enemyData.Armor += value;
            
            var size = _enemy.ArmorBar.size;
            size = new Vector2(_enemyData.Armor / 100, size.y);
            _enemy.ArmorBar.size = size;
        }

        public void ChangeHealth(int value)
        {
            _enemyData.Health += value;
            
            var size = _enemy.HealthBar.size;
            size = new Vector2(size.x / 100 * _enemyData.Health, size.y);
            _enemy.HealthBar.size = size;
        }
    }

    public struct EnemyAction
    {
        public int Value;
        public ActionType ActionType;

        public EnemyAction(ActionType actionType, int value)
        {
            Value = value;
            ActionType = actionType;
        }
    }
}