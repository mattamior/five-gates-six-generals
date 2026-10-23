using UnityEngine;

namespace SanguoLegend.Core
{
    /// <summary>Scene-wide runtime settings. One per playable scene.</summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 60;

        private void Awake()
        {
            Time.timeScale = 1f;
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = 0;
        }

        private void OnDestroy() => Time.timeScale = 1f;
    }
}
