using UnityEngine;
using UnityProject.Scripts.Gameplay.View;

namespace UnityProject.Scripts.Data
{
    [CreateAssetMenu(fileName = "PrefabDataBase", menuName = "Data/PrefabDataBase", order = 0)]
    public class PrefabDataBase : ScriptableObject
    {
        [SerializeField] private Character _player;
        [SerializeField] private Character _enemy;
        [SerializeField] private Room _room1;
        [SerializeField] private Room _room2;

        public Character Player => _player;
        public Character Enemy => _enemy;
        public Room Room1 => _room1;
        public Room Room2 => _room2;
    }
}