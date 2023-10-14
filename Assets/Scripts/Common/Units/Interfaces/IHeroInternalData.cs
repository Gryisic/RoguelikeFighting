using Common.Models.Actions;
using Common.Units.Heroes;
using Infrastructure.Utils;

namespace Common.Units.Interfaces
{
    public interface IHeroInternalData : IUnitInternalData
    {
        Enums.InputDirection InputDirection { get; }
        Enums.ActionExecutionPlacement Placement { get; }
        Enums.HeroActionType LastActionType { get; }
        HeroAnimationData AnimationData { get; }
        HeroActionsContainer ActionsContainer { get; }
        
        float DashDistance { get; }
        float DashForce { get; }
        int MaxDashesCount { get; }
        int RemainingDashes { get; }
        bool CanDash { get; }
        
        public int MaxJumps { get; }
        public int RemainingJumps { get; }
        public bool JumpRequested { get; }

        void SetDashData(float distance, float force, int maxDashesCount);
        void IncreaseRemainingDashes();
        void DecreaseRemainingDashes();
        
        public void SetJumpData(int maxJumpsCount);
        public void RequestJump();
        public void DecreaseRemainingJumps();
        public void ResetJumps();

        void SetAction(Enums.HeroActionType actionType);
        void ResetAction();
    }
}