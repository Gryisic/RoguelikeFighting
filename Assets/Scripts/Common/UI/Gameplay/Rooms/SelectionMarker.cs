using System;
using Common.Gameplay.Rooms;
using Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Gameplay.Rooms
{
    public class SelectionMarker : UIElement
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _type;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private FoldedSelectionMarker _foldedMarker;
        [SerializeField] private ExpandedSelectionMarker _expandedMarker;

        public override void Activate()
        {
            _foldedMarker.Activate();
            
            base.Activate();
        }

        public void Expand()
        {
            _foldedMarker.Deactivate();
            _expandedMarker.Activate();
            
            _description.gameObject.SetActive(true);
        }
        
        public void Fold()
        {
            _description.gameObject.SetActive(false);
            
            _expandedMarker.Deactivate();
            _foldedMarker.Activate();
        }

        public void SetData(RoomTemplate data)
        {
            _icon.sprite = data.Icon;
            _type.text = Enum.GetName(typeof(Enums.RoomType), data.Type);
            _description.text = data.Description;
        }
    }
}