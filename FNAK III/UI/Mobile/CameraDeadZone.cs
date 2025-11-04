using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace FNAK.UI.Mobile
{
    public class CameraDeadZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Header("Settings")]
        [SerializeField] private bool enableDeadZone = true;

        [Header("Visual")]
        [SerializeField] private Color deadZoneColor = new Color(1f, 0f, 0f, 0.2f);
        [SerializeField] private bool showVisual = true;

        private bool _isPointerInside = false;
        private RectTransform _rectTransform;
        private Image _image;

        public bool IsPointerInside => _isPointerInside && enableDeadZone;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();

            if (_image != null)
            {
                _image.color = showVisual ? deadZoneColor : new Color(0, 0, 0, 0);
                _image.raycastTarget = true;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPointerInside = true;
            Debug.Log("[CameraDeadZone] Pointer entered dead zone - camera rotation disabled");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPointerInside = false;
            Debug.Log("[CameraDeadZone] Pointer left dead zone - camera rotation enabled");
        }

        public void OnDrag(PointerEventData eventData)
        {
            _isPointerInside = RectTransformUtility.RectangleContainsScreenPoint(
                _rectTransform,
                eventData.position,
                eventData.pressEventCamera
            );
        }

        public bool IsPointInDeadZone(Vector2 screenPoint, Camera camera = null)
        {
            if (!enableDeadZone || _rectTransform == null)
                return false;

            return RectTransformUtility.RectangleContainsScreenPoint(
                _rectTransform,
                screenPoint,
                camera
            );
        }

        public void SetEnabled(bool enabled)
        {
            enableDeadZone = enabled;
            
            if (!enabled)
            {
                _isPointerInside = false;
            }

            Debug.Log($"[CameraDeadZone] Dead zone {(enabled ? "enabled" : "disabled")}");
        }

        public void SetVisualEnabled(bool enabled)
        {
            showVisual = enabled;
            
            if (_image != null)
            {
                _image.color = enabled ? deadZoneColor : new Color(0, 0, 0, 0);
            }
        }
    }
}
