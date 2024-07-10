﻿using System.Collections.Generic;
using UnityEngine;
using UnityProject.Scripts.Gameplay.View;
using UnityProject.Scripts.UI;

namespace UnityProject.Scripts.Data
{
    [CreateAssetMenu(fileName = "PrefabDataBase", menuName = "Data/PrefabDataBase", order = 0)]
    public class PrefabDataBase : ScriptableObject
    {
        [SerializeField] private Character _player;
        [SerializeField] private Character _enemy;
        [SerializeField] private List<Room> _rooms;
        [SerializeField] private Card _card;

        public Character Player => _player;
        public Character Enemy => _enemy;
        public List<Room> Rooms => _rooms;
        public Card Card => _card;
    }
}