using System;
using Core.PlayerInput;

namespace Core.Interfaces
{
    public interface IInputService : IDisposable
    {
        Input Input { get; }
    }
}