using System;
using System.Linq;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Gameplay.Hero
{
    [Serializable]
    public class SkillsViewHandler
    {
        [SerializeField] private SkillView[] _views;

        public void UpdateIcon(Enums.HeroActionType skillType, Sprite icon)
        {
            SkillView view = _views.First(v => v.SkillType == skillType);
            
            view.UpdateIcon(icon);
        }
    }
}