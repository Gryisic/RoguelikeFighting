using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    [CreateAssetMenu(menuName = "Configs/Templates/Waves/Wave", fileName = "Template")]
    public class WaveTemplate : ScriptableObject
    {
        [SerializeField] private Enums.WaveRequirement _requirement;

        [Space, Header("Timer Data")]
        [SerializeField] private float _timer;

        public Enums.WaveRequirement Requirement => _requirement;

        public float Timer => _timer;
    }
}