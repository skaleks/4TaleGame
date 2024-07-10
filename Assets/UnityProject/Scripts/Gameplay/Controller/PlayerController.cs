using UnityEngine;
using UnityProject.Scripts.Data;
using UnityProject.Scripts.Gameplay.Model;
using UnityProject.Scripts.Gameplay.View;
using UnityProject.Scripts.UI;
using VContainer;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class PlayerController
    {
        [Inject] private CharacterSpawner _characterSpawner;
        [Inject] private GameplayUIPresenter _uiPresenter;
        [Inject] private DefaultProfile _defaultProfile;
        [Inject] private RoomController _roomController;
        [Inject] private DeckController _deckController;

        private Character _playerView;
        private PlayerData _playerData;
        
        public void Initialize()
        {
            _playerView = _characterSpawner.InstantiatePlayer(_roomController.Room.PlayerSpawnPoint.position);
            _playerData = _defaultProfile.PlayerData;

            _uiPresenter.ChangeActionPoints(_playerData.Energy);
            _uiPresenter.ChangeDeckCount(_playerData.DeckCapacity);
            _uiPresenter.ChangeDiscardCount(0);
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
            _uiPresenter.ChangeActionPoints(_playerData.Energy);
        }

        public int GetHealth() => _playerData.Health;
        public int GetEnergy() => _playerData.Energy;
        public int GetDeckCapacity() => _playerData.DeckCapacity;
    }
}