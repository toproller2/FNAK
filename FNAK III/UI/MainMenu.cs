using UnityEngine;
using FNAK.Core.Services;
using FNAK.Core.Data;
using FNAK.Core.Constants;

namespace FNAK.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource buttonClickAudioSource;

        [Header("References")]
        [SerializeField] private GlitchScreenTransition glitchTransition;

        [Header("Settings")]
        [SerializeField] private int targetFPS = 120;
        [SerializeField] private float sceneLoadDelay = 2.0f;
        [SerializeField] private float quitDelay = 1.5f;

        private IAudioService _audioService;
        private IFrameRateLimiter _frameRateLimiter;
        private IGameApplication _gameApplication;
        private IGlitchScreenTransition _glitchTransition;
        private ISceneLoader _sceneLoader;
        private IProgressStorage _progressStorage;

        private GameProgress _currentProgress;

        private void Awake()
        {
            InitializeDependencies();
            
            _frameRateLimiter.SetTargetFrameRate(targetFPS);
            
            LoadProgress();
        }

        private void InitializeDependencies()
        {
            _audioService = new AudioService();
            _frameRateLimiter = new FrameRateLimiter();
            _gameApplication = new GameApplication();
            _sceneLoader = new SceneLoader();
            _progressStorage = new PlayerPrefsProgressStorage();
            _glitchTransition = glitchTransition;

            Debug.Log("[MainMenu] Dependencies initialized successfully.");
        }

        private void LoadProgress()
        {
            _currentProgress = _progressStorage.Load();
            
            if (_progressStorage.HasSavedProgress())
            {
                Debug.Log($"[MainMenu] 📊 Loaded saved progress: Current Night = {_currentProgress.CurrentNight}, Total Deaths = {_currentProgress.TotalDeaths}");
            }
            else
            {
                Debug.Log("[MainMenu] 📊 No saved progress found. Starting new game.");
            }
        }

        public void OnPlayButtonClicked()
        {
            Debug.Log("=== [MainMenu] 🎮 PLAY BUTTON CLICKED ===");
            Debug.Log($"[MainMenu] Current Progress: Night {_currentProgress.CurrentNight}");
            
            _audioService.PlayOneShot(buttonClickAudioSource);
            Debug.Log("[MainMenu] 🔊 Button click sound played.");
            
            int nightToLoad = DetermineNightToLoad();
            Debug.Log($"[MainMenu] 🌙 Night to load: {nightToLoad}");
            
            _glitchTransition.TriggerGlitchScreen();
            Debug.Log("[MainMenu] ✨ Glitch screen animation triggered.");
            
            Invoke(nameof(LoadNightScene), sceneLoadDelay);
            Debug.Log($"[MainMenu] ⏰ Scene will load in {sceneLoadDelay} seconds.");
        }

        public void OnLeaveButtonClicked()
        {
            Debug.Log("=== [MainMenu] 🚪 LEAVE BUTTON CLICKED ===");
            Debug.Log("[MainMenu] User is exiting the application.");
            
            _audioService.PlayOneShot(buttonClickAudioSource);
            Debug.Log("[MainMenu] 🔊 Button click sound played.");
            
            _glitchTransition.TriggerGlitchScreen();
            Debug.Log("[MainMenu] ✨ Glitch screen animation triggered.");
            
            Invoke(nameof(QuitApplication), quitDelay);
            Debug.Log($"[MainMenu] ⏰ Application will quit in {quitDelay} seconds.");
        }

        private int DetermineNightToLoad()
        {
            if (_currentProgress.CurrentNight == 0)
            {
                Debug.Log("[MainMenu] No progress found. Starting from Night 1.");
                return SceneConstants.FIRST_NIGHT;
            }
            
            if (_currentProgress.CurrentNight >= SceneConstants.MAX_NIGHT)
            {
                Debug.Log("[MainMenu] All nights completed! Restarting from Night 1.");
                return SceneConstants.FIRST_NIGHT;
            }
            
            int nightToLoad = _currentProgress.CurrentNight + 1;
            Debug.Log($"[MainMenu] Continuing from Night {nightToLoad}.");
            return nightToLoad;
        }

        private void LoadNightScene()
        {
            int nightNumber = DetermineNightToLoad();
            
            if (!SceneConstants.IsValidNight(nightNumber))
            {
                Debug.LogError($"[MainMenu] ❌ Invalid night number: {nightNumber}. Must be between {SceneConstants.MIN_NIGHT} and {SceneConstants.MAX_NIGHT}.");
                return;
            }

            if (SceneConstants.TryGetSceneForNight(nightNumber, out string sceneName))
            {
                Debug.Log($"[MainMenu] 🌙 Loading scene: {sceneName} (Night {nightNumber})");
                
                _currentProgress.CurrentNight = nightNumber;
                _progressStorage.Save(_currentProgress);
                
                _sceneLoader.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError($"[MainMenu] ❌ Scene not found for night {nightNumber}!");
            }
        }

        private void QuitApplication()
        {
            Debug.Log("[MainMenu] 👋 Quitting application now...");
            _gameApplication.Quit();
        }

        #region Validation
        
        private void OnValidate()
        {
            if (buttonClickAudioSource == null)
            {
                Debug.LogWarning("[MainMenu] Button Click Audio Source is not assigned!");
            }

            if (glitchTransition == null)
            {
                Debug.LogWarning("[MainMenu] Glitch Transition component is not assigned!");
            }

            if (targetFPS <= 0)
            {
                targetFPS = 60;
                Debug.LogWarning("[MainMenu] Target FPS must be positive. Reset to 60.");
            }
            
            if (sceneLoadDelay < 0)
            {
                sceneLoadDelay = 2.0f;
                Debug.LogWarning("[MainMenu] Scene load delay cannot be negative. Reset to 2.0.");
            }
            
            if (quitDelay < 0)
            {
                quitDelay = 1.5f;
                Debug.LogWarning("[MainMenu] Quit delay cannot be negative. Reset to 1.5.");
            }
        }
        
        #endregion
        
        #region Debug Commands
        
        [ContextMenu("Reset Progress")]
        public void DebugResetProgress()
        {
            _progressStorage.Clear();
            LoadProgress();
            Debug.Log("[MainMenu] 🔄 Progress has been reset!");
        }
        
        #endregion
    }
}

