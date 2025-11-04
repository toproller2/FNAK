using UnityEngine;

namespace FNAK.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [Header("State")]
        [SerializeField] private PlayerState currentState = PlayerState.Walking;
        
        [Header("Inventory")]
        [SerializeField] private bool hasFlashDrive = false;
        
        [Header("Crouch Settings")]
        [Range(0.1f, 0.9f)]
        [SerializeField] private float crouchHeightMultiplier = 0.5f;
        
        private CharacterController _characterController;
        
        private bool _isCrouched = false;
        private float _originalHeight;
        private Vector3 _originalCenter;

        public PlayerState State
        {
            get => currentState;
            private set
            {
                if (currentState != value)
                {
                    Debug.Log($"[Player] State changed: {currentState} -> {value}");
                    currentState = value;
                }
            }
        }

        public bool HasFlashDrive
        {
            get => hasFlashDrive;
            set => hasFlashDrive = value;
        }

        public bool IsCrouched => _isCrouched;

        public CharacterController Controller => _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            
            if (_characterController == null)
            {
                Debug.LogError("[Player] CharacterController component not found!");
                return;
            }
            
            _originalHeight = _characterController.height;
            _originalCenter = _characterController.center;
            
            Debug.Log("[Player] Player initialized.");
        }

        public void Hide()
        {
            State = PlayerState.Hiding;
            Debug.Log("[Player] Player is now hiding.");
        }

        public void Walk()
        {
            State = PlayerState.Walking;
            Debug.Log("[Player] Player is now walking.");
        }

        public void InteractWithUI()
        {
            State = PlayerState.InteractingUI;
            Debug.Log("[Player] Player is interacting with UI.");
        }

        public void ToggleCrouch()
        {
            if (_isCrouched)
            {
                StandUp();
            }
            else
            {
                Crouch();
            }
        }

        public void Crouch()
        {
            if (_isCrouched || _characterController == null)
                return;

            _isCrouched = true;

            float newHeight = _originalHeight * crouchHeightMultiplier;
            float heightDifference = _originalHeight - newHeight;

            _characterController.height = newHeight;
            
            _characterController.center = new Vector3(
                _originalCenter.x,
                _originalCenter.y - (heightDifference / 2f),
                _originalCenter.z
            );

            Debug.Log($"[Player] 🙇 Player crouched. Height: {_originalHeight:F2} -> {newHeight:F2} (x{crouchHeightMultiplier})");
        }

        public void StandUp()
        {
            if (!_isCrouched || _characterController == null)
                return;

            _isCrouched = false;

            _characterController.height = _originalHeight;
            _characterController.center = _originalCenter;

            Debug.Log($"[Player] 🚶 Player stood up. Height restored to {_originalHeight:F2}");
        }

        public void MoveTo(Vector3 position)
        {
            if (_characterController != null && _characterController.enabled)
            {
                _characterController.Move(position - transform.position);
            }
        }

        public bool CanMove()
        {
            return State == PlayerState.Walking;
        }

        #region Validation

        private void OnValidate()
        {
            if (crouchHeightMultiplier <= 0 || crouchHeightMultiplier >= 1)
            {
                crouchHeightMultiplier = 0.5f;
                Debug.LogWarning("[Player] Crouch height multiplier must be between 0.1 and 0.9. Reset to 0.5.");
            }
        }

        #endregion
    }
}
