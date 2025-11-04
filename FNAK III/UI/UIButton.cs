using UnityEngine;
using UnityEngine.UI;
using FNAK.Core.Services;

namespace FNAK.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButton : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource clickAudioSource;
        [SerializeField] private AudioClip clickSound;

        private Button _button;
        private IAudioService _audioService;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _audioService = new AudioService();
        }

        public void PlayClickSound()
        {
            if (clickAudioSource != null && clickSound != null)
            {
                _audioService.PlayOneShot(clickAudioSource, clickSound);
            }
            else if (clickAudioSource != null)
            {
                _audioService.PlayOneShot(clickAudioSource);
            }
        }
    }
}

