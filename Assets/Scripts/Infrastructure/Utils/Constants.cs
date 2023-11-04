namespace Infrastructure.Utils
{
    public static class Constants
    {
        public const int StartSceneIndex = 0;
        public const int FloorLayerIndex = 7;
        public const int SafeNumberOfStepsInLoops = 500;

        public const int ActivatedCameraPriority = 100;
        public const int DeactivatedCameraPriority = 0;
        
        public const float NeutralFollowCameraSize = 5f;
        public const float CloseFollowCameraSize = 3.5f;
        public const float FarFollowCameraSize = 7.5f;
        
        public const float NeutralFocusCameraSize = 5f;
        public const float CloseFocusCameraSize = 3.5f;
        public const float FarFocusCameraSize = 7f;

        public const float DefaultCameraBlendTime = 0.2f;

        public const int ExperienceNeededToRequestNextModifier = 100;
        public const int DefaultAmountOfModifiersToRequest = 100;

        public const int DefaultExperiencePerBattleAmount = 75;
        public const float ExtensiveBattleExperienceMultiplier = 2f;
        
        public const int DefaultAmountOfGald = 100;
        public const int MaxAmountOfGald = 9999;
        public const int MinAmountOfGald = 0;

        public const int DefaultHealCharges = 2;
        public const int MaxHealCharges = 5;
        public const int MinHealCharges = 0;

        public const int MaxTradeItemsAmount = 4;
        public const int MinTradeItemsAmount = 0;
        
        public const int InitialCopiesOfUnit = 5;
        public const int InitialCopiesOfProjectiles = 15;

        public const int DefaultStatMultiplier = 1;
        
        public const int MaxStatValue = 9999;
        public const int MinStatValue = 0;

        public const float InteractionRadius = 2f;
        public const float PlayerMovementSpeed = 5f;
        public const float FallMultiplier = 3f;
        public const float LinearVelocitySlowdownSpeed = 0.9f;
        public const float DashRefreshingTime = 2f;
        public const float JumpForce = 15f;
        public const float DropThroughPlatformTime = 0.25f;
        
        public const float ProjectileDangerZoneTime = 0.75f;

        public const float DefaultLegacyUnitSpawnDistance = 5f;

        public const float DefaultEnemyAwaitTime = 2f;

        public const float UnitSelectionCardHorizontalOffset = 4.7f;
        public const float UnitSelectionCardPortraitVerticalOffset = 1.25f;

        public const float StorageSpinPrewarmTime = 1f;
        public const float StorageSpinTime = 3f;
        public const float StorageSpinAfterimageTime = 1f;
        
        public const float ModifierCardHoverVerticalOffset = 110;
        public const float ModifierCardActivationVerticalOffset = 810;

        public const string PathToUnitPrefabs = "Units";
        public const string PathToProjectilesPrefabs = "Projectiles";
        public const string PathToDialogueAssets = "Dialogues";
        
        public const string ShaderHealthValueReference = "_HealthValue";
        public const string DissolveValueReference = "_DissolvePercent";
    }
}