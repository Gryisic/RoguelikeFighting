using System.Collections.Generic;
using Common.Scene;
using Common.Units;
using Infrastructure.Factories.Extensions;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using Infrastructure.Utils;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories.UnitsFactory
{
    public class UnitFactory : IUnitFactory
    {
        private readonly DiContainer _diContainer;
        private readonly Transform _root;
        
        private Dictionary<int, Unit> _idPrefabMap;
        
        public UnitFactory(DiContainer diContainer, SceneInfo sceneInfo)
        {
            _diContainer = diContainer;
            _root = sceneInfo.SpawnInfo.UnitsRoot;
        }
        
        public void Load(IReadOnlyList<int> id)
        {
            _idPrefabMap = new Dictionary<int, Unit>();
            
            for (var index = 0; index < id.Count; index++)
            {
                int i = id[index];

                string name = i.DefineUnit();
                Unit unit = Resources.Load<Unit>($"{Constants.PathToUnitPrefabs}/{name}");

                _idPrefabMap.Add(i, unit);
            }
        }

        public Unit Create(UnitTemplate template, Vector3 at)
        {
            Unit unit = _idPrefabMap[template.ID];

            unit = _diContainer.InstantiatePrefabForComponent<Unit>(unit, at, Quaternion.identity, _root);
            unit.Initialize(template);
            
            return unit;
        }
    }
}