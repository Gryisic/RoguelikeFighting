using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    [CreateAssetMenu(menuName = "Configs/Templates/Rooms/Battle", fileName = "Template")]
    public class BattleRoomTemplate : ScriptableObject
    {
        [SerializeField] private Enums.NextWaveRequirement _requirement;

        [Space, Header("Timer Data")]
        [SerializeField] private int _timer;

        public Enums.NextWaveRequirement Requirement => _requirement;

        public int Timer => _timer;
    }
}