using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public TextMeshProUGUI startOption;
        public TextMeshProUGUI resultOption;
        public TextMeshProUGUI configOption;
        public TextMeshProUGUI quitOption;
        public Color normalMenuItemColor;
        public Color focusedMenuItemColor;
        public AudioClip bgm;
        public AudioClip moveSE;
        public AudioClip selectSE;

        private BGMSingleton _bgmSingleton;
        private SESingleton _seSingleton;
        private TextMeshProUGUI[] _menuItems;
        private int _focusedMenuItemI;

        private void Start()
        {
            _bgmSingleton = BGMSingleton.GetInstance();
            _bgmSingleton.PlayBGM(bgm);
            _seSingleton = SESingleton.GetInstance();
            _menuItems = new[] { startOption, resultOption, configOption, quitOption };
            _focusedMenuItemI = 0;
            UpdateMenuItems();
        }

        private void UpdateMenuItems()
        {
            foreach (var menuItem in _menuItems)
            {
                menuItem.color = normalMenuItemColor;
                menuItem.fontStyle = FontStyles.Normal;
            }
            _menuItems[_focusedMenuItemI].color = focusedMenuItemColor;
            _menuItems[_focusedMenuItemI].fontStyle = FontStyles.Underline;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.K))
            {
                FocusPrevMenuItem();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.J))
            {
                FocusNextMenuItem();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M)))
            {
                SelectMenuItem();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftBracket)))
            {
                FocusQuitMenuItemOrSelectQuitMenuItem();
            }
        }

        private void FocusPrevMenuItem()
        {
            FocusMenuItem(_focusedMenuItemI > 0 ? _focusedMenuItemI-1 : _menuItems.Length-1);
        }

        private void FocusNextMenuItem()
        {
            FocusMenuItem(_focusedMenuItemI+1 < _menuItems.Length ? _focusedMenuItemI+1 : 0);
        }

        private void FocusMenuItem(int menuItemI)
        {
            _seSingleton.PlaySE(moveSE);
            _focusedMenuItemI = menuItemI;
            UpdateMenuItems();
        }

        private void SelectMenuItem()
        {
            _seSingleton.PlaySE(selectSE);
            var focusedMenuItem = _menuItems[_focusedMenuItemI];
            if (focusedMenuItem == configOption)
            {
                SceneManager.LoadScene("ConfigScene");
            }
            else if (focusedMenuItem == quitOption)
            {
                Quit();
            }
        }

        private void FocusQuitMenuItemOrSelectQuitMenuItem()
        {
            var focusedMenuItem = _menuItems[_focusedMenuItemI];
            if (focusedMenuItem == quitOption)
            {
                SelectMenuItem();
            }
            else
            {
                FocusMenuItem(Array.IndexOf(_menuItems, quitOption));
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
