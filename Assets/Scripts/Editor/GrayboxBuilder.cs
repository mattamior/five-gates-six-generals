using System.Collections.Generic;
using SanguoLegend.CameraRig;
using SanguoLegend.Combat;
using SanguoLegend.Core;
using SanguoLegend.Enemies;
using SanguoLegend.Narrative;
using SanguoLegend.Player;
using SanguoLegend.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SanguoLegend.EditorTools
{
    /// <summary>Builds the Dongling Pass graybox from primitives so gameplay work can start before art exists.</summary>
    public static class GrayboxBuilder
    {
        public const string ScenePath = "Assets/Scenes/Graybox_Dongling.unity";

        [MenuItem("Sanguo/Build Graybox Scene")]
        public static void Build()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Scenes")) AssetDatabase.CreateFolder("Assets", "Scenes");
            if (!AssetDatabase.IsValidFolder("Assets/Settings")) AssetDatabase.CreateFolder("Assets", "Settings");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var groundMaterial = GetOrCreateMaterial("Assets/Settings/Graybox_Ground.mat", new Color(0.30f, 0.28f, 0.24f));
            var playerMaterial = GetOrCreateMaterial("Assets/Settings/Graybox_Player.mat", new Color(0.14f, 0.30f, 0.20f));
            var enemyMaterial = GetOrCreateMaterial("Assets/Settings/Graybox_Enemy.mat", new Color(0.44f, 0.13f, 0.11f));
            var generalMaterial = GetOrCreateMaterial("Assets/Settings/Graybox_General.mat", new Color(0.30f, 0.10f, 0.24f));

            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(10f, 1f, 10f);
            ground.GetComponent<Renderer>().sharedMaterial = groundMaterial;

            var sun = new GameObject("Sun");
            var light = sun.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1f, 0.95f, 0.86f);
            light.intensity = 1.1f;
            light.shadows = LightShadows.Soft;
            sun.transform.rotation = Quaternion.Euler(48f, -35f, 0f);

            var player = BuildPlayer(playerMaterial);
            BuildCamera(player.transform);

            SpawnEnemy(new Vector3(3f, 1.05f, 10f), enemyMaterial, 45f, 2.0f, 8f);
            SpawnEnemy(new Vector3(-4f, 1.05f, 12f), enemyMaterial, 45f, 2.1f, 8f);
            SpawnEnemy(new Vector3(5.5f, 1.05f, 14f), enemyMaterial, 45f, 1.9f, 9f);
            SpawnEnemy(new Vector3(-1.5f, 1.05f, 18f), generalMaterial, 160f, 1.5f, 14f);

            var flow = new GameObject("LevelFlow");
            flow.AddComponent<LevelDirector>();

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AddToBuildSettings(ScenePath);

            Selection.activeGameObject = player;
            Debug.Log($"Graybox scene built: {ScenePath}");
        }

        private static GameObject BuildPlayer(Material material)
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.tag = "Player";
            player.transform.position = new Vector3(0f, 1.05f, -6f);
            player.GetComponent<Renderer>().sharedMaterial = material;

            Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());

            var character = player.AddComponent<CharacterController>();
            character.height = 2f;
            character.radius = 0.42f;
            character.center = Vector3.zero;

            player.AddComponent<PlayerMotor>();
            player.AddComponent<PlayerCombat>();
            player.AddComponent<MountController>();
            player.AddComponent<GameBootstrap>();
            player.AddComponent<DebugHud>();

            var health = player.AddComponent<Damageable>();
            health.Configure(Damageable.Team.Player, 100f);

            return player;
        }

        private static void BuildCamera(Transform target)
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 3.2f, -10.5f);

            var camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 55f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 500f;
            cameraObject.AddComponent<AudioListener>();

            var rig = cameraObject.AddComponent<ThirdPersonCamera>();
            rig.SetTarget(target);
        }

        private static void SpawnEnemy(Vector3 position, Material material, float health, float speed, float damage)
        {
            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Soldier";
            enemy.transform.position = position;
            enemy.GetComponent<Renderer>().sharedMaterial = material;

            var damageable = enemy.AddComponent<Damageable>();
            damageable.Configure(Damageable.Team.Enemy, health);

            var agent = enemy.AddComponent<EnemyAgent>();
            var serialized = new SerializedObject(agent);
            serialized.FindProperty("moveSpeed").floatValue = speed;
            serialized.FindProperty("attackDamage").floatValue = damage;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Material GetOrCreateMaterial(string path, Color color)
        {
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;

            var shader = Shader.Find("Standard");
            if (shader == null)
            {
                Debug.LogError("Standard shader not found. The project may already be using URP.");
                shader = Shader.Find("Universal Render Pipeline/Lit");
            }

            var material = new Material(shader) { color = color };
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static void AddToBuildSettings(string path)
        {
            var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            if (scenes.Exists(scene => scene.path == path)) return;

            scenes.Insert(0, new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
