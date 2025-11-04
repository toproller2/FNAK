using UnityEngine;
using FNAK.Gameplay.Player.Input;
using FNAK.UI.Mobile;

namespace FNAK.Gameplay.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 5.0f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Camera Settings")]
        [SerializeField] private Transform playerCamera;
        [SerializeField] private float mouseSensitivity = 2.0f;
        [SerializeField] private float touchSensitivity = 1.5f;
        [SerializeField] private float minPitch = -90f;
        [SerializeField] private float maxPitch = 90f;

        [Header("Input Settings")]
        [SerializeField] private bool usePCInput = true;
        
        [Header("Mobile UI Components (для Mobile режима)")]
        [SerializeField] private VirtualJoystick movementJoystick;
        [SerializeField] private CameraDeadZone cameraDeadZone;
        [SerializeField] private CrouchButton crouchButton;
        
        [Header("Crosshair")]
        [SerializeField] private bool showCrosshair = true;

        private Player _player;
        private IPlayerInputSource _inputSource;

        private float _pitch = 0f;
        private float _yaw = 0f;

        private Vector3 _velocity;

        private void Awake()
        {
            _player = GetComponent<Player>();
            
            InitializeInputSource();
            
            if (usePCInput)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            
            Debug.Log("[PlayerController] Player controller initialized.");
        }

        private void InitializeInputSource()
        {
            if (usePCInput)
            {
                _inputSource = new PCInputSource(mouseSensitivity);
                Debug.Log("[PlayerController] Using PC input (Keyboard + Mouse).");
            }
            else
            {
                var mobileInput = new MobileInputSource(
                    movementJoystick,
                    cameraDeadZone,
                    crouchButton,
                    touchSensitivity
                );
                
                _inputSource = mobileInput;
                
                if (crouchButton != null)
                {
                    crouchButton.OnCrouchPressed.AddListener(OnCrouchButtonPressed);
                }
                
                Debug.Log("[PlayerController] Using Mobile input (Touch + Virtual Joysticks).");
            }
        }

        private void Update()
        {
            if (_inputSource.GetPauseInput())
            {
                HandlePause();
            }

            if (_player.CanMove())
            {
                HandleMovement();
                HandleCamera();
            }

            if (_inputSource.GetCrouchInput())
            {
                _player.ToggleCrouch();
            }

            if (_inputSource.GetInteractInput())
            {
                HandleInteraction();
            }
        }

        private void HandleMovement()
        {
            Vector2 input = _inputSource.GetMovement();

            Vector3 forward = playerCamera.forward;
            Vector3 right = playerCamera.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * input.y + right * input.x).normalized;

            Vector3 move = moveDirection * walkSpeed * Time.deltaTime;

            if (!_player.Controller.isGrounded)
            {
                _velocity.y += gravity * Time.deltaTime;
            }
            else
            {
                _velocity.y = -2f;
            }

            move += _velocity * Time.deltaTime;

            _player.Controller.Move(move);
        }

        private void HandleCamera()
        {
            if (playerCamera == null)
            {
                Debug.LogWarning("[PlayerController] Player camera is not assigned!");
                return;
            }

            Vector2 lookInput = _inputSource.GetLook();

            _yaw += lookInput.x;
            _pitch -= lookInput.y;

            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(0, _yaw, 0);
            playerCamera.localRotation = Quaternion.Euler(_pitch, 0, 0);
        }

        private void HandleInteraction()
        {
            Debug.Log("[PlayerController] Interact button pressed!");
        }

        private void HandlePause()
        {
            Debug.Log("[PlayerController] Pause button pressed!");
        }

        private void OnCrouchButtonPressed()
        {
            _player.ToggleCrouch();
        }

        private void OnGUI()
        {
            if (showCrosshair && usePCInput)
            {
                DrawCrosshair();
            }
        }

        private void DrawCrosshair()
        {
            float crosshairSize = 10f;
            float thickness = 2f;
            Color crosshairColor = Color.white;

            float centerX = Screen.width / 2f;
            float centerY = Screen.height / 2f;

            GUI.DrawTexture(
                new Rect(centerX - crosshairSize, centerY - thickness / 2, crosshairSize * 2, thickness),
                Texture2D.whiteTexture,
                ScaleMode.StretchToFill,
                true,
                0,
                crosshairColor,
                0,
                0
            );

            GUI.DrawTexture(
                new Rect(centerX - thickness / 2, centerY - crosshairSize, thickness, crosshairSize * 2),
                Texture2D.whiteTexture,
                ScaleMode.StretchToFill,
                true,
                0,
                crosshairColor,
                0,
                0
            );
        }

        #region Public Methods

        public void SetMovementJoystick(VirtualJoystick joystick)
        {
            movementJoystick = joystick;
            
            if (_inputSource is MobileInputSource mobileInput)
            {
                mobileInput.SetMovementJoystick(joystick);
            }
        }

        public void SetCameraDeadZone(CameraDeadZone deadZone)
        {
            cameraDeadZone = deadZone;
            
            if (_inputSource is MobileInputSource mobileInput)
            {
                mobileInput.SetDeadZone(deadZone);
            }
        }

        public void SetCrouchButton(CrouchButton button)
        {
            crouchButton = button;
            
            if (_inputSource is MobileInputSource mobileInput)
            {
                mobileInput.SetCrouchButton(button);
            }
            
            if (button != null)
            {
                button.OnCrouchPressed.AddListener(OnCrouchButtonPressed);
            }
        }

        #endregion

        #region Validation

        private void OnValidate()
        {
            if (playerCamera == null)
            {
                Debug.LogWarning("[PlayerController] Player Camera is not assigned!");
            }

            if (walkSpeed <= 0)
            {
                walkSpeed = 5.0f;
                Debug.LogWarning("[PlayerController] Walk speed must be positive. Reset to 5.0.");
            }

            if (mouseSensitivity <= 0)
            {
                mouseSensitivity = 2.0f;
                Debug.LogWarning("[PlayerController] Mouse sensitivity must be positive. Reset to 2.0.");
            }
            
            if (touchSensitivity <= 0)
            {
                touchSensitivity = 1.5f;
                Debug.LogWarning("[PlayerController] Touch sensitivity must be positive. Reset to 1.5.");
            }

            if (!usePCInput)
            {
                if (movementJoystick == null)
                {
                    Debug.LogWarning("[PlayerController] Movement Joystick is not assigned (required for Mobile mode)!");
                }

                if (cameraDeadZone == null)
                {
                    Debug.LogWarning("[PlayerController] Camera Dead Zone is not assigned (optional for Mobile mode).");
                }

                if (crouchButton == null)
                {
                    Debug.LogWarning("[PlayerController] Crouch Button is not assigned (optional for Mobile mode).");
                }
            }
        }

        #endregion
    }
}
