using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Models.Actions;
using Common.Models.Actions.Templates;
using Common.Scene;
using Common.Scene.Cameras.Interfaces;
using Common.UI.Gameplay;
using Common.UI.Gameplay.Hero;
using Common.UI.Gameplay.RunData;
using Common.Units;
using Common.Units.Legacy;
using Common.Utils.Interfaces;
using Core.Interfaces;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.States
{
    public class GameplayInitializeState : IGameplayState
    {
        private readonly IStateChanger<IGameplayState> _stateChanger;
        private readonly IUnitFactory _unitFactory;
        private readonly IServicesHandler _servicesHandler;
        
        private readonly Player _player;
        private readonly SpawnInfo _spawnInfo;
        private readonly UnitsHandler _unitsHandler;
        private readonly RunData _runData;
        private readonly Stage _stage;
        private readonly UI.UI _ui;
        private readonly RunDatasView _runDataView;
        private readonly HeroView _heroView;

        public GameplayInitializeState(IStateChanger<IGameplayState> stateChanger, IServicesHandler servicesHandler, Player player, SceneInfo sceneInfo, IUnitFactory unitFactory, UnitsHandler unitsHandler, IStageData stageData, IRunData runData, UI.UI ui)
        {
            _stateChanger = stateChanger;
            _unitFactory = unitFactory;
            _servicesHandler = servicesHandler;
            
            _player = player;
            _unitsHandler = unitsHandler;

            _ui = ui;
            _runDataView = ui.Get<RunDatasView>();
            _heroView = ui.Get<HeroView>();
            
            _spawnInfo = sceneInfo.SpawnInfo;

            _stage = stageData as Stage;
            _runData = runData as RunData;
        }

        public void Activate()
        {
            CreatePlayer();
            CreateEnemies();
            SetUI();
            
            _stage.Initialize();
            
            _stateChanger.ChangeState<GameplayActiveState>();
        }

        private void CreatePlayer()
        {
            _unitFactory.Load(_runData.HeroTemplate.ID);

            Vector3 playerSpawnPointPosition = _stage.Rooms[0].HeroInitialPosition.position;
            
            Hero hero = _unitFactory.Create(_runData.HeroTemplate, playerSpawnPointPosition) as Hero;
            hero.Initialize(_runData.HeroTemplate);
            
            if (TryCreateLegacyUnit(_runData.InitialLegacyUnitData.FirstLegacyUnitTemplate, playerSpawnPointPosition, out LegacyUnit firstLegacyUnit))
                hero.AddLegacyUnit(firstLegacyUnit, Enums.HeroActionType.FirstLegacySkill);
            if (TryCreateLegacyUnit(_runData.InitialLegacyUnitData.SecondLegacyUnitTemplate, playerSpawnPointPosition, out LegacyUnit secondLegacyUnit))
                hero.AddLegacyUnit(secondLegacyUnit, Enums.HeroActionType.SecondLegacySkill);
            
            _runData.SetHeroData(hero);
            _unitsHandler.Add(hero);
            _player.UpdateHero(hero);
        }

        private bool TryCreateLegacyUnit(LegacyUnitTemplate template, Vector2 position, out LegacyUnit unit)
        {
            unit = null;
            
            if (template == null)
                return false;
            
            _unitFactory.Load(template.ID);

            unit = _unitFactory.Create(_runData.InitialLegacyUnitData.FirstLegacyUnitTemplate, position) as LegacyUnit;
            unit.Initialize(template);
            unit.Deactivate();

            return true;
        }

        private void CreateEnemies()
        {
            IReadOnlyList<int> idList = _spawnInfo.GetIDList;
            _unitFactory.Load(idList);

            for (var i = 0; i < _spawnInfo.TemplatesToSpawn.Count; i++)
            {
                UnitTemplate template = _spawnInfo.TemplatesToSpawn[i];

                for (int j = 0; j < Constants.InitialCopiesOfUnit; j++)
                {
                    Enemy enemy = _unitFactory.Create(template, _spawnInfo.UnitsRoot.position) as Enemy;
                    
                    enemy.SetHeroData(_unitsHandler.Hero);
                    enemy.Initialize(template);
                    _unitsHandler.Add(enemy);  
                    
                    enemy.gameObject.SetActive(false);
                }
            }
        }
        
        private void SetUI()
        {
            int currentHealth = _unitsHandler.Hero.StatsData.GetStatValueAsInt(Enums.Stat.Health);
            int maxHealth = _unitsHandler.Hero.StatsData.GetStatValueAsInt(Enums.Stat.MaxHealth);
            _heroView.UpdateHealth(currentHealth, maxHealth);

            List<HeroActionTemplate> baseSkills = _runData.HeroTemplate.Actions.Where(a =>
                a.ExtendsFrom == Enums.HeroActionType.Skill || a.ExtendsFrom == Enums.HeroActionType.FirstLegacySkill ||
                a.ExtendsFrom == Enums.HeroActionType.SecondLegacySkill).ToList();

            foreach (var template in baseSkills)
            {
                if (template.Direction == Enums.InputDirection.Horizontal)
                    _heroView.UpdateSkillIcon(template.ExtendsFrom, template.Icon);
            }

            int healCharges = _runData.Get<HealData>(Enums.RunDataType.Heal).HealCharges;
            int xpAmount = _runData.Get<ExperienceData>(Enums.RunDataType.Experience).Amount;
            int galdAmount = _runData.Get<GaldData>(Enums.RunDataType.Gald).Amount;
            
            _runDataView.SetAmount(Enums.RunDataType.Heal, healCharges);
            _runDataView.SetAmount(Enums.RunDataType.Experience, xpAmount);
            _runDataView.SetAmount(Enums.RunDataType.Gald, galdAmount);

            Camera camera = _servicesHandler.GetSubService<ICameraService>().SceneCamera;
            _ui.SetCameraToLayer(camera, Enums.UILayer.Camera);
        }
    }
}