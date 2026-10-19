using UnityEngine;

namespace SanguoLegend.Combat
{
    /// <summary>Brief time-scale dip on hit. Uses unscaled time so the effect can end while paused.</summary>
    public static class HitStop
    {
        private const float TimeScale = 0.05f;
        private const float DefaultFixedDeltaTime = 0.02f;

        private static HitStopRunner runner;
        private static float remaining;

        public static void Play(float seconds)
        {
            if (seconds <= 0f) return;

            remaining = Mathf.Max(remaining, seconds);
            Time.timeScale = TimeScale;
            Time.fixedDeltaTime = DefaultFixedDeltaTime * TimeScale;

            if (runner != null) return;

            var host = new GameObject("HitStopRunner") { hideFlags = HideFlags.HideInHierarchy };
            Object.DontDestroyOnLoad(host);
            runner = host.AddComponent<HitStopRunner>();
        }

        internal static void Tick(float unscaledDeltaTime)
        {
            if (remaining <= 0f) return;

            remaining -= unscaledDeltaTime;
            if (remaining > 0f) return;

            remaining = 0f;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = DefaultFixedDeltaTime;
        }
    }

    internal sealed class HitStopRunner : MonoBehaviour
    {
        private void Update() => HitStop.Tick(Time.unscaledDeltaTime);
    }
}
