using UnityEngine;
using FNAK.UI.Mobile;

namespace FNAK.Gameplay.Player.Input
{
    public class MobileInputSource : IPlayerInputSource
    {
        private readonly float _touchSensitivity;
        
        private VirtualJoystick _movementJoystick;
        private CameraDeadZone _deadZone;
        private CrouchButton _crouchButton;

        private Vector2 _lastTouchPosition;
        private bool _isTouching = false;
        private int _cameraFingerID = -1;

        public MobileInputSource(
            VirtualJoystick movementJoystick,
            CameraDeadZone deadZone = null,
            CrouchButton crouchButton = null,
            float touchSensitivity = 1.5f)
        {
            _movementJoystick = movementJoystick;
            _deadZone = deadZone;
            _crouchButton = crouchButton;
            _touchSensitivity = touchSensitivity;

            if (_movementJoystick == null)
            {
                Debug.LogWarning("[MobileInputSource] Movement joystick is not assigned!");
            }

            Debug.Log("[MobileInputSource] Mobile input source initialized with joystick support.");
        }

        public Vector2 GetMovement()
        {
            if (_movementJoystick == null)
                return Vector2.zero;

            return _movementJoystick.GetDirection();
        }

        public Vector2 GetLook()
        {
            Vector2 lookDelta = Vector2.zero;

            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                Touch touch = UnityEngine.Input.GetTouch(i);

                if (_movementJoystick != null && _movementJoystick.IsActive)
                {
                    continue;
                }

                if (_deadZone != null && _deadZone.IsPointInDeadZone(touch.position))
                {
                    continue;
                }

                if (touch.phase == TouchPhase.Began)
                {
                    _cameraFingerID = touch.fingerId;
                    _lastTouchPosition = touch.position;
                    _isTouching = true;
                }
                else if (touch.phase == TouchPhase.Moved && touch.fingerId == _cameraFingerID)
                {
                    Vector2 delta = touch.position - _lastTouchPosition;
                    lookDelta = delta * _touchSensitivity * Time.deltaTime;
                    _lastTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended && touch.fingerId == _cameraFingerID)
                {
                    _isTouching = false;
                    _cameraFingerID = -1;
                }
            }

            return lookDelta;
        }

        public bool GetInteractInput()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);
                
                bool inDeadZone = _deadZone != null && _deadZone.IsPointInDeadZone(touch.position);
                bool onJoystick = _movementJoystick != null && _movementJoystick.IsActive;
                
                if (!inDeadZone && !onJoystick && touch.tapCount == 2)
                {
                    return true;
                }
            }

            return false;
        }

        public bool GetPauseInput()
        {
            return UnityEngine.Input.touchCount >= 3;
        }

        public bool GetCrouchInput()
        {
            return false;
        }

        public void SetMovementJoystick(VirtualJoystick joystick)
        {
            _movementJoystick = joystick;
            Debug.Log("[MobileInputSource] Movement joystick assigned.");
        }

        public void SetDeadZone(CameraDeadZone deadZone)
        {
            _deadZone = deadZone;
            Debug.Log("[MobileInputSource] Camera dead zone assigned.");
        }

        public void SetCrouchButton(CrouchButton button)
        {
            _crouchButton = button;
            Debug.Log("[MobileInputSource] Crouch button assigned.");
        }
    }
}
