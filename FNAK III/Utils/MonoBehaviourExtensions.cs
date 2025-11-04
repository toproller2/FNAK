using System.Collections;
using UnityEngine;

namespace FNAK.Utils
{
    public static class MonoBehaviourExtensions
    {
        public static Coroutine ExecuteAfterDelay(this MonoBehaviour mono, System.Action action, float delay)
        {
            return mono.StartCoroutine(ExecuteAfterDelayCoroutine(action, delay));
        }

        private static IEnumerator ExecuteAfterDelayCoroutine(System.Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        public static Coroutine ExecuteNextFrame(this MonoBehaviour mono, System.Action action)
        {
            return mono.StartCoroutine(ExecuteNextFrameCoroutine(action));
        }

        private static IEnumerator ExecuteNextFrameCoroutine(System.Action action)
        {
            yield return null;
            action?.Invoke();
        }
    }
}

