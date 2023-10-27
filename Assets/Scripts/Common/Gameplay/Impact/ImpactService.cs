using Common.Gameplay.Interfaces;
using Common.Scene.Cameras.Interfaces;

namespace Common.Gameplay.Impact
{
    public class ImpactService : IImpactService
    {
        private readonly ICameraService _cameraService;
        
        public ImpactService(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }
        
        public void ShakeCamera()
        {
            
        }
    }
}