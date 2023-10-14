using Common.Gameplay.Rooms;
using UnityEngine;

namespace Common.UI.Gameplay.Rooms
{
    public class SelectionMarkersHandler : UIElement
    {
        [SerializeField] private SelectionMarker[] _selectionMarkers;

        public override void Deactivate()
        {
            foreach (var marker in _selectionMarkers) 
                marker.Deactivate();

            base.Deactivate();
        }

        public void SetDataAndActivate(int index, RoomTemplate data)
        {
            _selectionMarkers[index].SetData(data);
            _selectionMarkers[index].Activate();
        }

        public void Fold(int index) => _selectionMarkers[index].Fold();
        
        public void Expand(int index) => _selectionMarkers[index].Expand();
    }
}