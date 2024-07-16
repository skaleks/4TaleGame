using System.Collections.Generic;
using UnityEngine;

namespace UnityProject.Scripts.Gameplay.View
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private List<Transform> _enemySpawnPoints;
        [SerializeField] private SpriteRenderer _back;
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public List<Transform> EnemySpawnPoints => _enemySpawnPoints;
        public SpriteRenderer BackGround => _back;
    }
}