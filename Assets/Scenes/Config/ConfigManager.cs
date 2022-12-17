using System;
using Persistent;
using Singleton;
using TMPro;
using UnityEngine;
using Scene;

namespace Scenes.Config
{
    public class ConfigManager : MonoBehaviour
    {
        public TextMeshProUGUI bgmVolumeParam;
        public TextMeshProUGUI seVolumeParam;
        public TextMeshProUGUI backMenu;
        public Color normalOptionColor;
        public Color focusedOptionColor;
        public AudioClip configBGM;
        public AudioClip moveSE;
        public AudioClip selectSE;

        private PersistentConfig _persistentConfig;
        private BGMSingleton _bgm;
        private SESingleton _se;
        private FadeSingleton _fade;
        private TextMeshProUGUI[] _configItems;
        private int _focusedConfigItemI;
        private bool _inTransition;

        private void Start()
        {
            _persistentConfig = PersistentConfig.LoadPersistentConfigFromPlayerPrefs();
            _configItems = new[] { bgmVolumeParam, seVolumeParam, backMenu };
            _bgm = BGMSingleton.GetInstance();
            _bgm.Play(configBGM);
            _se = SESingleton.GetInstance();
            _fade = FadeSingleton.GetInstance();
            _fade.FadeIn();
            _inTransition = false;
            UpdateConfigItems();
            UpdateBGMVolume();
            UpdateSEVolume();
        }

        private void Update()
        {
            if (_inTransition)
            {
                return;
            }
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
            else if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M)))
            {
                SelectConfigItem();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftBracket)))
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
            _se.Play(moveSE);
            _focusedConfigItemI = configItemI;
            UpdateConfigItems();
        }

        private void DecreaseConfigItem()
        {
            var focusedConfigItem = _configItems[_focusedConfigItemI];
            if (focusedConfigItem == bgmVolumeParam)
            {
                _persistentConfig.DecreaseBGMVolume();
                _bgm.SetVolume(_persistentConfig.bgmVolume);
                _se.Play(moveSE);
                UpdateBGMVolume();
            }
            else if (focusedConfigItem == seVolumeParam)
            {
                _persistentConfig.DecreaseSEVolume();
                _se.SetVolume(_persistentConfig.seVolume);
                _se.Play(moveSE);
                UpdateSEVolume();
            }
        }

        private void IncreaseConfigItem()
        {
            var focusedConfigItem = _configItems[_focusedConfigItemI];
            if (focusedConfigItem == bgmVolumeParam)
            {
                _persistentConfig.IncreaseBGMVolume();
                _bgm.SetVolume(_persistentConfig.bgmVolume);
                _se.Play(moveSE);
                UpdateBGMVolume();
            }
            else if (focusedConfigItem == seVolumeParam)
            {
                _persistentConfig.IncreaseSEVolume();
                _se.SetVolume(_persistentConfig.seVolume);
                _se.Play(moveSE);
                UpdateSEVolume();
            }
        }

        private void SelectConfigItem()
        {
            _se.Play(selectSE);
            var focusedConfigItem = _configItems[_focusedConfigItemI];
            if (focusedConfigItem == backMenu)
            {
                _inTransition = true;
                _fade.FadeOut();
                PersistentConfig.SavePersistentConfigToPlayerPrefs(_persistentConfig);
                StartCoroutine(SceneTransition.LoadSceneWithDelay(SceneTransition.MenuScene));
            }
        }

        private void FocusBackConfigItemOrSelectBackConfigItem()
        {
            var focusedConfigItem = _configItems[_focusedConfigItemI];
            if (focusedConfigItem == backMenu)
            {
                SelectConfigItem();
            }
            else
            {
                FocusConfigItem(Array.IndexOf(_configItems, backMenu));
            }
        }
    }
}
