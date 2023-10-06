using Infrastructure.Utils;
using UnityEngine;

namespace Common.UI.Gameplay.Hero
{
    public class HeroView : UIElement
    {
        [SerializeField] private CanvasHealthBar _canvasHealthBar;
        [SerializeField] private SkillsViewHandler _skillsViewHandler;
        [SerializeField] private HealChargesView _healChargesView;
        
        public override void Activate()
        {
            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        public void UpdateHealth(int currentValue, int maxValue) => _canvasHealthBar.UpdateValue(currentValue, maxValue);

        public void UpdateSkillIcon(Enums.HeroActionType actionType, Sprite icon) => _skillsViewHandler.UpdateIcon(actionType, icon);

        public void UpdateHealCharges(int amount) => _healChargesView.UpdateCharges(amount);
    }
}