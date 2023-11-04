using System;
using System.Collections.Generic;
using System.Threading;
using Common.UI.Interfaces;
using Common.UI.MainMenu.MenuView.Buttons;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Button = Common.UI.MainMenu.MenuView.Buttons.Button;

namespace Common.UI.MainMenu.MenuView
{
    public class MainMenuView : AnimatableUIElement, IVerticallyNavigatableUIElement, ISelectableUIElement
    {
        [SerializeField] private Image _logo;
        [SerializeField] private Button[] _buttons;

        private bool _isInitialized;
        private int _hoveredButtonIndex;
        private int _maxIndex;

        public event Action Selected;
        public event Action Backed;

        public override async UniTask ActivateAsync(CancellationToken token)
        {
            base.Activate();
            
            if (_isInitialized == false)
                Initialize();
            
            _hoveredButtonIndex = 0;
            
            foreach (var button in _buttons)
            {
                button.HoveredViaPointer += HoveredViaPointer;
                
                if (button is PlayButton playButton)
                    playButton.Pressed += OnPlayButtonPressed;
            }

            await DOTween.Sequence()
                .Append(_logo.DOFade(1, 0f).From(0))
                .ToUniTask(cancellationToken: token);

            await ActivateButtons(token);
        }

        public override async UniTask DeactivateAsync(CancellationToken token)
        {
            foreach (var button in _buttons)
            {
                button.HoveredViaPointer -= HoveredViaPointer;
                
                if (button is PlayButton playButton)
                    playButton.Pressed -= OnPlayButtonPressed;
            }
            
            UniTask task1 = DOTween.Sequence()
                .Append(_logo.DOFade(0, 0f).From(1))
                .ToUniTask(cancellationToken: token);

            UniTask task2 = DeactivateButtons(token);

            await UniTask.WhenAll(task1, task2);
            
            base.Deactivate();
        }

        public void MoveUp()
        {
            int currentIndex = _hoveredButtonIndex;
            _hoveredButtonIndex = _hoveredButtonIndex - 1 <= 0 ? 0 : _hoveredButtonIndex - 1;
            
            UpdateHover(currentIndex, _hoveredButtonIndex);
        }

        public void MoveDown()
        {
            int currentIndex = _hoveredButtonIndex;
            _hoveredButtonIndex = _hoveredButtonIndex + 1 >= _maxIndex ? _maxIndex : _hoveredButtonIndex + 1;
            
            UpdateHover(currentIndex, _hoveredButtonIndex);
        }
        
        public void Select() => Selected?.Invoke();

        public void Back() => Backed?.Invoke();

        private void Initialize()
        {
            _isInitialized = true;

            _maxIndex = _buttons.Length - 1;
            
            foreach (var button in _buttons) 
                button.Initialize();
        }

        private void HoveredViaPointer(int index)
        {
            if (index == _hoveredButtonIndex)
                return;
            
            int currentIndex = _hoveredButtonIndex;
            _hoveredButtonIndex = index;
            
            UpdateHover(currentIndex, _hoveredButtonIndex);
        }
        
        private void UpdateHover(int currentIndex, int nextIndex)
        {
            _buttons[currentIndex].UnHover();
            _buttons[nextIndex].Hover();
        }
        
        private void OnPlayButtonPressed() => Selected?.Invoke();
        
        private async UniTask ActivateButtons(CancellationToken token)
        {
            List<UniTask> tasks = new List<UniTask>();

            for (var i = 0; i < _buttons.Length; i++)
            {
                Button button = _buttons[i];
                
                button.SetIndex(i);
                
                tasks.Add(button.ActivateAsync(token));
            }

            await UniTask.WhenAll(tasks);
            
            _buttons[_hoveredButtonIndex].Hover();
        }
        
        private async UniTask DeactivateButtons(CancellationToken token)
        {
            List<UniTask> tasks = new List<UniTask>();
            
            foreach (var button in _buttons) 
                tasks.Add(button.DeactivateAsync(token));
            
            await UniTask.WhenAll(tasks);
        }
    }
}