using UnityEngine;
using UnityEngine.SceneManagement;
using FNAK.Core.Constants;

namespace FNAK.Core.Services
{
    public class SceneLoader : ISceneLoader
    {
        public void LoadNight(int nightNumber)
        {
            if (!SceneConstants.IsValidNight(nightNumber))
            {
                Debug.LogError($"[SceneLoader] Invalid night number: {nightNumber}. Must be between {SceneConstants.MIN_NIGHT} and {SceneConstants.MAX_NIGHT}.");
                return;
            }

            if (SceneConstants.TryGetSceneForNight(nightNumber, out string sceneName))
            {
                Debug.Log($"[SceneLoader] Loading scene: {sceneName} (Night {nightNumber})");
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError($"[SceneLoader] Scene not found for night {nightNumber}!");
            }
        }

        public void LoadMainMenu()
        {
            Debug.Log($"[SceneLoader] Loading scene: {SceneConstants.SCENE_MAIN_MENU}");
            SceneManager.LoadScene(SceneConstants.SCENE_MAIN_MENU);
        }

        public void ReloadCurrentScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            Debug.Log($"[SceneLoader] Reloading current scene: {currentScene.name}");
            SceneManager.LoadScene(currentScene.name);
        }
        
        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("[SceneLoader] Scene name is null or empty!");
                return;
            }
            
            Debug.Log($"[SceneLoader] Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
    }
}
