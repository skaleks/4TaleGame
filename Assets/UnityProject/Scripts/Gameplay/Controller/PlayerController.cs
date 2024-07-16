using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.Gameplay.Model;
using UnityProject.Scripts.Gameplay.View;
using VContainer;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class PlayerController : IDamageHandler, IAnimationHandler
    {
        [Inject] private DefaultProfile _defaultProfile;
        [Inject] private CharacterSpawner _characterSpawner;
        [Inject] private RoomController _roomController;

        private Character _playerView;
        private PlayerData _playerData;

        public event Action OnPlayerDead;
        
        public void Initialize()
        {
            _playerView = _characterSpawner.InstantiatePlayer(_roomController.CurrentRoom.PlayerSpawnPoint.position);
            _playerData = new PlayerData
            {
                Armor = _defaultProfile.PlayerData.Armor,
                DeckCapacity = _defaultProfile.PlayerData.DeckCapacity,
                Energy = _defaultProfile.PlayerData.Energy,
                Health = _defaultProfile.PlayerData.Health,
                MaxHealth = _defaultProfile.PlayerData.MaxHealth
            };

            PlayAnimation("Idle", false, true);
            ChangeHealthView();
            ChangeArmorView();
        }

        public void ChangeHealth(float value)
        {
            _playerData.Health = Mathf.Clamp(_playerData.Health + value, 0, _playerData.MaxHealth);
            ChangeHealthView();

            if (_playerData.Health <= 0)
            {
                PlayAnimation("Dead", true, false);
                OnPlayerDead?.Invoke();
            }
        }

        private void ChangeHealthView()
        {
            var size = _playerView.HealthBar.size;
            size = new Vector2(_playerData.Health / 100, size.y);
            _playerView.HealthBar.size = size;
        }
        
        private void ChangeArmorView()
        {
            var size = _playerView.ArmorBar.size;
            size = new Vector2(_playerData.Armor / 100, size.y);
            _playerView.ArmorBar.size = size;
        }

        public void ChangeEnergy(float value)
        {
            _playerData.Energy += value;
        }
        
        public float ChangeArmor(float value)
        {
            PlayAnimation("Skill", true, false);
            _playerData.Armor += value;

            var restValue = 0f;
            if (_playerData.Armor < 0)
            {
                restValue = _playerData.Armor;
                _playerData.Armor = 0;
            }
            
            ChangeArmorView();
            PlayAnimation("Idle", false, true);
            
            return restValue;
        }

        public float GetHealth() => _playerData.Health;
        public float GetEnergy() => _playerData.Energy;
        public int GetDeckCapacity() => _playerData.DeckCapacity;

        public void ResetEnergy()
        {
            _playerData.Energy = _defaultProfile.PlayerData.Energy;
        }

        public void Damage(float value, Character character = null)
        {
            if (_playerData.Armor > 0)
            {
                value = ChangeArmor(value);
            }

            PlayAnimation("Hit", true, false);
            PlayAnimation("Idle", false, true);
            ChangeHealth(value);
        }

        public async UniTask FinishBattleMove()
        { 
            PlayAnimation("Move", false, true);

            var position = _playerView.transform.position;
            await _playerView.transform.DOMove(position + new Vector3(-position.x, 0, 0), 2f)
                .AsyncWaitForCompletion()
                .AsUniTask();
            
            PlayAnimation("Idle", false, true);
        }
        
        public async UniTask StartBattleMove()
        {
            PlayAnimation("Move", false, true);
            
            await _playerView.transform.DOMove(_roomController.CurrentRoom.PlayerSpawnPoint.position, 2f)
                .AsyncWaitForCompletion()
                .AsUniTask();
            
            PlayAnimation("Idle", false, true);
        }

        public void PlayAnimation(string name, bool replace, bool loop, Character character = null)
        {
            if (replace)
            {
                _playerView.SkeletonAnimation.AnimationState.SetAnimation(0, name, loop);
            }
            else
            {
                _playerView.SkeletonAnimation.AnimationState.AddAnimation(0, name, loop, 0f);
            }
        }
    }
}