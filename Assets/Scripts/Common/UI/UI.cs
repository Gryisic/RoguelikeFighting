using System;
using UnityEngine;

namespace Common.UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private UILayer[] _layers;
        
        public T Get<T>() where T: UIElement
        {
            foreach (var layer in _layers)
            {
                if (layer.TryGetElement(out T element))
                    return element;
            }

            throw new InvalidOperationException($"Trying to get ui element that's not presented in ui layers. Element: {typeof(T)}");
        }
    }
}