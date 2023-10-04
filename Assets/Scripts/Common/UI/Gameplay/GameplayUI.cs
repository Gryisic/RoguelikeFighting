using UnityEngine;
using UnityEngine.Serialization;

namespace Common.UI.Gameplay
{
    public class GameplayUI : UIElement
    {
        [SerializeField] private HeroView _heroView;
        [SerializeField] private ModifierCardsHandler _cardsHandler;
        
        public HeroView HeroView => _heroView;
        public ModifierCardsHandler CardsHandler => _cardsHandler;
    }
}