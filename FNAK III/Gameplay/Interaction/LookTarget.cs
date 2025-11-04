using UnityEngine;
using UnityEngine.Events;

namespace FNAK.Gameplay.Interaction
{
    public class LookTarget : MonoBehaviour, ILookTarget
    {
        [Header("Settings")]
        [SerializeField] private string targetName = "Look Target";

        [Header("Events")]
        public UnityEvent OnLookEnterEvent;
        public UnityEvent OnLookStayEvent;
        public UnityEvent OnLookExitEvent;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = true;

        private bool _isBeingLookedAt = false;

        public void OnLookEnter()
        {
            _isBeingLookedAt = true;
            
            if (enableDebugLogs)
            {
                Debug.Log($"[LookTarget] 👁️ Started looking at: {targetName}");
            }

            OnLookEnterEvent?.Invoke();
        }

        public void OnLookStay()
        {
            OnLookStayEvent?.Invoke();
        }

        public void OnLookExit()
        {
            _isBeingLookedAt = false;
            
            if (enableDebugLogs)
            {
                Debug.Log($"[LookTarget] 👁️ Stopped looking at: {targetName}");
            }

            OnLookExitEvent?.Invoke();
        }

        public string GetTargetName()
        {
            return targetName;
        }

        public bool IsBeingLookedAt => _isBeingLookedAt;

        #region Debug Visualization

        private void OnDrawGizmos()
        {
            Gizmos.color = _isBeingLookedAt ? Color.yellow : Color.blue;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

        #endregion
    }
}
