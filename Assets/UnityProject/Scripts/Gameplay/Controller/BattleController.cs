using UnityProject.Scripts.Enums;
using UnityProject.Scripts.UI;
using VContainer;

namespace UnityProject.Scripts.Gameplay.Controller
{
    public sealed class BattleController
    {
        [Inject] private RoomController _roomController;
        [Inject] private GameplayUIPresenter _uiPresenter;

        public Turn TurnOrder { get; private set; } = Turn.Player;

        public void Finish()
        {
            _roomController.InstantiateRoom();
        }

        public void SwitchTurn()
        {
            TurnOrder = TurnOrder == Turn.Player ? Turn.Enemy : Turn.Player;
        }
    }
}