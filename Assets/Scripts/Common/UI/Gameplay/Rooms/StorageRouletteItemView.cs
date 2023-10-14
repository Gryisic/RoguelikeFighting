using Common.Models.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    public class StorageRouletteItemView : UIElement
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _description;

        public void UpdateData(StorageItemData data)
        {
            _icon.sprite = data.Icon;
            _description.text = data.Description;
        }
    }
}