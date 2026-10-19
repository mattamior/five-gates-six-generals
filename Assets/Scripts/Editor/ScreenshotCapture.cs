using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SanguoLegend.EditorTools
{
    /// <summary>Six orbit shots around the play area, used for visual review by a separate agent.</summary>
    public static class ScreenshotCapture
    {
        private const int Width = 1920;
        private const int Height = 1080;
        private const string OutputDirectory = "Screenshots";

        private static string ProjectRoot => Path.GetDirectoryName(Application.dataPath) ?? ".";

        [MenuItem("Sanguo/Capture Scene Shots")]
        public static void CaptureMenu()
        {
            var scene = EditorSceneManager.GetActiveScene();
            if (string.IsNullOrEmpty(scene.path))
            {
                Debug.LogError("Save the scene before capturing shots.");
                return;
            }

            Capture(scene.path);
        }

        /// <summary>Batchmode entry point: opens the graybox, captures, leaves the project untouched.</summary>
        public static void CaptureFromCommandLine()
        {
            var sceneAsset = Path.Combine(ProjectRoot, GrayboxBuilder.ScenePath);
            if (!File.Exists(sceneAsset))
            {
                Debug.LogError($"Missing scene: {GrayboxBuilder.ScenePath}. Run Sanguo/Build Graybox Scene first.");
                EditorApplication.Exit(1);
                return;
            }

            EditorSceneManager.OpenScene(GrayboxBuilder.ScenePath, OpenSceneMode.Single);
            Capture(GrayboxBuilder.ScenePath);
        }

        private static void Capture(string scenePath)
        {
            var outputDirectory = Path.Combine(ProjectRoot, OutputDirectory);
            Directory.CreateDirectory(outputDirectory);

            var cameraObject = new GameObject("ShotCamera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 55f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 500f;

            var target = new RenderTexture(Width, Height, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = target;

            var focus = new Vector3(0f, 1.4f, 6f);
            const float radius = 20f;

            for (var i = 0; i < 6; i++)
            {
                var angle = i * 60f * Mathf.Deg2Rad;
                camera.transform.position = new Vector3(Mathf.Sin(angle) * radius, 7.5f, Mathf.Cos(angle) * radius) + focus;
                camera.transform.LookAt(focus);
                camera.Render();

                RenderTexture.active = target;
                var texture = new Texture2D(Width, Height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0f, 0f, Width, Height), 0, 0);
                texture.Apply();

                File.WriteAllBytes(Path.Combine(outputDirectory, $"shot_{i:00}.png"), texture.EncodeToPNG());
                Object.DestroyImmediate(texture);
            }

            RenderTexture.active = null;
            camera.targetTexture = null;
            Object.DestroyImmediate(target);
            Object.DestroyImmediate(cameraObject);

            Debug.Log($"Captured 6 shots from {scenePath} into {OutputDirectory}/");
        }
    }
}
