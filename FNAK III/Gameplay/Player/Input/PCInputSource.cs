using UnityEngine;

namespace FNAK.Gameplay.Player.Input
{
    public class PCInputSource : IPlayerInputSource
    {
        private readonly float _mouseSensitivity;

        public PCInputSource(float mouseSensitivity = 2.0f)
        {
            _mouseSensitivity = mouseSensitivity;
        }

        public Vector2 GetMovement()
        {
            float horizontal = UnityEngine.Input.GetAxis("Horizontal");
            float vertical = UnityEngine.Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(horizontal, vertical);
            
            if (movement.magnitude > 1f)
            {
                movement.Normalize();
            }

            return movement;
        }

        public Vector2 GetLook()
        {
            float mouseX = UnityEngine.Input.GetAxis("Mouse X") * _mouseSensitivity;
            float mouseY = UnityEngine.Input.GetAxis("Mouse Y") * _mouseSensitivity;

            return new Vector2(mouseX, mouseY);
        }

        public bool GetInteractInput()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.E) || 
                   UnityEngine.Input.GetMouseButtonDown(0);
        }

        public bool GetPauseInput()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.Escape);
        }

        public bool GetCrouchInput()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.LeftControl) ||
                   UnityEngine.Input.GetKeyDown(KeyCode.RightControl);
        }
    }
}
