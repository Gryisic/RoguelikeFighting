using System.Collections.Generic;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Scene;
using Common.Units;
using Common.Utils.Interfaces;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using Infrastructure.Utils;

namespace Common.Gameplay.States
{
    public class GameplayInitializeState : IGameplayState
    {
        private readonly IStateChanger<IGameplayState> _stateChanger;
        private readonly IUnitFactory _unitFactory;
        
        private readonly Player _player;
        private readonly SpawnInfo _spawnInfo;
        private readonly UnitsHandler _unitsHandler;
        private readonly RunData _runData;

        public GameplayInitializeState(IStateChanger<IGameplayState> stateChanger, Player player, SceneInfo sceneInfo, IUnitFactory unitFactory, UnitsHandler unitsHandler, IRunData runData)
        {
            _stateChanger = stateChanger;
            _unitFactory = unitFactory;
            
            _player = player;
            _unitsHandler = unitsHandler;
            
            _spawnInfo = sceneInfo.SpawnInfo;
            
            _runData = runData as RunData;
        }

        public void Activate()
        {
            CreatePlayer();
            CreateUnits();
            
            _stateChanger.ChangeState<GameplayActiveState>();
        }
        
        private void CreatePlayer()
        {
            IReadOnlyList<int> pID = new List<int> { _runData.InitialHeroData.HeroTemplate.ID };
            _unitFactory.Load(pID);
            
            Hero unit = _unitFactory.Create(_runData.InitialHeroData.HeroTemplate, _spawnInfo.PlayerSpawnPoint.position) as Hero;
            
            _runData.SetHeroData(unit);
            _unitsHandler.Add(unit);
            _player.UpdateHero(unit);
        }

        private void CreateUnits()
        {
            IReadOnlyList<int> idList = _spawnInfo.GetIDList;
            _unitFactory.Load(idList);

            for (var i = 0; i < _spawnInfo.TemplatesToSpawn.Count; i++)
            {
                UnitTemplate template = _spawnInfo.TemplatesToSpawn[i];

                for (int j = 0; j < Constants.InitialCopiesOfUnit; j++)
                {
                    Unit unit = _unitFactory.Create(template, _spawnInfo.UnitsRoot.position);

                    _unitsHandler.Add(unit);  
                    
                    unit.gameObject.SetActive(false);
                }
            }
        }
    }
}