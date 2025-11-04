using System;

namespace FNAK.Core.Data
{
    [Serializable]
    public class GameSettings
    {
        public int TargetFPS { get; set; }
        public string Language { get; set; }
        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SFXVolume { get; set; }

        public GameSettings()
        {
            TargetFPS = 120;
            Language = "en";
            MasterVolume = 1.0f;
            MusicVolume = 0.7f;
            SFXVolume = 1.0f;
        }
    }
}
