using System;
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
            _playerView = _characterSpawner.InstantiatePlayer(_roomController.Room.PlayerSpawnPoint.position);
            _playerData = new PlayerData
            {
                Armor = _defaultProfile.PlayerData.Armor,
                DeckCapacity = _defaultProfile.PlayerData.DeckCapacity,
                Energy = _defaultProfile.PlayerData.Energy,
                Health = _defaultProfile.PlayerData.Health,
                MaxHealth = _defaultProfile.PlayerData.MaxHealth
            };

            SetAnimation("Idle", true);
            ChangeHealthView();
            ChangeArmorView();
        }

        public void ChangeHealth(float value)
        {
            _playerData.Health = Mathf.Clamp(_playerData.Health + value, 0, _playerData.MaxHealth);
            ChangeHealthView();

            if (_playerData.Health <= 0)
            {
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
            SetAnimation("Skill", false);
            _playerData.Armor += value;

            var restValue = 0f;
            if (_playerData.Armor < 0)
            {
                restValue = _playerData.Armor;
                _playerData.Armor = 0;
            }
            
            ChangeArmorView();
            SetAnimation("Idle", true);
            
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

            SetAnimation("Hit", false);
            ChangeHealth(value);
            SetAnimation("Idle", true);
        }

        public void SetAnimation(string name, bool loop, Character character = null)
        {
            _playerView.SkeletonAnimation.AnimationState.AddAnimation(0, name, loop, 0f);
        }
    }
}