namespace FNAK.Core.Services
{
    public interface ISceneLoader
    {
        void LoadNight(int nightNumber);
        void LoadMainMenu();
        void ReloadCurrentScene();
        void LoadScene(string sceneName);
    }
}
