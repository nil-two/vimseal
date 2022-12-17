using Persistent;
using UnityEngine;

namespace Singleton
{
    public class BGMSingleton : MonoBehaviour
    {
        private static BGMSingleton _instance;

        private const float BGMVolumeCoefficient = 0.0025f;

        private AudioSource _audioSource;

        public static BGMSingleton GetInstance()
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
                _audioSource.volume = persistentConfig.bgmVolume * BGMVolumeCoefficient;
                _audioSource.loop = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Play(AudioClip bgmAudioClip, float time = 0f)
        {
            if (bgmAudioClip == _audioSource.clip)
            {
                return;
            }
            _audioSource.clip = bgmAudioClip;
            _audioSource.time = time;
            _audioSource.Play();
        }

        public void SetVolume(int bgmVolume)
        {
            _audioSource.volume = bgmVolume * BGMVolumeCoefficient;
        }
    }
}
