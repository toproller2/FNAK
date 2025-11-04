using UnityEngine;

namespace FNAK.Core.Services
{
    public class FrameRateLimiter : IFrameRateLimiter
    {
        public void SetTargetFrameRate(int targetFps)
        {
            Application.targetFrameRate = targetFps;
            Debug.Log($"[FrameRateLimiter] Target FPS set to: {targetFps}");
        }

        public int GetTargetFrameRate()
        {
            return Application.targetFrameRate;
        }
    }
}
