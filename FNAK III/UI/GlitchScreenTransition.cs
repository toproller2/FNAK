using UnityEngine;
using FNAK.Core.Constants;

namespace FNAK.UI
{
    [RequireComponent(typeof(Animator))]
    public class GlitchScreenTransition : MonoBehaviour, IGlitchScreenTransition
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            if (_animator == null)
            {
                Debug.LogError("[GlitchScreenTransition] Animator component not found!");
            }
        }

        public void TriggerGlitchScreen()
        {
            if (_animator == null)
            {
                Debug.LogError("[GlitchScreenTransition] Animator is null. Cannot trigger animation.");
                return;
            }

            Debug.Log($"[GlitchScreenTransition] Triggering: {AnimatorConstants.TRIGGER_GLITCH_SCREEN_SAVER}");
            _animator.SetTrigger(AnimatorConstants.TRIGGER_GLITCH_SCREEN_SAVER);
        }
    }
}

