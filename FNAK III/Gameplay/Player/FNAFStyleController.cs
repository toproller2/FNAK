using UnityEngine;
using FNAK.Gameplay.Player.Input;
using FNAK.UI.Mobile;

namespace FNAK.Gameplay.Player
{
    public class FNAFStyleController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Transform playerCamera;
        
        [Header("Rotation Settings")]
        [SerializeField] private bool limitRotation = true;
        [SerializeField] private float minYaw = -80f;
        [SerializeField] private float maxYaw = 80f;
        [Range(0.1f, 5.0f)]
        [SerializeField] private float rotationSpeedMultiplier = 1.0f;

        [Header("Rotation Buttons")]
        [SerializeField] private RotationButton leftRotationButton;
        [SerializeField] private RotationButton rightRotationButton;

        [Header("Action Buttons")]
        [SerializeField] private ActionButton hideButton;
        [SerializeField] private ActionButton openCameraButton;

        [Header("Look Detection")]
        [SerializeField] private LookTargetDetector lookDetector;

        private Player _player;
        private IPlayerInputSource _inputSource;

        private float _currentYaw = 0f;

        private void Awake()
        {
            _player = GetComponent<Player>();
            
            InitializeInputSource();
            
            ConnectActionButtons();
            
            Debug.Log("[FNAFStyleController] FNAF-style controller initialized.");
        }

        private void InitializeInputSource()
        {
            if (leftRotationButton == null)
            {
                Debug.LogError("[FNAFStyleController] ❌ Left Rotation Button is NOT assigned!");
            }
            else
            {
                Debug.Log("[FNAFStyleController] ✅ Left Rotation Button assigned.");
            }

            if (rightRotationButton == null)
            {
                Debug.LogError("[FNAFStyleController] ❌ Right Rotation Button is NOT assigned!");
            }
            else
            {
                Debug.Log("[FNAFStyleController] ✅ Right Rotation Button assigned.");
            }

            _inputSource = new ButtonInputSource(leftRotationButton, rightRotationButton);
            
            string rotationMode = limitRotation ? $"Limited ({minYaw}° to {maxYaw}°)" : "Unlimited (360°)";
            Debug.Log($"[FNAFStyleController] Button input source initialized. Rotation: {rotationMode}, Speed Multiplier: x{rotationSpeedMultiplier}");
        }

        private void ConnectActionButtons()
        {
            if (hideButton != null)
            {
                hideButton.OnActionPressed.AddListener(OnHideButtonPressed);
            }

            if (openCameraButton != null)
            {
                openCameraButton.OnActionPressed.AddListener(OnOpenCameraButtonPressed);
            }

            Debug.Log("[FNAFStyleController] Action buttons connected.");
        }

        private void Update()
        {
            HandleCameraRotation();
        }

        private void HandleCameraRotation()
        {
            if (playerCamera == null)
            {
                Debug.LogWarning("[FNAFStyleController] Player camera is not assigned!");
                return;
            }

            Vector2 lookInput = _inputSource.GetLook();

            float rotationDelta = lookInput.x * rotationSpeedMultiplier;

            if (rotationDelta != 0)
            {
                Debug.Log($"[FNAFStyleController] 🔄 Rotation delta: {rotationDelta:F2}, Current Yaw: {_currentYaw:F2}, Multiplier: x{rotationSpeedMultiplier}");
            }

            _currentYaw += rotationDelta;

            if (limitRotation)
            {
                _currentYaw = Mathf.Clamp(_currentYaw, minYaw, maxYaw);
            }
            else
            {
                _currentYaw = Mathf.Repeat(_currentYaw, 360f);
            }

            transform.rotation = Quaternion.Euler(0, _currentYaw, 0);
        }

        private void OnHideButtonPressed()
        {
            Debug.Log("[FNAFStyleController] 🙈 Hide button pressed!");
            
            if (_player != null)
            {
                _player.Hide();
            }
        }

        private void OnOpenCameraButtonPressed()
        {
            Debug.Log("[FNAFStyleController] 📹 Open Camera button pressed!");
        }

        #region Public Methods

        public void SetLeftRotationButton(RotationButton button)
        {
            leftRotationButton = button;
            
            if (_inputSource is ButtonInputSource buttonInput)
            {
                buttonInput.SetLeftButton(button);
            }
        }

        public void SetRightRotationButton(RotationButton button)
        {
            rightRotationButton = button;
            
            if (_inputSource is ButtonInputSource buttonInput)
            {
                buttonInput.SetRightButton(button);
            }
        }

        public float GetCurrentYaw()
        {
            return _currentYaw;
        }

        [ContextMenu("Reset Rotation")]
        public void ResetRotation()
        {
            _currentYaw = 0f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("[FNAFStyleController] Camera rotation reset to center.");
        }

        public void SetRotationLimit(bool enabled)
        {
            limitRotation = enabled;
            string mode = enabled ? $"Limited ({minYaw}° to {maxYaw}°)" : "Unlimited (360°)";
            Debug.Log($"[FNAFStyleController] Rotation limit {(enabled ? "enabled" : "disabled")}: {mode}");
        }

        public void SetRotationSpeedMultiplier(float multiplier)
        {
            if (multiplier < 0.1f)
            {
                Debug.LogWarning("[FNAFStyleController] Speed multiplier too low. Minimum is 0.1");
                multiplier = 0.1f;
            }
            else if (multiplier > 5.0f)
            {
                Debug.LogWarning("[FNAFStyleController] Speed multiplier too high. Maximum is 5.0");
                multiplier = 5.0f;
            }

            rotationSpeedMultiplier = multiplier;
            Debug.Log($"[FNAFStyleController] Rotation speed multiplier set to: x{multiplier}");
        }

        #endregion

        #region Validation

        private void OnValidate()
        {
            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>()?.transform;
                
                if (playerCamera == null)
                {
                    Debug.LogWarning("[FNAFStyleController] Player Camera is not assigned!");
                }
            }

            if (minYaw >= maxYaw)
            {
                minYaw = -80f;
                maxYaw = 80f;
                Debug.LogWarning("[FNAFStyleController] Min yaw must be less than max yaw. Reset to defaults.");
            }

            if (leftRotationButton == null)
            {
                Debug.LogWarning("[FNAFStyleController] Left Rotation Button is not assigned!");
            }

            if (rightRotationButton == null)
            {
                Debug.LogWarning("[FNAFStyleController] Right Rotation Button is not assigned!");
            }
        }

        #endregion
    }
}
