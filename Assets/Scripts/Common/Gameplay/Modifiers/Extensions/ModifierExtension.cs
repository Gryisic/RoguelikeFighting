using System;

namespace Common.Gameplay.Modifiers.Extensions
{
    public static class ModifierExtension
    {
        public static void Dispose(this Modifier modifier)
        {
            if (modifier is IDisposable disposable)
                disposable.Dispose();
        }
    }
}