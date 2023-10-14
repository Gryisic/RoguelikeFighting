using System;
using Common.Gameplay.Rooms;
using UnityEngine;

namespace Common.Models.Animators
{
    [Serializable]
    public class RoomAnimator
    {
        [SerializeField] private AnimatedRoomBackground[] _backgroundsQueue;

        private int _index;

        public void Activate()
        {
            if (_index >= _backgroundsQueue.Length)
                return;
            
            _backgroundsQueue[_index]?.Activate();
        }

        public void Deactivate()
        {
            if (_index >= _backgroundsQueue.Length)
                return;
            
            _backgroundsQueue[_index]?.Deactivate();
        }

        public void PlayNext()
        {
            if (_index + 1 >= _backgroundsQueue.Length)
                return;

            _backgroundsQueue[_index].Deactivate();
            _backgroundsQueue[_index + 1].Activate();
            
            _index++;
        }
    }
}