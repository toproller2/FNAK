using UnityEngine;
using FNAK.Gameplay.Player;
using FNAK.Core.Services;
using FNAK.Core.Constants;

namespace FNAK.Gameplay
{
    public class Night1Controller : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerController playerController;

        [Header("Settings")]
        [SerializeField] private int targetFPS = 120;

        private IFrameRateLimiter _frameRateLimiter;
        private ISceneLoader _sceneLoader;

        private void Awake()
        {
            Debug.Log("=== [Night1Controller] 🌙 NIGHT 1 STARTING ===");
            
            InitializeDependencies();
            
            _frameRateLimiter.SetTargetFrameRate(targetFPS);
            
            ValidateComponents();
            
            Debug.Log("[Night1Controller] Night 1 initialized successfully.");
        }

        private void InitializeDependencies()
        {
            _frameRateLimiter = new FrameRateLimiter();
            _sceneLoader = new SceneLoader();

            Debug.Log("[Night1Controller] Dependencies initialized.");
        }

        private void ValidateComponents()
        {
            if (playerController == null)
            {
                Debug.LogError("[Night1Controller] ❌ Player Controller is not assigned!");
                return;
            }

            Debug.Log("[Night1Controller] ✅ All components validated successfully.");
        }

        public void OnNightCompleted()
        {
            Debug.Log("[Night1Controller] 🎉 NIGHT 1 COMPLETED!");
            
            _sceneLoader.LoadMainMenu();
        }

        public void OnNightFailed()
        {
            Debug.Log("[Night1Controller] 💀 NIGHT 1 FAILED (Game Over)!");
            
            _sceneLoader.ReloadCurrentScene();
        }

        #region Validation

        private void OnValidate()
        {
            if (playerController == null)
            {
                Debug.LogWarning("[Night1Controller] Player Controller is not assigned!");
            }

            if (targetFPS <= 0)
            {
                targetFPS = 120;
                Debug.LogWarning("[Night1Controller] Target FPS must be positive. Reset to 120.");
            }
        }

        #endregion

        #region Debug Commands

        [ContextMenu("Complete Night (Debug)")]
        private void DebugCompleteNight()
        {
            OnNightCompleted();
        }

        [ContextMenu("Fail Night (Debug)")]
        private void DebugFailNight()
        {
            OnNightFailed();
        }

        #endregion
    }
}
