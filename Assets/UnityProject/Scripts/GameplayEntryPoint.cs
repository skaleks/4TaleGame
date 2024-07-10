using UnityProject.Scripts.Data;
using UnityProject.Scripts.Gameplay;
using UnityProject.Scripts.Gameplay.Controller;
using UnityProject.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace UnityProject.Scripts
{
    public class GameplayEntryPoint : IStartable
    {
        [Inject] private readonly GameplayUIPresenter _gameplayUIPresenter;
        [Inject] private readonly PrefabDataBase _prefabDataBase;
        [Inject] private readonly CharacterSpawner _characterSpawner;
        [Inject] private readonly RoomController _roomController;
        [Inject] private readonly PlayerController _playerController;
        [Inject] private readonly EnemyController _enemyController;
        [Inject] private readonly DeckController _deckController;

        public void Start()
        {
            _gameplayUIPresenter.Initialize();
            _roomController.InstantiateRoom();
            _playerController.Initialize();
            _enemyController.Initialize();
            _deckController.InstantiateDeck();
        }
    }
}