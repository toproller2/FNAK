using UnityEngine;

namespace FNAK.Core.Services
{
    public class AudioService : IAudioService
    {
        public void PlayOneShot(AudioSource audioSource)
        {
            if (audioSource == null)
            {
                Debug.LogWarning("[AudioService] AudioSource is null. Cannot play sound.");
                return;
            }

            if (audioSource.clip == null)
            {
                Debug.LogWarning("[AudioService] AudioSource has no clip assigned.");
                return;
            }

            audioSource.PlayOneShot(audioSource.clip);
        }

        public void PlayOneShot(AudioSource audioSource, AudioClip clip)
        {
            if (audioSource == null)
            {
                Debug.LogWarning("[AudioService] AudioSource is null. Cannot play sound.");
                return;
            }

            if (clip == null)
            {
                Debug.LogWarning("[AudioService] AudioClip is null. Cannot play sound.");
                return;
            }

            audioSource.PlayOneShot(clip);
        }
    }
}
