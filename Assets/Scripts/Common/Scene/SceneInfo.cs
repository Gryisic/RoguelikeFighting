using System;
using System.Collections.Generic;
using Common.Units;
using UnityEngine;

namespace Common.Scene
{
    [Serializable]
    public class SceneInfo
    {
        [SerializeField] private SpawnInfo _spawnInfo;
        
        public SpawnInfo SpawnInfo => _spawnInfo;
    }
}