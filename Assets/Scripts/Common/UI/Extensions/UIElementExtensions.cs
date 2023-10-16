using Common.UI.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.UI.Extensions
{
    public static class UIElementExtensions
    {
        public static void MoveRight(this UIElement element, InputAction.CallbackContext context)
        {
            if (element is IHorizontalNavigatableUIElement navigatable)
                navigatable.MoveRight();
        }
        
        public static void MoveLeft(this UIElement element, InputAction.CallbackContext context)
        {
            if (element is IHorizontalNavigatableUIElement navigatable)
                navigatable.MoveLeft();
        }
        
        public static void MoveUp(this UIElement element, InputAction.CallbackContext context)
        {
            if (element is IVerticalNavigatableUIElement navigatable)
                navigatable.MoveUp();
        }
        
        public static void MoveDown(this UIElement element, InputAction.CallbackContext context)
        {
            if (element is IVerticalNavigatableUIElement navigatable)
                navigatable.MoveDown();
        }
        
        public static void Select(this UIElement element, InputAction.CallbackContext context)
        {
            if (element is ISelectableUIElement selectable) 
                selectable.Select();
        }
        
        public static void Undo(this UIElement element, InputAction.CallbackContext context)
        {
            if (element is ISelectableUIElement selectable)
                selectable.Undo();
        }
    }
}