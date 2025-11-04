using UnityEngine;
using FNAK.Core.Data;

namespace FNAK.Core.Services
{
    public class PlayerPrefsProgressStorage : IProgressStorage
    {
        private const string KEY_CURRENT_NIGHT = "FNAK_CurrentNight";
        private const string KEY_TOTAL_DEATHS = "FNAK_TotalDeaths";
        private const string KEY_HAS_SAVE = "FNAK_HasSave";

        public void Save(GameProgress progress)
        {
            if (progress == null)
            {
                Debug.LogWarning("[PlayerPrefsProgressStorage] Cannot save null progress.");
                return;
            }

            PlayerPrefs.SetInt(KEY_CURRENT_NIGHT, progress.CurrentNight);
            PlayerPrefs.SetInt(KEY_TOTAL_DEATHS, progress.TotalDeaths);
            PlayerPrefs.SetInt(KEY_HAS_SAVE, 1);
            PlayerPrefs.Save();

            Debug.Log($"[PlayerPrefsProgressStorage] Progress saved: Night {progress.CurrentNight}, Deaths {progress.TotalDeaths}");
        }

        public GameProgress Load()
        {
            if (!HasSavedProgress())
            {
                Debug.Log("[PlayerPrefsProgressStorage] No saved progress found. Creating new progress.");
                return new GameProgress();
            }

            var progress = new GameProgress
            {
                CurrentNight = PlayerPrefs.GetInt(KEY_CURRENT_NIGHT, 0),
                TotalDeaths = PlayerPrefs.GetInt(KEY_TOTAL_DEATHS, 0)
            };

            Debug.Log($"[PlayerPrefsProgressStorage] Progress loaded: Night {progress.CurrentNight}, Deaths {progress.TotalDeaths}");
            return progress;
        }

        public bool HasSavedProgress()
        {
            return PlayerPrefs.HasKey(KEY_HAS_SAVE) && PlayerPrefs.GetInt(KEY_HAS_SAVE) == 1;
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(KEY_CURRENT_NIGHT);
            PlayerPrefs.DeleteKey(KEY_TOTAL_DEATHS);
            PlayerPrefs.DeleteKey(KEY_HAS_SAVE);
            PlayerPrefs.Save();

            Debug.Log("[PlayerPrefsProgressStorage] Progress cleared.");
        }
    }
}
