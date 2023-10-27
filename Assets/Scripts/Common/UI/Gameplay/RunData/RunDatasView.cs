using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Gameplay.RunData
{
    public class RunDatasView : UIElement
    {
        [SerializeField] private List<RunDataView> _dataViews;

        public override void Activate()
        {
            foreach (var view in _dataViews) 
                view.Activate();

            base.Activate();
        }

        public override void Deactivate()
        {
            foreach (var view in _dataViews) 
                view.Deactivate();

            base.Deactivate();
        }

        public void SetAmount(Enums.RunDataType data, int amount)
        {
            RunDataView view = DefineView(data);
            
            view.SetAmount(amount);
        }

        private RunDataView DefineView(Enums.RunDataType data)
        {
            switch (data)
            {
                case Enums.RunDataType.Gald:
                    return _dataViews.First(v => v is GaldView);
                
                case Enums.RunDataType.Heal:
                    return _dataViews.First(v => v is HealChargesView);
                
                case Enums.RunDataType.Experience:
                    return _dataViews.First(v => v is XPBarView);
                
                case Enums.RunDataType.Modifiers:
                    return null;

                default:
                    throw new ArgumentOutOfRangeException(nameof(data), data, null);
            }
        }
    }
}