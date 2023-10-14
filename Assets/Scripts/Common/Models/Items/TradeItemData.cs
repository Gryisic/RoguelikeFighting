using UnityEngine;

namespace Common.Models.Items
{
    [CreateAssetMenu(menuName = "Configs/Items/TradeItem")]
    public class TradeItemData : Item
    {
        [SerializeField] private int _cost;
        
        public int Cost => _cost;
    }
}