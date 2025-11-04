using UnityEngine;
using FNAK.Core.Services;

namespace FNAK.Core.Bootstrap
{
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Initial Settings")]
        [SerializeField] private int initialTargetFPS = 120;

        private void Awake()
        {
            InitializeGlobalSettings();
        }

        private void InitializeGlobalSettings()
        {
            IFrameRateLimiter frameRateLimiter = new FrameRateLimiter();
            frameRateLimiter.SetTargetFrameRate(initialTargetFPS);

            Debug.Log("[GameBootstrap] Global settings initialized.");
        }
    }
}
