namespace FNAK.Gameplay.Interaction
{
    public interface ILookTarget
    {
        void OnLookEnter();
        void OnLookStay();
        void OnLookExit();
        string GetTargetName();
    }
}
