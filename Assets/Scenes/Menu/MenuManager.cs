using TMPro;
using UnityEngine;

namespace Scenes.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public TextMeshProUGUI startOption;
        public TextMeshProUGUI resultOption;
        public TextMeshProUGUI configOption;
        public TextMeshProUGUI quitOption;
        public AudioSource seAudioSource;
        public Color normalOptionColor;
        public Color focusedOptionColor;
        public AudioClip moveSe;
        public AudioClip selectSe;

        private TextMeshProUGUI[] _options;
        private int _focusedOptionI;

        private void Start()
        {
            _options = new[] { startOption, resultOption, configOption, quitOption };
            _focusedOptionI = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.K))
            {
                FocusPrevOption();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.J))
            {
                FocusNextOption();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                SelectOption();
            }
        }

        private void FocusPrevOption()
        {
            FocusOption(_focusedOptionI > 0 ? _focusedOptionI-1 : _options.Length-1);
        }

        private void FocusNextOption()
        {
            FocusOption(_focusedOptionI+1 < _options.Length ? _focusedOptionI+1 : 0);
        }

        private void FocusOption(int optionI)
        {
            seAudioSource.PlayOneShot(moveSe);
            foreach (var option in _options)
            {
                option.color = normalOptionColor;
                option.fontStyle = FontStyles.Normal;
            }
            _options[optionI].color = focusedOptionColor;
            _options[optionI].fontStyle = FontStyles.Underline;
            _focusedOptionI = optionI;
        }

        private void SelectOption()
        {
            seAudioSource.PlayOneShot(selectSe);
            var focusedOption = _options[_focusedOptionI];
            if (focusedOption == startOption)
            {
            }
            else if (focusedOption == resultOption)
            {
            }
            else if (focusedOption == configOption)
            {
            }
            else if (focusedOption == quitOption)
            {
                Quit();
            }
        }

        private static void Quit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}
