using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Triggers
{
    public class ConcreteRoomTrigger : RoomChangeTrigger
    {
        [SerializeField] private Enums.RoomType _roomType;

        protected override Enums.RoomType RoomType => _roomType;
    }
}