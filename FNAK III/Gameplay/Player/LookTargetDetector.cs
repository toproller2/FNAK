using UnityEngine;
using FNAK.Gameplay.Interaction;

namespace FNAK.Gameplay.Player
{
    public class LookTargetDetector : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float maxDistance = 10f;
        [SerializeField] private LayerMask targetLayers = -1;

        [Header("Debug")]
        [SerializeField] private bool showDebugRay = true;
        [SerializeField] private bool enableDebugLogs = true;

        private ILookTarget _currentTarget;
        private GameObject _currentTargetObject;

        private void Update()
        {
            CheckLookTarget();
        }

        private void CheckLookTarget()
        {
            if (playerCamera == null)
            {
                Debug.LogWarning("[LookTargetDetector] Player camera is not assigned!");
                return;
            }

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (showDebugRay)
            {
                Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
            }

            if (Physics.Raycast(ray, out hit, maxDistance, targetLayers))
            {
                ILookTarget target = hit.collider.GetComponent<ILookTarget>();

                if (target != null)
                {
                    if (_currentTarget != target)
                    {
                        if (_currentTarget != null)
                        {
                            _currentTarget.OnLookExit();
                        }

                        _currentTarget = target;
                        _currentTargetObject = hit.collider.gameObject;
                        _currentTarget.OnLookEnter();

                        if (enableDebugLogs)
                        {
                            Debug.Log($"[LookTargetDetector] 🎯 Looking at NEW target: {target.GetTargetName()}");
                        }
                    }
                    else
                    {
                        _currentTarget.OnLookStay();
                    }

                    return;
                }
            }

            if (_currentTarget != null)
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[LookTargetDetector] 👁️ Stopped looking at: {_currentTarget.GetTargetName()}");
                }

                _currentTarget.OnLookExit();
                _currentTarget = null;
                _currentTargetObject = null;
            }
        }

        public ILookTarget GetCurrentTarget()
        {
            return _currentTarget;
        }

        public bool IsLookingAtTarget()
        {
            return _currentTarget != null;
        }

        public bool IsLookingAt(GameObject targetObject)
        {
            return _currentTargetObject == targetObject;
        }

        #region Validation

        private void OnValidate()
        {
            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>();
                
                if (playerCamera == null)
                {
                    Debug.LogWarning("[LookTargetDetector] Player Camera is not assigned!");
                }
            }

            if (maxDistance <= 0)
            {
                maxDistance = 10f;
                Debug.LogWarning("[LookTargetDetector] Max distance must be positive. Reset to 10.");
            }
        }

        #endregion

        #region Debug Visualization

        private void OnDrawGizmos()
        {
            if (playerCamera == null || !showDebugRay) return;

            Gizmos.color = _currentTarget != null ? Color.green : Color.red;
            Vector3 direction = playerCamera.transform.forward * maxDistance;
            Gizmos.DrawRay(playerCamera.transform.position, direction);

            if (_currentTarget != null && _currentTargetObject != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(playerCamera.transform.position, _currentTargetObject.transform.position);
            }
        }

        #endregion
    }
}
