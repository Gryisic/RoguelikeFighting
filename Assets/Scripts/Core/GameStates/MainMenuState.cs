using System;
using System.Threading;
using Common.Gameplay.Interfaces;
using Common.UI;
using Common.UI.Extensions;
using Common.UI.MainMenu.MenuView;
using Common.UI.MainMenu.SelectionView;
using Common.Units.Selection;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Infrastructure.Utils;

namespace Core.GameStates
{
    public class MainMenuState : IGameState, IDeactivatable, IDisposable
    {
        private readonly IGameStateSwitcher _stateSwitcher;
        private readonly IInputService _inputService;
        private readonly IRunData _runData;
        private readonly UI _ui;
        private readonly SelectionHeroesDatabase _heroesDatabase;

        private MainMenuView _mainMenuView;
        private UnitSelectionView _unitSelectionView;
        
        private AnimatableUIElement _activeUI;

        private CancellationTokenSource _animationsTokenSource;
        
        public MainMenuState(IGameStateSwitcher stateSwitcher, IServicesHandler servicesHandler, IRunData runData, UI ui, SelectionHeroesDatabase heroesDatabase)
        {
            _stateSwitcher = stateSwitcher;
            _inputService = servicesHandler.InputService;
            _runData = runData;
            _ui = ui;
            _heroesDatabase = heroesDatabase;
        }
        
        public void Dispose()
        {
            _animationsTokenSource?.Cancel();
            _animationsTokenSource?.Dispose();
        }

        public void Activate(GameStateArgs args)
        {
            _mainMenuView = _ui.Get<MainMenuView>();
            _unitSelectionView = _ui.Get<UnitSelectionView>();
            
            _ui.gameObject.SetActive(false);
            _unitSelectionView.SetData(_heroesDatabase.Templates);
            
            ToMainMenuView();
            SubscribeToEvents();
        }

        public void Deactivate()
        {
            DeAttachInput();
            UnsubscribeToEvents();
            
            _ui.gameObject.SetActive(true);
        }

        private void AttachInput()
        {
            _inputService.Input.Menu.Up.performed += _activeUI.MoveUp;
            _inputService.Input.Menu.Down.performed += _activeUI.MoveDown;
            _inputService.Input.Menu.Left.performed += _activeUI.MoveLeft;
            _inputService.Input.Menu.Right.performed += _activeUI.MoveRight;
            _inputService.Input.Menu.Select.performed += _activeUI.Select;
            _inputService.Input.Menu.Back.performed += _activeUI.Undo;
            
            _inputService.Input.Enable();
        }
        
        private void DeAttachInput()
        {
            _inputService.Input.Disable();
            
            _inputService.Input.Menu.Up.performed -= _activeUI.MoveUp;
            _inputService.Input.Menu.Down.performed -= _activeUI.MoveDown;
            _inputService.Input.Menu.Left.performed -= _activeUI.MoveLeft;
            _inputService.Input.Menu.Right.performed -= _activeUI.MoveRight;
            _inputService.Input.Menu.Select.performed -= _activeUI.Select;
            _inputService.Input.Menu.Back.performed -= _activeUI.Undo;
        }
        
        private void SubscribeToEvents()
        {
            _mainMenuView.Selected += ToUnitSelectionView;
            _unitSelectionView.Backed += ToMainMenuView;
            _unitSelectionView.UnitSelected += OnUnitSelected;
        }

        private void UnsubscribeToEvents()
        {
            _mainMenuView.Selected -= ToUnitSelectionView;
            _unitSelectionView.Backed -= ToMainMenuView;
            _unitSelectionView.UnitSelected -= OnUnitSelected;
        }
        
        private void ToNextScene()
        {
            _activeUI.Deactivate();
            
            _stateSwitcher.SwitchState<SceneSwitchState>(new SceneSwitchArgs(Enums.SceneType.Arena, Enums.GameStateType.Gameplay));
        }
        
        private void OnUnitSelected(SelectionHeroTemplate template)
        {
            _runData.SetHeroTemplate(template.HeroTemplate);

            DOTween.Sequence().AppendInterval(1f).AppendCallback(ToNextScene);
        }

        private void ToUnitSelectionView()
        {
            _animationsTokenSource.Cancel();
            _animationsTokenSource = new CancellationTokenSource();

            ToView(_unitSelectionView).Forget();
        }

        private void ToMainMenuView()
        {
            _animationsTokenSource = new CancellationTokenSource();

            ToView(_mainMenuView);
        }

        private async UniTask ToView(AnimatableUIElement nextView)
        {
            DeAttachInput();
            
            if (_activeUI != null)
                await _activeUI.DeactivateAsync(_animationsTokenSource.Token);
            
            _activeUI = nextView;
            
            await _activeUI.ActivateAsync(_animationsTokenSource.Token);
            
            AttachInput();
        }
    }
}