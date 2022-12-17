using System;
using Scene;
using Singleton;
using TMPro;
using UnityEngine;

namespace Scenes.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public TextMeshProUGUI startMenu;
        public TextMeshProUGUI resultMenu;
        public TextMeshProUGUI configMenu;
        public TextMeshProUGUI quitMenu;
        public Color normalMenuItemColor;
        public Color focusedMenuItemColor;
        public AudioClip menuBGM;
        public AudioClip moveSE;
        public AudioClip selectSE;

        private BGMSingleton _bgm;
        private SESingleton _se;
        private FadeSingleton _fade;
        private LastMenuSingleton _lastMenu;
        private TextMeshProUGUI[] _menuItems;
        private int _focusedMenuItemI;
        private bool _inTransition;

        private void Start()
        {
            _lastMenu = LastMenuSingleton.GetInstance();
            _menuItems = new[] { startMenu, resultMenu, configMenu, quitMenu };
            _focusedMenuItemI = _lastMenu.Index;
            UpdateMenuItems();
            _bgm = BGMSingleton.GetInstance();
            _bgm.Play(menuBGM);
            _se = SESingleton.GetInstance();
            _fade = FadeSingleton.GetInstance();
            _fade.FadeIn();
            _inTransition = false;
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
            if (_inTransition)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.K))
            {
                FocusPrevMenuItem();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.J))
            {
                FocusNextMenuItem();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M)))
            {
                SelectMenuItem();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftBracket)))
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
            _se.Play(moveSE);
            _focusedMenuItemI = menuItemI;
            UpdateMenuItems();
        }

        private void SelectMenuItem()
        {
            _se.Play(selectSE);
            var focusedMenuItem = _menuItems[_focusedMenuItemI];
            if (focusedMenuItem == resultMenu)
            {
                _inTransition = true;
                _lastMenu.Index = _focusedMenuItemI;
                _fade.FadeOut();
                StartCoroutine(SceneTransition.LoadSceneWithDelay(SceneTransition.ResultScene));
            }
            else if (focusedMenuItem == configMenu)
            {
                _inTransition = true;
                _lastMenu.Index = _focusedMenuItemI;
                _fade.FadeOut();
                StartCoroutine(SceneTransition.LoadSceneWithDelay(SceneTransition.ConfigScene));
            }
            else if (focusedMenuItem == quitMenu)
            {
                _inTransition = true;
                _fade.FadeOut();
                StartCoroutine(SceneTransition.QuitWithDelay());
            }
        }

        private void FocusQuitMenuItemOrSelectQuitMenuItem()
        {
            var focusedMenuItem = _menuItems[_focusedMenuItemI];
            if (focusedMenuItem == quitMenu)
            {
                SelectMenuItem();
            }
            else
            {
                FocusMenuItem(Array.IndexOf(_menuItems, quitMenu));
            }
        }
    }
}
