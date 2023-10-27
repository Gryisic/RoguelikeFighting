using Common.Models.Actions;
using Common.Models.Actions.Templates;
using UnityEngine;

namespace Common.Units.Interfaces
{
    public interface ILegacyUnit : IUnitInternalData
    {
        LegacyActionTemplate ActionData { get; }
        
        void Activate(Vector2 position);
        void Deactivate();
        void MoveTo(Vector2 position, float speed);
    }
}