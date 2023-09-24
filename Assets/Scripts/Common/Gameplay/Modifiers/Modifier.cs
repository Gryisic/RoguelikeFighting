using Common.Gameplay.Modifiers.Templates;
using Common.Units.Interfaces;

namespace Common.Gameplay.Modifiers
{
    public abstract class Modifier
    {
        protected readonly ModifierTemplate data;
        protected readonly Modifier wrappedModifier;

        public ModifierTemplate DefaultData => data;

        protected Modifier(ModifierTemplate data, Modifier wrappedModifier = null)
        {
            this.data = data;
            this.wrappedModifier = wrappedModifier;
        }

        public T GetData<T>() where T : ModifierTemplate => GetDataInternal<T>();
        
        public abstract void Execute(IUnitInternalData internalData);

        protected abstract T GetDataInternal<T>() where T: ModifierTemplate;
    }
}