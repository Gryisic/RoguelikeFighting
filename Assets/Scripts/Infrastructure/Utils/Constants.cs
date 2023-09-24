namespace Infrastructure.Utils
{
    public static class Constants
    {
        public const int StartSceneIndex = 0;

        public const int ActivatedCameraPriority = 100;
        public const int DeactivatedCameraPriority = 0;
        
        public const float NeutralFollowCameraSize = 5f;
        public const float CloseFollowCameraSize = 3.5f;
        public const float FarFollowCameraSize = 7.5f;
        
        public const float NeutralFocusCameraSize = 3.5f;
        public const float CloseFocusCameraSize = 1f;
        public const float FarFocusCameraSize = 5f;

        public const int InitialCopiesOfUnit = 5;
        public const int InitialCopiesOfProjectiles = 15;
        
        public const int MaxStatValue = 9999;
        public const int MinStatValue = 0;
        
        public const float PlayerMovementSpeed = 5f;
        public const float FallMultiplier = 3f;
        public const float LinearVelocitySlowdownSpeed = 0.9f;
        public const float DashRefreshingTime = 2f;
        public const float JumpForce = 15f;
        public const float DropThroughPlatformTime = 0.25f;

        public const string PathToUnitPrefabs = "Units";
        public const string PathToProjectilesPrefabs = "Projectiles";
        public const string PathToDialogueAssets = "Dialogues";
    }
}