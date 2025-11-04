using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FNAK.UI.Mobile
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Header("Joystick Components")]
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;

        [Header("Settings")]
        [SerializeField] private float handleRange = 50f;
        [Range(0f, 1f)]
        [SerializeField] private float deadZone = 0.1f;

        [Header("Visual")]
        [SerializeField] private Color backgroundColor = new Color(1f, 1f, 1f, 0.3f);
        [SerializeField] private Color handleColor = new Color(1f, 1f, 1f, 0.8f);

        private Vector2 _inputDirection = Vector2.zero;
        private bool _isActive = false;

        public Vector2 Direction => _inputDirection;

        public bool IsActive => _isActive;

        private void Start()
        {
            ValidateComponents();
            SetupVisuals();
            ResetJoystick();
        }

        private void ValidateComponents()
        {
            if (background == null)
            {
                Debug.LogError("[VirtualJoystick] Background is not assigned!");
            }

            if (handle == null)
            {
                Debug.LogError("[VirtualJoystick] Handle is not assigned!");
            }
        }

        private void SetupVisuals()
        {
            if (background != null)
            {
                Image bgImage = background.GetComponent<Image>();
                if (bgImage != null)
                {
                    bgImage.color = backgroundColor;
                }
            }

            if (handle != null)
            {
                Image handleImage = handle.GetComponent<Image>();
                if (handleImage != null)
                {
                    handleImage.color = handleColor;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isActive = true;
            OnDrag(eventData);
            Debug.Log($"[VirtualJoystick] Pointer down at {eventData.position}");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isActive = false;
            ResetJoystick();
            Debug.Log("[VirtualJoystick] Pointer up - joystick reset");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (background == null || handle == null)
                return;

            Vector2 position;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background,
                eventData.position,
                eventData.pressEventCamera,
                out position))
            {
                position = position / (background.sizeDelta / 2f);

                Vector2 direction = position.magnitude > 1f ? position.normalized : position;

                float magnitude = direction.magnitude;
                if (magnitude < deadZone)
                {
                    direction = Vector2.zero;
                }
                else
                {
                    float adjustedMagnitude = (magnitude - deadZone) / (1f - deadZone);
                    direction = direction.normalized * adjustedMagnitude;
                }

                _inputDirection = direction;

                Vector2 clampedPosition = position * handleRange;
                if (clampedPosition.magnitude > handleRange)
                {
                    clampedPosition = clampedPosition.normalized * handleRange;
                }
                handle.anchoredPosition = clampedPosition;
            }
        }

        private void ResetJoystick()
        {
            _inputDirection = Vector2.zero;
            
            if (handle != null)
            {
                handle.anchoredPosition = Vector2.zero;
            }
        }

        public Vector2 GetDirection()
        {
            return _inputDirection;
        }

        #region Validation

        private void OnValidate()
        {
            if (handleRange <= 0)
            {
                handleRange = 50f;
                Debug.LogWarning("[VirtualJoystick] Handle range must be positive. Reset to 50.");
            }
        }

        #endregion
    }
}
