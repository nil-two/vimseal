using Persistent;
using UnityEngine;

namespace Singleton
{
    public class SESingleton : MonoBehaviour
    {
        private static SESingleton _instance;

        private const float SEVolumeCoefficient = 0.0025f;

        private AudioSource _audioSource;

        public static SESingleton GetInstance()
        {
            return _instance;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
                _audioSource = gameObject.GetComponent<AudioSource>();

                var persistentConfig = PersistentConfig.LoadPersistentConfigFromPlayerPrefs();
                _audioSource.volume = persistentConfig.seVolume * SEVolumeCoefficient;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Play(AudioClip seAudioClip)
        {
            _audioSource.PlayOneShot(seAudioClip);
        }

        public void SetVolume(int seVolume)
        {
            _audioSource.volume = seVolume * SEVolumeCoefficient;
        }
    }
}
