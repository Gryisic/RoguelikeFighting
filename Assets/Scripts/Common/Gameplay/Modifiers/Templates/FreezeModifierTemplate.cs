using Infrastructure.Utils;
using UnityEngine;

namespace Common.Gameplay.Modifiers.Templates
{
    [CreateAssetMenu(menuName = "Configs/Templates/Modifiers/Freeze", fileName = "Template")]
    public class FreezeModifierTemplate : ModifierTemplate
    {
        public override Enums.Modifier Type => Enums.Modifier.Freeze;
    }
}