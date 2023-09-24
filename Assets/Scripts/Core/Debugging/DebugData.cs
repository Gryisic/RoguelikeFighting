#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Core.Debugging
{
    public static class DebugData
    {
        public static bool ShowGameStateData => EditorPrefs.GetBool("ShowGameStateData");
        private static bool _showGameStateData;
        
        public static bool ShowExploringStateData => EditorPrefs.GetBool("ShowExploringStateData");
        private static bool _showExploringStateData;

        [MenuItem("Debugging/IsShowGameStateData")]
        public static void IsShowGameStateData()
        {
            _showGameStateData = !_showGameStateData;
            
            EditorPrefs.SetBool("ShowGameStateData", _showGameStateData);
            
            Debug.Log($"Is Game State Data Showed : {_showGameStateData}");
        }
        
        [MenuItem("Debugging/IsShowExploringStateData")]
        public static void IsShowExploringStateData()
        {
            _showExploringStateData = !_showExploringStateData;
            
            EditorPrefs.SetBool("ShowExploringStateData", _showExploringStateData);
            
            Debug.Log($"Is Exploring State Data Showed : {_showExploringStateData}");
        }
    }
}
#endif