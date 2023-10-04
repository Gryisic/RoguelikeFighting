﻿using System;
using System.Collections.Generic;
using Common.Gameplay.Interfaces;
using Common.Units.Interfaces;
using Common.Units.Stats;
using UnityEngine;
using Zenject;

namespace Common.Units.Enemies
{
    public class DebugEnemy : Enemy
    {
        [SerializeField] private EnemyTemplate _template;
        [SerializeField] private bool _isKicking;
        
        private IRunData _runData;
        
        [Inject]
        private void DebugConstruct(IRunData runData)
        {
            _runData = runData;
        }
        
        private void Start()
        {
            if (_isKicking)
            {
                Initialize(_template);
                
                Invoke(nameof(Activate), 0.5f);
                Invoke(nameof(DebugInfo), 0.5f);
            }
        }

        private void OnDestroy()
        {
            if (_isKicking)
            {
                Dispose();
            }
        }

        private void DebugInfo()
        {
            var enemyData = internalData as EnemyInternalData;
            
            enemyData.SetHeroData(_runData.SharedHeroData);
            
            //Debug.Log(_runData.SharedHeroData.Transform.position);
        }
    }
}