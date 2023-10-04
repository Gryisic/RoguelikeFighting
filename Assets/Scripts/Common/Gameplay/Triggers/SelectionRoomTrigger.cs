using Infrastructure.Utils;

namespace Common.Gameplay.Triggers
{
    public class SelectionRoomTrigger : RoomChangeTrigger
    {
        private Enums.RoomType _roomType;

        protected override Enums.RoomType RoomType => _roomType;

        public void UpdateType(Enums.RoomType type) => _roomType = type;
    }
}