using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    [CreateAssetMenu(menuName = "Configs/Templates/Rooms/General", fileName = "Template")]
    public class RoomTemplate : ScriptableObject
    {
        [SerializeField] private Enums.RoomType _type;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _description;

        public Enums.RoomType Type => _type;
        public Sprite Icon => _icon;
        public string Description => _description;
    }
}