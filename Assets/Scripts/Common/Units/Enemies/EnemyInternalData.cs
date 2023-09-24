using System;
using Common.Models.Actions;
using Common.Units.Interfaces;
using UnityEngine;

namespace Common.Units.Enemies
{
    public class EnemyInternalData : UnitInternalData, IEnemyInternalData
    {
        public ISharedUnitData HeroData { get; private set; }
        public EnemyAction Action { get; private set; }

        public EnemyInternalData(UnitPhysics physics, Transform transform, UnitAnimator animator, IUnitStatsData statsData, IActionsData actionsData, IAnimationEventsReceiver animationEventsReceiver, Type type) : base(transform, physics, animator, statsData, actionsData, animationEventsReceiver, type) { }

        public override void Dispose()
        {
            ActionsData.Dispose();
            Action.Dispose();
        }

        public void SetHeroData(ISharedUnitData heroData) => HeroData = heroData;
        
        public void SetAction(EnemyAction action) => Action = action;
    }
}