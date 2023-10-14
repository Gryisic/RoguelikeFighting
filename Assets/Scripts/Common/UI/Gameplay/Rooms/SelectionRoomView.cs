using System;
using UnityEngine;

namespace Common.UI.Gameplay.Rooms
{
    [RequireComponent(typeof(Canvas))]
    public class SelectionRoomView : UIElement
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private SelectionMarkersHandler _selectionMarkersHandler;

        public SelectionMarkersHandler SelectionMarkersHandler => _selectionMarkersHandler;

        private void Awake()
        {
            if (_canvas == null)
            {
                Debug.LogError("Canvas of SelectionRoomView is not assigned");

                _canvas = GetComponent<Canvas>();
            }
        }

        public override void Activate()
        {
            _selectionMarkersHandler.Activate();
            
            _canvas.enabled = true;
        }

        public override void Deactivate()
        {
            _canvas.enabled = false;
            
            _selectionMarkersHandler.Deactivate();
        }
    }
}