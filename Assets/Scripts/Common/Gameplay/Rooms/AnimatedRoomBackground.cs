using UnityEngine;

namespace Common.Gameplay.Rooms
{
    public class AnimatedRoomBackground : MonoBehaviour
    {
        public void Activate() => gameObject.SetActive(true);

        public void Deactivate() => gameObject.SetActive(false);
    }
}