using SanguoLegend.Combat;
using SanguoLegend.Player;
using UnityEngine;

namespace SanguoLegend.UI
{
    /// <summary>Temporary IMGUI HUD for the graybox. Replaced by uGUI once the art pass starts.</summary>
    public sealed class DebugHud : MonoBehaviour
    {
        private Damageable health;
        private PlayerMotor motor;
        private float fps;

        private void Awake()
        {
            health = GetComponent<Damageable>();
            motor = GetComponent<PlayerMotor>();
        }

        private void Update()
        {
            fps = Mathf.Lerp(fps, 1f / Mathf.Max(Time.unscaledDeltaTime, 0.0001f), 0.1f);

            if (Input.GetKeyDown(KeyCode.Escape)) ReleaseCursor();
        }

        private static void ReleaseCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnGUI()
        {
            var style = new GUIStyle(GUI.skin.label) { fontSize = 16 };

            GUILayout.BeginArea(new Rect(24f, 24f, 520f, 220f));
            GUILayout.Label($"HP {health.Health:0}/{health.MaxHealth:0}", style);
            GUILayout.Label($"FPS {fps:0}", style);
            GUILayout.Label(motor.IsMounted ? "骑乘" : "步行", style);
            GUILayout.Space(8f);
            GUILayout.Label("WASD 移动 · Shift 疾跑 · 左键 轻击 · Q 重击 · 右键 格挡 · F 骑乘", style);
            GUILayout.EndArea();
        }
    }
}
