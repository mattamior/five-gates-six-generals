using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SanguoLegend.EditorTools
{
    /// <summary>macOS player build. Also callable from batchmode via -executeMethod.</summary>
    public static class BuildScript
    {
        private const string OutputPath = "Builds/macOS/SanguoLegend.app";

        [MenuItem("Sanguo/Build macOS")]
        public static void BuildMac()
        {
            var scenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError("Build aborted: no enabled scenes in Build Settings.");
                EditorApplication.Exit(1);
                return;
            }

            var output = Path.Combine(Path.GetDirectoryName(Application.dataPath) ?? ".", OutputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(output) ?? ".");

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = output,
                target = BuildTarget.StandaloneOSX,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;
            Debug.Log($"Build {summary.result}: {summary.totalSize / (1024f * 1024f):0.0} MB, {summary.totalErrors} errors, {summary.totalWarnings} warnings");

            if (summary.result != BuildResult.Succeeded) EditorApplication.Exit(1);
        }
    }
}
