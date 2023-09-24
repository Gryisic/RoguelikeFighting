using System;
using Common.Gameplay.Interfaces;
using Common.Units.Interfaces;
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
        
        private void Awake()
        {
            Initialize(_template);
            
            if (_isKicking)
            {

                Invoke(nameof(Activate), 0.5f);
                Invoke(nameof(DebugInfo), 0.5f);
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void DebugInfo()
        {
            var enemyData = internalData as EnemyInternalData;
            
            enemyData.SetHeroData(_runData.SharedHeroData);
            
            //Debug.Log(_runData.SharedHeroData.Transform.position);
        }
    }
}