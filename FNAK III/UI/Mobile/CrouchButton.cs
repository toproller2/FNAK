using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace FNAK.UI.Mobile
{
    [RequireComponent(typeof(Button))]
    public class CrouchButton : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent OnCrouchPressed;

        [Header("Visual")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color crouchedColor = Color.yellow;

        private Button _button;
        private Image _buttonImage;
        private bool _isCrouched = false;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _buttonImage = GetComponent<Image>();

            if (_button != null)
            {
                _button.onClick.AddListener(OnButtonClicked);
            }

            UpdateVisual();
        }

        private void OnButtonClicked()
        {
            _isCrouched = !_isCrouched;
            UpdateVisual();
            
            OnCrouchPressed?.Invoke();
            
            Debug.Log($"[CrouchButton] Button clicked - Crouched: {_isCrouched}");
        }

        private void UpdateVisual()
        {
            if (_buttonImage != null)
            {
                _buttonImage.color = _isCrouched ? crouchedColor : normalColor;
            }
        }

        public void SetCrouched(bool crouched)
        {
            _isCrouched = crouched;
            UpdateVisual();
        }

        public bool IsCrouched => _isCrouched;
    }
}
