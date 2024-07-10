using UnityEngine;
using UnityProject.Scripts.Gameplay.Model;

namespace UnityProject.Scripts.Data
{
    [CreateAssetMenu(fileName = "DefaultProfile", menuName = "Data/DefaultProfile", order = 2)]
    public class DefaultProfile : ScriptableObject
    {
        [SerializeField] private PlayerData _playerData;

        public PlayerData PlayerData => _playerData;
    }
}