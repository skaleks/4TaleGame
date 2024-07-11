using System;
using UnityEngine;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.Enums;
using UnityProject.Scripts.Gameplay.Model;
using UnityProject.Scripts.Gameplay.View;
using UnityProject.Scripts.UI;
using VContainer;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class PlayerController : IDisposable
    {
        [Inject] private DefaultProfile _defaultProfile;
        [Inject] private CharacterSpawner _characterSpawner;
        [Inject] private GameplayUIPresenter _uiPresenter;
        [Inject] private RoomController _roomController;

        private Character _playerView;
        private PlayerData _playerData;
        
        public void Initialize()
        {
            _playerView = _characterSpawner.InstantiatePlayer(_roomController.Room.PlayerSpawnPoint.position);
            _playerData = _defaultProfile.PlayerData;
            
            _uiPresenter.SetActionPoints(_playerData.Energy);
            _uiPresenter.SetDeckCount(_playerData.DeckCapacity);
            _uiPresenter.SetDiscardCount(0);
            _uiPresenter.OnCardActivate += OnCardActivate;
        }

        public void Dispose()
        {
            _uiPresenter.OnCardActivate -= OnCardActivate;
        }
        
        private void OnCardActivate(Card card)
        {
            var data = card.CardData;
            switch (data.CardType)
            {
                case CardType.Attack:
                    break;
                case CardType.Defense:
                    ChangeArmor(data.Value);
                    break;
                case CardType.Healing:
                    ChangeHealth(data.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void ChangeHealth(int value)
        {
            _playerData.Health += value;
            
            var size = _playerView.HealthBar.size;
            size = new Vector2(size.x / 100 * _playerData.Health, size.y);
            _playerView.HealthBar.size = size;
        }
        
        public void ChangeEnergy(int value)
        {
            _playerData.Energy += value;
            _uiPresenter.SetActionPoints(_playerData.Energy);
        }
        
        public void ChangeArmor(int value)
        {
            _playerData.Armor += value;
            
            var size = _playerView.ArmorBar.size;
            size = new Vector2(size.x / 100 * _playerData.Armor, size.y);
            _playerView.ArmorBar.size = size;
        }

        public int GetHealth() => _playerData.Health;
        public int GetEnergy() => _playerData.Energy;
        public int GetDeckCapacity() => _playerData.DeckCapacity;


    }
}