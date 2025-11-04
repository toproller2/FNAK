using FNAK.Core.Data;

namespace FNAK.Core.Services
{
    public interface IProgressStorage
    {
        void Save(GameProgress progress);
        GameProgress Load();
        bool HasSavedProgress();
        void Clear();
    }
}
