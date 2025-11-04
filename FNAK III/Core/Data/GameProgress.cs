using System;

namespace FNAK.Core.Data
{
    [Serializable]
    public class GameProgress
    {
        public int CurrentNight { get; set; }
        public int TotalDeaths { get; set; }

        public GameProgress()
        {
            CurrentNight = 0;
            TotalDeaths = 0;
        }
    }
}
