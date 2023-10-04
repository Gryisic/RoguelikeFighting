using System;
using System.Collections.Generic;
using Common.Models.Actions;
using Common.Units.Interfaces;
using UnityEngine;

namespace Common.Units.Enemies
{
    public class EnemyInternalData : UnitInternalData, IEnemyInternalData
    {
        public ISharedUnitData HeroData { get; private set; }
        public EnemyTemplate Data { get; }
        public IReadOnlyList<EnemyAction> Actions { get; private set; }

        public EnemyInternalData(EnemyTemplate data, UnitPhysics physics, Transform transform, UnitAnimator animator, IUnitStatsData statsData, IActionsData actionsData, IAnimationEventsReceiver animationEventsReceiver, Type type) : base(transform, physics, animator, statsData, actionsData, animationEventsReceiver, type)
        {
            Data = data;
        }

        public override void Dispose()
        {
            ActionsData.Dispose();

            foreach (var action in Actions) 
                action.Dispose();
        }

        public void SetHeroData(ISharedUnitData heroData) => HeroData = heroData;
        
        public void SetActions(IReadOnlyList<EnemyAction> actions) => Actions = actions;
    }
}