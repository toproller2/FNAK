using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace FNAK.UI.Mobile
{
    [RequireComponent(typeof(Button))]
    public class ActionButton : MonoBehaviour
    {
        public enum ActionType
        {
            Hide,
            OpenCamera,
            Custom
        }

        [Header("Settings")]
        [SerializeField] private ActionType actionType = ActionType.Hide;
        [SerializeField] private string actionName = "Action";

        [Header("Animation")]
        [SerializeField] private Animator buttonAnimator;
        [SerializeField] private string pressedTrigger = "Pressed";

        [Header("Events")]
        public UnityEvent OnActionPressed;

        private Button _button;

        public ActionType Type => actionType;

        private void Awake()
        {
            _button = GetComponent<Button>();

            if (_button != null)
            {
                _button.onClick.AddListener(OnButtonClicked);
            }

            Debug.Log($"[ActionButton] Action button initialized. Type: {actionType}, Name: {actionName}");
        }

        private void OnButtonClicked()
        {
            Debug.Log($"[ActionButton] 🎮 Action pressed: {actionName} ({actionType})");

            if (buttonAnimator != null && !string.IsNullOrEmpty(pressedTrigger))
            {
                buttonAnimator.SetTrigger(pressedTrigger);
            }

            OnActionPressed?.Invoke();
        }

        public void Press()
        {
            OnButtonClicked();
        }

        public void SetInteractable(bool interactable)
        {
            if (_button != null)
            {
                _button.interactable = interactable;
            }
        }

        #region Validation

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(actionName))
            {
                actionName = actionType.ToString();
            }

            if (buttonAnimator == null)
            {
                buttonAnimator = GetComponent<Animator>();
            }
        }

        #endregion
    }
}
