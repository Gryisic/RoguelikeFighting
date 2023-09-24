using Common.Gameplay.Modifiers.Templates;
using Common.Units.Interfaces;
using UnityEngine;

namespace Common.Gameplay.Modifiers
{
    public class FreezeModifier : Modifier
    {
        public FreezeModifier(ModifierTemplate data, Modifier wrappedModifier = null) : base(data, wrappedModifier) { }

        public override void Execute(IUnitInternalData internalData)
        {
            Debug.Log("Freeze");
            
            wrappedModifier?.Execute(internalData);
        }

        protected override T GetDataInternal<T>()
        {
            if (wrappedModifier != null)
                return wrappedModifier.GetData<T>();

            return data as T;
        }
    }
}