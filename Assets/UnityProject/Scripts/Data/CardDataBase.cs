using System.Collections.Generic;
using UnityEngine;
using UnityProject.Scripts.Gameplay.Model;

namespace UnityProject.Scripts.Data
{
    [CreateAssetMenu(fileName = "CardDataBase", menuName = "Data/CardDataBase", order = 1)]
    public class CardDataBase : ScriptableObject
    {
        [SerializeField] private List<CardData> _cards;

        public List<CardData> Cards => _cards;
    }
}