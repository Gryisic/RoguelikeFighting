using System;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Models.Items;
using Common.Scene.Cameras.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public abstract class DataModifierRoom : Room
    {
        [Space, Header("Additional Data")]
        [SerializeField] protected Transform cameraFocusPoint;

        protected override ICameraService CameraService { get; set; }

        protected IRunData runData;

        public override void Initialize(IStageData stageData, IRunData runData, ICameraService cameraService)
        {
            this.runData = runData;
            CameraService = cameraService;
        }

        public override void Enter()
        {
            runData.IncreaseVisitedRoomsAmount();
            
            base.Enter();
        }

        protected void ModifyData(Enums.RunDataType type, Item itemData)
        {
            if (itemData is TradeItemData tradeItemData)
                runData.Get<GaldData>(Enums.RunDataType.Gald).Decrease(tradeItemData.Cost);
            
            switch (type)
            {
                case Enums.RunDataType.Heal:
                    HealData healData = runData.Get<HealData>(Enums.RunDataType.Heal);
                    
                    if (itemData.Change == Enums.RunDataChange.Increase) 
                        healData.RestoreCharge();
                    else 
                        healData.UseCharge();
                    
                    break;
                
                case Enums.RunDataType.Experience:
                    ExperienceData experienceData = runData.Get<ExperienceData>(Enums.RunDataType.Experience);
                    
                    if (itemData.Change == Enums.RunDataChange.Increase) 
                        experienceData.Add(itemData.Amount);
                    else 
                        experienceData.Remove(itemData.Amount);
                    
                    break;
                
                case Enums.RunDataType.Gald:
                    GaldData galdData = runData.Get<GaldData>(Enums.RunDataType.Gald);
                    
                    if (itemData.Change == Enums.RunDataChange.Increase) 
                        galdData.Increase(itemData.Amount);
                    else 
                        galdData.Decrease(itemData.Amount);
                    
                    break;
                
                case Enums.RunDataType.None:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}