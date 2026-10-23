using System.Collections.Generic;
using SanguoLegend.Combat;
using SanguoLegend.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SanguoLegend.Narrative
{
    /// <summary>Minimal level loop: chapter card on entry, clear state when all enemies are dead.</summary>
    public sealed class LevelDirector : MonoBehaviour
    {
        [SerializeField] private string chapterTitle = "第一关 · 东岭关";
        [SerializeField] private string chapterSubtitle = "过五关斩六将";
        [SerializeField] private float titleDuration = 3.5f;

        private readonly List<Damageable> enemies = new();
        private float titleUntil;
        private Damageable player;
        private bool cleared;
        private bool failed;

        public bool Cleared => cleared;

        private void Start()
        {
            foreach (var candidate in FindObjectsOfType<Damageable>())
            {
                if (candidate.Faction == Damageable.Team.Player)
                {
                    player = candidate;
                    player.Died += OnPlayerDied;
                    continue;
                }

                if (candidate.Faction != Damageable.Team.Enemy) continue;

                enemies.Add(candidate);
                candidate.Died += OnEnemyDied;
            }

            titleUntil = Time.time + titleDuration;
        }

        private void Update()
        {
            if (!failed || !Input.GetKeyDown(KeyCode.R)) return;

            Time.timeScale = 1f;
            var scene = SceneManager.GetActiveScene();
            if (!string.IsNullOrEmpty(scene.path))
                SceneManager.LoadScene(scene.path);
            else
                Debug.LogError("Cannot restart: the active scene has no saved asset path.");
        }

        private void OnDestroy()
        {
            foreach (var enemy in enemies)
                if (enemy != null) enemy.Died -= OnEnemyDied;

            if (player != null) player.Died -= OnPlayerDied;
        }

        private void OnPlayerDied(Damageable _)
        {
            failed = true;

            var motor = player.GetComponent<PlayerMotor>();
            if (motor != null) motor.enabled = false;

            var combat = player.GetComponent<PlayerCombat>();
            if (combat != null) combat.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }

        private void OnEnemyDied(Damageable enemy)
        {
            enemies.Remove(enemy);
            if (enemies.Count == 0) cleared = true;
        }

        private void OnGUI()
        {
            if (Time.time < titleUntil)
            {
                var fade = Mathf.Clamp01(Mathf.Min(Time.time / 0.6f, (titleUntil - Time.time) / 0.8f));
                var titleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 42,
                    fontStyle = FontStyle.Bold
                };
                var subtitleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 20
                };

                var previous = GUI.color;
                GUI.color = new Color(0.92f, 0.86f, 0.72f, fade);
                GUI.Label(new Rect(0f, Screen.height * 0.36f, Screen.width, 60f), chapterTitle, titleStyle);
                GUI.Label(new Rect(0f, Screen.height * 0.36f + 58f, Screen.width, 40f), chapterSubtitle, subtitleStyle);
                GUI.color = previous;
            }

            if (failed)
            {
                var defeatStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 42,
                    fontStyle = FontStyle.Bold
                };
                var hintStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 20
                };

                GUI.Label(new Rect(0f, Screen.height * 0.43f, Screen.width, 60f), "战败", defeatStyle);
                GUI.Label(new Rect(0f, Screen.height * 0.43f + 58f, Screen.width, 40f), "按 R 重新挑战", hintStyle);
                return;
            }

            if (!cleared) return;

            var clearStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 36,
                fontStyle = FontStyle.Bold
            };
            GUI.Label(new Rect(0f, Screen.height * 0.45f, Screen.width, 60f), "关隘已破", clearStyle);
        }
    }
}
