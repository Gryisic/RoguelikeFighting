using UnityEngine;

namespace Common.Models.Actions.Templates
{
    [CreateAssetMenu(menuName = "Configs/Actions/TeleportationAction")]
    public class TeleportationActionTemplate : ActionTemplate
    {
        [SerializeField] private float _chargingTime;

        public float ChargingTime => _chargingTime;
    }
}