using UnityEngine;

namespace Common.Models.Items
{
    [CreateAssetMenu(menuName = "Configs/Items/StorageItem")]
    public class StorageItemData : Item
    {
        [SerializeField] private int _dropChance;

        public int DropChance => _dropChance;
    }
}