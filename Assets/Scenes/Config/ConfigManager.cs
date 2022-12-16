using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Config
{
    public class ConfigManager : MonoBehaviour
    {
        public TextMeshProUGUI bgmVolumeParam;
        public TextMeshProUGUI seVolumeParam;
        public TextMeshProUGUI back;
        public Color normalOptionColor;
        public Color focusedOptionColor;
        public AudioClip bgm;
        public AudioClip moveSE;
        public AudioClip selectSE;

        private BGMSingleton _bgmSingleton;
        private SESingleton _seSingleton;
        private TextMeshProUGUI[] _configItems;
        private int _focusedConfigItemI;

        private PersistentConfig _persistentConfig;

        private void Start()
        {
            _bgmSingleton = BGMSingleton.GetInstance();
            _bgmSingleton.PlayBGM(bgm);
            _seSingleton = SESingleton.GetInstance();
            _configItems = new[] { bgmVolumeParam, seVolumeParam, back };
            _focusedConfigItemI = 0;
            _persistentConfig = PersistentConfig.LoadPersistentConfigFromPlayerPrefs();
            UpdateConfigItems();
            UpdateBGMVolume();
            UpdateSEVolume();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.K))
            {
                FocusPrevConfigItem();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.J))
            {
                FocusNextConfigItem();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.H))
            {
                DecreaseConfigItem();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.L))
            {
                IncreaseConfigItem();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M)))
            {
                SelectConfigItem();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftBracket)))
            {
                FocusBackConfigItemOrSelectBackConfigItem();
            }
        }

        private void UpdateConfigItems()
        {
            foreach (var configItem in _configItems)
            {
                configItem.color = normalOptionColor;
                configItem.fontStyle = FontStyles.Normal;
            }
            _configItems[_focusedConfigItemI].color = focusedOptionColor;
            _configItems[_focusedConfigItemI].fontStyle = FontStyles.Underline;
        }

        private void UpdateBGMVolume()
        {
            bgmVolumeParam.text = $"BGM Volume: {_persistentConfig.bgmVolume,3}";
        }

        private void UpdateSEVolume()
        {
            seVolumeParam.text = $"SE Volume:  {_persistentConfig.seVolume,3}";
        }

        private void FocusPrevConfigItem()
        {
            FocusConfigItem(_focusedConfigItemI > 0 ? _focusedConfigItemI-1 : _configItems.Length-1);
        }

        private void FocusNextConfigItem()
        {
            FocusConfigItem(_focusedConfigItemI+1 < _configItems.Length ? _focusedConfigItemI+1 : 0);
        }

        private void FocusConfigItem(int configItemI)
        {
            _seSingleton.PlaySE(moveSE);
            _focusedConfigItemI = configItemI;
            UpdateConfigItems();
        }

        private void DecreaseConfigItem()
        {
            var focusedConfigItem = _configItems[_focusedConfigItemI];
            if (focusedConfigItem == bgmVolumeParam)
            {
                _persistentConfig.DecreaseBGMVolume();
                _bgmSingleton.SetBGMVolume(_persistentConfig.bgmVolume);
                _seSingleton.PlaySE(moveSE);
                UpdateBGMVolume();
            }
            else if (focusedConfigItem == seVolumeParam)
            {
                _persistentConfig.DecreaseSEVolume();
                _seSingleton.SetSEVolume(_persistentConfig.seVolume);
                _seSingleton.PlaySE(moveSE);
                UpdateSEVolume();
            }
        }

        private void IncreaseConfigItem()
        {
            var focusedConfigItem = _configItems[_focusedConfigItemI];
            if (focusedConfigItem == bgmVolumeParam)
            {
                _persistentConfig.IncreaseBGMVolume();
                _bgmSingleton.SetBGMVolume(_persistentConfig.bgmVolume);
                _seSingleton.PlaySE(moveSE);
                UpdateBGMVolume();
            }
            else if (focusedConfigItem == seVolumeParam)
            {
                _persistentConfig.IncreaseSEVolume();
                _seSingleton.SetSEVolume(_persistentConfig.seVolume);
                _seSingleton.PlaySE(moveSE);
                UpdateSEVolume();
            }
        }

        private void SelectConfigItem()
        {
            _seSingleton.PlaySE(selectSE);
            var focusedConfigItem = _configItems[_focusedConfigItemI];
            if (focusedConfigItem == back)
            {
                PersistentConfig.SavePersistentConfigToPlayerPrefs(_persistentConfig);
                SceneManager.LoadScene("MenuScene");
            }
        }

        private void FocusBackConfigItemOrSelectBackConfigItem()
        {
            var focusedConfigItem = _configItems[_focusedConfigItemI];
            if (focusedConfigItem == back)
            {
                SelectConfigItem();
            }
            else
            {
                FocusConfigItem(Array.IndexOf(_configItems, back));
            }
        }
    }
}
