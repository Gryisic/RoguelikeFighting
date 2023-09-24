using Common.Units;
using UnityEngine;

namespace Common.Scene
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private UnitTemplate _template;

        public UnitTemplate Template => _template;
        public Vector3 Position => transform.position;
    }
}