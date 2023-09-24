using System;
using System.Linq;
using UnityEngine;

namespace Common.UI
{
    [Serializable]
    public class UI
    {
        [SerializeField] private UIElement[] _uiElements;

        public T GetElementAndCastToType<T>() where T: UIElement
        {
            UIElement element = _uiElements.First(e => e is T);

            return element as T;
        }
    }
}