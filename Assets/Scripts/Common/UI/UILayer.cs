using UnityEngine;

namespace Common.UI
{
    public class UILayer : UIElement
    {
        [SerializeField] private UIElement[] _elements;

        public bool TryGetElement<T>(out T element) where T: UIElement
        {
            element = null;
            
            foreach (var uiElement in _elements)
            {
                if (uiElement is T concreteElement)
                {
                    element = concreteElement;

                    return true;
                }
            }

            return false;
        }
    }
}