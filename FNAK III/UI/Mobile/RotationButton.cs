using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FNAK.UI.Mobile
{
    [RequireComponent(typeof(Button))]
    public class RotationButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public enum RotationDirection
        {
            Left = -1,
            Right = 1
        }

        [Header("Settings")]
        [SerializeField] private RotationDirection direction = RotationDirection.Left;
        [SerializeField] private float rotationSpeed = 60f;

        [Header("Visual Feedback")]
        [SerializeField] private Color pressedColor = Color.yellow;
        
        private Button _button;
        private Image _buttonImage;
        private Color _originalColor;
        private bool _isPressed = false;

        public bool IsPressed => _isPressed;

        public float GetRotationSpeed()
        {
            return _isPressed ? rotationSpeed * (int)direction : 0f;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _buttonImage = GetComponent<Image>();

            if (_buttonImage != null)
            {
                _originalColor = _buttonImage.color;
            }

            Debug.Log($"[RotationButton] Rotation button initialized. Direction: {direction}");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;

            if (_buttonImage != null)
            {
                _buttonImage.color = pressedColor;
            }

            Debug.Log($"[RotationButton] Button pressed - rotating {direction}");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;

            if (_buttonImage != null)
            {
                _buttonImage.color = _originalColor;
            }

            Debug.Log($"[RotationButton] Button released");
        }

        #region Validation

        private void OnValidate()
        {
            if (rotationSpeed <= 0)
            {
                rotationSpeed = 60f;
                Debug.LogWarning("[RotationButton] Rotation speed must be positive. Reset to 60.");
            }
        }

        #endregion
    }
}
