using UnityEngine;

namespace FNAK.Core.Services
{
    public interface IAudioService
    {
        void PlayOneShot(AudioSource audioSource);
        void PlayOneShot(AudioSource audioSource, AudioClip clip);
    }
}
