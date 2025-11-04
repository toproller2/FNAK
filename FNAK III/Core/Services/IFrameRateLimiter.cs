namespace FNAK.Core.Services
{
    public interface IFrameRateLimiter
    {
        void SetTargetFrameRate(int targetFps);
        int GetTargetFrameRate();
    }
}
