using System.Collections.Generic;
using System.Linq;
using Common.Gameplay.Data;
using Common.Gameplay.Interfaces;
using Common.Models.Actions.Templates;
using Common.Scene;
using Common.UI.Gameplay;
using Common.Units;
using Common.Utils.Interfaces;
using Infrastructure.Factories.UnitsFactory.Interfaces;
using Infrastructure.Utils;
using UnityEngine;

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
        private readonly GameplayUI _ui;

        public GameplayInitializeState(IStateChanger<IGameplayState> stateChanger, Player player, SceneInfo sceneInfo, IUnitFactory unitFactory, UnitsHandler unitsHandler, IRunData runData, UI.UI ui)
        {
            _stateChanger = stateChanger;
            _unitFactory = unitFactory;
            
            _player = player;
            _unitsHandler = unitsHandler;

            _ui = ui.Gameplay;
            
            _spawnInfo = sceneInfo.SpawnInfo;
            
            _runData = runData as RunData;
        }

        public void Activate()
        {
            CreatePlayer();
            CreateEnemies();
            SetUI();
            
            _stateChanger.ChangeState<GameplayActiveState>();
        }

        private void CreatePlayer()
        {
            IReadOnlyList<int> pID = new List<int> { _runData.InitialHeroData.HeroTemplate.ID };
            _unitFactory.Load(pID);
            
            Hero hero = _unitFactory.Create(_runData.InitialHeroData.HeroTemplate, _spawnInfo.PlayerSpawnPoint.position) as Hero;
            hero.Initialize(_runData.InitialHeroData.HeroTemplate);

            _runData.SetHeroData(hero);
            _unitsHandler.Add(hero);
            _player.UpdateHero(hero);
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
            int currentHealth = _unitsHandler.Hero.StatsData.GetStatValue(Enums.Stat.Health);
            int maxHealth = _unitsHandler.Hero.StatsData.GetStatValue(Enums.Stat.MaxHealth);
            _ui.HeroView.UpdateHealth(currentHealth, maxHealth);

            List<HeroActionTemplate> baseSkills = _runData.InitialHeroData.HeroTemplate.Actions.Where(a =>
                a.ExtendsFrom == Enums.HeroActionType.Skill || a.ExtendsFrom == Enums.HeroActionType.FirstLegacySkill ||
                a.ExtendsFrom == Enums.HeroActionType.SecondLegacySkill).ToList();

            foreach (var template in baseSkills)
            {
                if (template.Direction == Enums.InputDirection.Horizontal)
                    _ui.HeroView.UpdateSkillIcon(template.ExtendsFrom, template.Icon);
            }
            
            _ui.HeroView.UpdateHealCharges(1);
        }
    }
}