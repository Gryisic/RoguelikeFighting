using System;

namespace Common.UI.MainMenu.MenuView.Buttons
{
    public class PlayButton : Button
    {
        public override event Action Pressed;

        public override void Execute() => Pressed?.Invoke();
    }
}