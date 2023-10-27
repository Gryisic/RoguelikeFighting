using Infrastructure.Utils;
using UnityEngine;

namespace Common.Models.Actions.Templates
{
    [CreateAssetMenu(menuName = "Configs/Actions/LegacyAction")]
    public class LegacyActionTemplate : ActionTemplate
    {
        [Space, Header("Legacy Data")]
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _movementsSpeed;
        [SerializeField] private AnimationClip _moveClip;
        [SerializeField] private Enums.LegacyUnitNavigationStrategy _navigationStrategy;
        [SerializeField] private LayerMask _floorMask;

        public Sprite Icon => _icon;
        public float MovementsSpeed => _movementsSpeed;
        public AnimationClip MoveClip => _moveClip;
        public LayerMask FloorMask => _floorMask;
        public Enums.LegacyUnitNavigationStrategy NavigationStrategy => _navigationStrategy;
    }
}