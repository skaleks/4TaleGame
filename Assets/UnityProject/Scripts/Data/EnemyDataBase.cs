using System.Collections.Generic;
using UnityEngine;
using UnityProject.Scripts.Gameplay.Model;

namespace UnityProject.Scripts.Data
{
    [CreateAssetMenu(fileName = "EnemyDataBase", menuName = "Data/EnemyDataBase", order = 4)]
    public class EnemyDataBase : ScriptableObject
    {
        [SerializeField] private List<EnemyData> _enemyData;

        public List<EnemyData> EnemyDataList => _enemyData;
    }
}