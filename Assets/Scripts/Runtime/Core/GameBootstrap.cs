using UnityEngine;

namespace SanguoLegend.Core
{
    /// <summary>Scene-wide runtime settings. One per playable scene.</summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 60;

        private void Awake()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = 0;
        }
    }
}
