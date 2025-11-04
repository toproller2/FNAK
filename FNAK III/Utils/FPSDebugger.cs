using UnityEngine;

namespace FNAK.Utils
{
    public class FPSDebugger : MonoBehaviour
    {
        [Header("Debug Settings")]
        [SerializeField] private bool enableFPSDebug = true;
        [SerializeField] private float updateInterval = 1.0f;

        private float _accumulatedTime = 0f;
        private int _frameCount = 0;
        private float _currentFPS = 0f;

        private void Update()
        {
            if (!enableFPSDebug) return;

            _accumulatedTime += Time.deltaTime;
            _frameCount++;

            if (_accumulatedTime >= updateInterval)
            {
                _currentFPS = _frameCount / _accumulatedTime;

                Debug.Log($"[FPS Debug] Current FPS: {_currentFPS:F2} | Target FPS: {Application.targetFrameRate}");

                _accumulatedTime = 0f;
                _frameCount = 0;
            }
        }
        
        public void SetDebugEnabled(bool enabled)
        {
            enableFPSDebug = enabled;
        }

        public float GetCurrentFPS()
        {
            return _currentFPS;
        }
    }
}
