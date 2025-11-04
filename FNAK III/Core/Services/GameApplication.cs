using UnityEngine;

namespace FNAK.Core.Services
{
    public class GameApplication : IGameApplication
    {
        public void Quit()
        {
            Debug.Log("[GameApplication] Quitting application...");
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
