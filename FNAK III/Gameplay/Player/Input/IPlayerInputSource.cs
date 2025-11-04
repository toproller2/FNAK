using UnityEngine;

namespace FNAK.Gameplay.Player.Input
{
    public interface IPlayerInputSource
    {
        Vector2 GetMovement();
        Vector2 GetLook();
        bool GetInteractInput();
        bool GetPauseInput();
        bool GetCrouchInput();
    }
}
