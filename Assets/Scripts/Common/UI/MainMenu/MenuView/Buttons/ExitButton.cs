using System;
using UnityEngine;

namespace Common.UI.MainMenu.MenuView.Buttons
{
    public class ExitButton : Button
    {
        public override event Action Pressed;
        
        public override void Execute() => Application.Quit();
    }
}