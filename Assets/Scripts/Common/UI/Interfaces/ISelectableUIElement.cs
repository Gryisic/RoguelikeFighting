using System;

namespace Common.UI.Interfaces
{
    public interface ISelectableUIElement
    {
        event Action Entered;
        event Action Exited;
        
        void Select();
        void Undo();
    }
}