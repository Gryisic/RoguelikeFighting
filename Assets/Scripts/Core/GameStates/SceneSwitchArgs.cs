using Infrastructure.Utils;

namespace Core.GameStates
{
    public class SceneSwitchArgs : GameStateArgs
    {
        public Enums.SceneType NextSceneType { get; }

        public SceneSwitchArgs(Enums.SceneType nextSceneType)
        {
            NextSceneType = nextSceneType;
        }
    }
}