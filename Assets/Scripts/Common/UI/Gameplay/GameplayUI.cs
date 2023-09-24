using UnityEngine;
using UnityEngine.Serialization;

namespace Common.UI.Gameplay
{
    public class GameplayUI : UIElement
    {
        [SerializeField] private HealthBarsView _healthBarsView;

        public HealthBarsView HealthBarsView => _healthBarsView;

        public override void Activate()
        {
            gameObject.SetActive(true);
        }

        public override void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}