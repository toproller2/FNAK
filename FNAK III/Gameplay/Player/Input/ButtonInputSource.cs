using UnityEngine;
using FNAK.UI.Mobile;

namespace FNAK.Gameplay.Player.Input
{
    public class ButtonInputSource : IPlayerInputSource
    {
        private RotationButton _leftButton;
        private RotationButton _rightButton;

        public ButtonInputSource(RotationButton leftButton, RotationButton rightButton)
        {
            _leftButton = leftButton;
            _rightButton = rightButton;

            Debug.Log("[ButtonInputSource] Button input source initialized.");
        }

        public Vector2 GetMovement()
        {
            return Vector2.zero;
        }

        public Vector2 GetLook()
        {
            float horizontalRotation = 0f;

            if (_leftButton != null && _leftButton.IsPressed)
            {
                horizontalRotation += _leftButton.GetRotationSpeed() * Time.deltaTime;
            }

            if (_rightButton != null && _rightButton.IsPressed)
            {
                horizontalRotation += _rightButton.GetRotationSpeed() * Time.deltaTime;
            }

            return new Vector2(horizontalRotation, 0f);
        }

        public bool GetInteractInput()
        {
            return false;
        }

        public bool GetPauseInput()
        {
            return false;
        }

        public bool GetCrouchInput()
        {
            return false;
        }

        public void SetLeftButton(RotationButton button)
        {
            _leftButton = button;
        }

        public void SetRightButton(RotationButton button)
        {
            _rightButton = button;
        }
    }
}
