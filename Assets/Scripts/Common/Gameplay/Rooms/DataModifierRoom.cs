using System;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Models.Items;
using Infrastructure.Utils;

namespace Common.Gameplay.Rooms
{
    public abstract class DataModifierRoom : Room
    {
        private IRunData _runData;

        public override void Initialize(IStageData stageData, IRunData runData)
        {
            _runData = runData;
        }

        protected void ModifyData(Enums.RunDataType type, TradeItemData itemData)
        {
            _runData.Get<GaldData>(Enums.RunDataType.Gald).Decrease(itemData.Amount);

            switch (type)
            {
                case Enums.RunDataType.Heal:
                    HealData healData = _runData.Get<HealData>(Enums.RunDataType.Heal);
                    
                    if (itemData.Change == Enums.RunDataChange.Increase) 
                        healData.RestoreCharge();
                    else 
                        healData.UseCharge();
                    
                    break;
                
                case Enums.RunDataType.Experience:
                    ExperienceData experienceData = _runData.Get<ExperienceData>(Enums.RunDataType.Experience);
                    
                    if (itemData.Change == Enums.RunDataChange.Increase) 
                        experienceData.Add(itemData.Amount);
                    else 
                        experienceData.Remove(itemData.Amount);
                    
                    break;
                
                case Enums.RunDataType.None:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}