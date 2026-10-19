using SanguoLegend.Data;
using UnityEditor;
using UnityEngine;

namespace SanguoLegend.EditorTools
{
    /// <summary>One-time project defaults. Safe to re-run.</summary>
    public static class ProjectSetup
    {
        private const string DonglingLevelPath = "Assets/Data/Level_Dongling.asset";

        [MenuItem("Sanguo/Setup Project")]
        public static void Setup()
        {
            PlayerSettings.colorSpace = ColorSpace.Linear;
            PlayerSettings.companyName = "Sanguo Lab";
            PlayerSettings.productName = "单骑·过五关";
            PlayerSettings.defaultScreenWidth = 1920;
            PlayerSettings.defaultScreenHeight = 1080;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.runInBackground = true;

            if (!AssetDatabase.IsValidFolder("Assets/Data")) AssetDatabase.CreateFolder("Assets", "Data");
            if (AssetDatabase.LoadAssetAtPath<LevelDefinition>(DonglingLevelPath) == null)
            {
                var level = ScriptableObject.CreateInstance<LevelDefinition>();
                AssetDatabase.CreateAsset(level, DonglingLevelPath);
            }

            AssetDatabase.SaveAssets();
            Debug.Log("Project setup complete: Linear color space, 1920x1080 windowed, product name set.");
        }
    }
}
