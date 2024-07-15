using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.Enums;
using UnityProject.Scripts.Gameplay.Model;
using UnityProject.Scripts.Gameplay.View;
using VContainer;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class EnemyController : IDamageHandler, IAnimationHandler
    {
        [Inject] private RoomController _roomController;
        [Inject] private CharacterSpawner _characterSpawner;
        [Inject] private EnemyDataBase _enemyDataBase;

        private List<Enemy> _enemies = new();
        public event Action OnAllEnemiesDead;
        private bool _isTargetDefine;
        private Enemy _targetEnemy;

        public void Initialize()
        {
            foreach (var spawnPoint in _roomController.Room.EnemySpawnPoints)
            {
                var enemy = _characterSpawner.InstantiateEnemy(spawnPoint.position);
                var enemyData = _enemyDataBase.EnemyDataList[Random.Range(0, _enemyDataBase.EnemyDataList.Count)];
                
                enemy.Initialize(new EnemyData
                {
                    ActionValue = enemyData.ActionValue,
                    Armor =  enemyData.Armor,
                    Health = enemyData.Health,
                    MaxHealth = enemyData.MaxHealth
                });

                ChangeHealthBar(enemy);
                ChangeArmorBar(enemy);
                
                _enemies.Add(enemy);
                SetAnimation("Idle", true, enemy);
            }
        }

        public List<EnemyAction> GetEnemyActions()
        {
            var enemyAction = new List<EnemyAction>();
            
            foreach (var enemy in _enemies)
            {
                var actionType = enemy.EnemyData.Health < enemy.EnemyData.MaxHealth / 2
                    ? ActionType.Defense
                    : ActionType.Attack;
                
                enemyAction.Add(new EnemyAction(enemy, actionType , enemy.EnemyData.ActionValue));
            }

            return enemyAction;
        }

        public float ChangeArmor(Enemy enemy, float value)
        {
            SetAnimation("Skill", false, enemy);
            enemy.EnemyData.Armor += value;

            var restValue = 0f;
            if (enemy.EnemyData.Armor < 0)
            {
                restValue = enemy.EnemyData.Armor;
                enemy.EnemyData.Armor = 0;
            }
            
            ChangeArmorBar(enemy);

            SetAnimation("Idle", true, enemy);
            return restValue;
        }

        private void ChangeArmorBar(Enemy enemy)
        {
            var size = enemy.ArmorBar.size;
            size = new Vector2(enemy.EnemyData.Armor / 100, size.y);
            enemy.ArmorBar.size = size;
        }

        private void ChangeHealth(Enemy enemy, float value)
        {
            enemy.EnemyData.Health += value;
            ChangeHealthBar(enemy);
            
            if (enemy.EnemyData.Health <= 0)
            {
                _enemies.Remove(enemy);
                SetAnimation("Dead", false, enemy);
                Object.Destroy(enemy.gameObject);
            }

            if (_enemies.Count == 0)
            {
                OnAllEnemiesDead?.Invoke();
            }
        }

        private void ChangeHealthBar(Enemy enemy)
        {
            var size = enemy.HealthBar.size;
            size = new Vector2(size.x / 100 * enemy.EnemyData.Health, size.y);
            enemy.HealthBar.size = size;
        }

        public void Damage(float value, Character character = null)
        {
            var enemy = character as Enemy;
            SetAnimation("Hit", false, enemy);
            if (enemy.EnemyData.Armor > 0)
            {
                value = ChangeArmor(enemy, value);
            }
            
            ChangeHealth(enemy, value);
            SetAnimation("Idle", true, enemy);
        }

        public async UniTask<Enemy> GetTarget()
        {
            await UniTask.WaitUntil(() => _isTargetDefine);
            _isTargetDefine = false;
            return _targetEnemy;
        }

        public void SetAnimation(string name, bool loop, Character enemy)
        {
            enemy.SkeletonAnimation.AnimationState.SetAnimation(0, name, loop);
        }
    }

    public struct EnemyAction
    {
        public readonly Enemy Enemy;
        public readonly float Value;
        public readonly ActionType ActionType;

        public EnemyAction(Enemy enemy, ActionType actionType, float value)
        {
            Enemy = enemy;
            Value = value;
            ActionType = actionType;
        }
    }
}