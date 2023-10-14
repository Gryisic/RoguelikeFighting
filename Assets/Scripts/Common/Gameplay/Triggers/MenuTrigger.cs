using System;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Triggers
{
    public class MenuTrigger : Trigger
    {
        [SerializeField] private Enums.MenuType _menuType;

        public event Action<Enums.MenuType> Triggered; 

        public override void Execute() => Triggered?.Invoke(_menuType);
    }
}