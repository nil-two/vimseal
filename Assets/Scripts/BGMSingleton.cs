using UnityEngine;

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip bgmAudioClip)
    {
        if (bgmAudioClip == _audioSource.clip)
        {
            return;
        }
        _audioSource.clip = bgmAudioClip;
        _audioSource.Play();
    }

    public void SetBGMVolume(int bgmVolume)
    {
        _audioSource.volume = bgmVolume * BGMVolumeCoefficient;
    }
}