using System.Collections.Generic;

namespace FNAK.Core.Constants
{
    public static class SceneConstants
    {
        public const string SCENE_MAIN_MENU = "MainMenu";
        public const string SCENE_NIGHT_1 = "Night1";
        public const string SCENE_NIGHT_2 = "Night2";
        public const string SCENE_NIGHT_3 = "Night3";
        public const string SCENE_NIGHT_4 = "Night4";
        public const string SCENE_NIGHT_5 = "Night5";

        public const int MIN_NIGHT = 1;
        public const int MAX_NIGHT = 5;
        public const int FIRST_NIGHT = 1;

        private static readonly Dictionary<int, string> NightToSceneMap = new Dictionary<int, string>
        {
            { 1, SCENE_NIGHT_1 },
            { 2, SCENE_NIGHT_2 },
            { 3, SCENE_NIGHT_3 },
            { 4, SCENE_NIGHT_4 },
            { 5, SCENE_NIGHT_5 }
        };

        public static bool TryGetSceneForNight(int nightNumber, out string sceneName)
        {
            return NightToSceneMap.TryGetValue(nightNumber, out sceneName);
        }

        public static bool IsValidNight(int nightNumber)
        {
            return nightNumber >= MIN_NIGHT && nightNumber <= MAX_NIGHT;
        }
    }
}
