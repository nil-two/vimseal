using System;
using System.Collections.Generic;
using System.Linq;
using Singleton;
using UnityEngine;
using Contents;
using Persistent;
using Scene;
using TMPro;

namespace Scenes.Game
{
    public class GameManager : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI text;
        public TextMeshProUGUI cursor;
        public TextMeshProUGUI checkList;
        public TextMeshProUGUI guide;
        public Color normalColor;
        public Color cursorFgColor;
        public Color checkListItemPassedColor;
        public AudioClip gameBGM;
        public AudioClip checkSE;
        public AudioClip selectSE;

        private Course _course;
        private int _currentSectionI;
        private char[][] _buffer;
        private int _cursorX;
        private int _cursorY;
        private BGMSingleton _bgm;
        private SESingleton _se;
        private FadeSingleton _fade;
        private bool _inTransition;

        private void Start()
        {
            _course = Course.BuildDefaultCourse();
            _currentSectionI = 0;
            _cursorX = 1;
            _cursorY = 1;
            UpdateSection();
            _bgm = BGMSingleton.GetInstance();
            _bgm.Play(gameBGM);
            _se = SESingleton.GetInstance();
            _fade = FadeSingleton.GetInstance();
            _fade.FadeIn();
            _inTransition = false;
        }

        private void Update()
        {
            if (_inTransition)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.M)))
            {
                NextSection();
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                MoveCursorLeft();
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                MoveCursorBottom();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                MoveCursorTop();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                MoveCursorRight();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                MoveCursorLead();
            }
            else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Alpha4))
            {
                MoveCursorTrail();
            }
        }

        private void MoveCursorLeft()
        {
            var section = _course.Sections[_currentSectionI];
            if (section.SectionId == Section.SectionIdOfBasicMoving && !section.CheckListVault.MovedWithH)
            {
                _se.Play(checkSE);
                section.CheckListVault.MovedWithH = true;
                UpdateCheckList();
            }
            if (_buffer.Length < 1 || _buffer.Length == 1 && _buffer[0].Length == 0 || _cursorY < 1 || _buffer.Length < _cursorY)
            {
                return;
            }
            _cursorX = Math.Clamp(_cursorX - 1, 1, _buffer[_cursorY - 1].Length);
            UpdateEditor();
        }

        private void MoveCursorBottom()
        {
            var section = _course.Sections[_currentSectionI];
            if (section.SectionId == Section.SectionIdOfBasicMoving && !section.CheckListVault.MovedWithJ)
            {
                _se.Play(checkSE);
                section.CheckListVault.MovedWithJ = true;
                UpdateCheckList();
            }
            if (_buffer.Length < 1 || (_buffer.Length == 1 && _buffer[0].Length == 0))
            {
                return;
            }
            _cursorY = Math.Clamp(_cursorY + 1, 1, _buffer.Length);
            _cursorX = Math.Clamp(_cursorX, 1, _buffer[_cursorY - 1].Length);
            UpdateEditor();
        }

        private void MoveCursorTop()
        {
            var section = _course.Sections[_currentSectionI];
            if (section.SectionId == Section.SectionIdOfBasicMoving && !section.CheckListVault.MovedWithK)
            {
                _se.Play(checkSE);
                section.CheckListVault.MovedWithK = true;
                UpdateCheckList();
            }
            if (_buffer.Length < 1 || (_buffer.Length == 1 && _buffer[0].Length == 0))
            {
                return;
            }
            _cursorY = Math.Clamp(_cursorY - 1, 1, _buffer.Length);
            _cursorX = Math.Clamp(_cursorX, 1, _buffer[_cursorY - 1].Length);
            UpdateEditor();
        }

        private void MoveCursorRight()
        {
            var section = _course.Sections[_currentSectionI];
            if (section.SectionId == Section.SectionIdOfBasicMoving && !section.CheckListVault.MovedWithL)
            {
                _se.Play(checkSE);
                section.CheckListVault.MovedWithL = true;
                UpdateCheckList();
            }
            if (_buffer.Length < 1 || (_buffer.Length == 1 && _buffer[0].Length == 0) || _cursorY < 1 || _buffer.Length < _cursorY)
            {
                return;
            }
            _cursorX = Math.Clamp(_cursorX + 1, 1, _buffer[_cursorY - 1].Length);
            UpdateEditor();
        }

        private void MoveCursorLead()
        {
            if (_buffer.Length < 1 || (_buffer.Length == 1 && _buffer[0].Length == 0) || _cursorY < 1 || _buffer.Length < _cursorY)
            {
                return;
            }
            _cursorX = 1;
            UpdateEditor();
        }

        private void MoveCursorTrail()
        {
            if (_buffer.Length < 1 || (_buffer.Length == 1 && _buffer[0].Length == 0) || _cursorY < 1 || _buffer.Length < _cursorY)
            {
                return;
            }
            _cursorX = _buffer[_cursorY - 1].Length;
            UpdateEditor();
        }

        private void NextSection()
        {
            var section = _course.Sections[_currentSectionI];
            if (!section.CheckList.All((item) => item.Passed))
            {
                return;
            }
            if (_currentSectionI+1 < _course.Sections.Length)
            {
                _se.Play(selectSE);
                _currentSectionI++;
                UpdateSection();
            }
            else
            {
                _inTransition = true;
                _se.Play(selectSE);
                _fade.FadeOut();
                PersistentArchive.RecordBasicCoursePassed();
                StartCoroutine(SceneTransition.LoadSceneWithDelay(SceneTransition.MenuScene));
            }
        }

        private void UpdateSection()
        {
            var section = _course.Sections[_currentSectionI];
            _cursorX = 1;
            _cursorY = 1;
            _buffer = TranslateTextToBuffer(section.Text);
            title.text = TranslateCheckListItemsToTitle(section.Title, section.CheckList, ColorUtility.ToHtmlStringRGB(checkListItemPassedColor));
            text.text = TranslateBufferToEditorText(_buffer, _cursorX, _cursorY, ColorUtility.ToHtmlStringRGB(cursorFgColor));
            cursor.text = TranslateBufferToEditorCursor(_buffer, _cursorX, _cursorY, ColorUtility.ToHtmlStringRGB(normalColor));
            checkList.text = TranslateCheckListItemsToCheckList(section.CheckList, ColorUtility.ToHtmlStringRGB(checkListItemPassedColor));
            guide.text = section.Guide;
        }

        private void UpdateEditor()
        {
            text.text = TranslateBufferToEditorText(_buffer, _cursorX, _cursorY, ColorUtility.ToHtmlStringRGB(cursorFgColor));
            cursor.text = TranslateBufferToEditorCursor(_buffer, _cursorX, _cursorY, ColorUtility.ToHtmlStringRGB(normalColor));
        }

        private void UpdateCheckList()
        {
            var section = _course.Sections[_currentSectionI];
            section.UpdateCheckListFromCheckListVault();
            title.text = TranslateCheckListItemsToTitle(section.Title, section.CheckList, ColorUtility.ToHtmlStringRGB(checkListItemPassedColor));
            checkList.text = TranslateCheckListItemsToCheckList(section.CheckList, ColorUtility.ToHtmlStringRGB(checkListItemPassedColor));
        }

        private static string TranslateCheckListItemsToTitle(string title, IReadOnlyCollection<CheckListItem> checkListItems, string passedColorCode)
        {
            if (checkListItems.Count >= 1 && checkListItems.All((item) => item.Passed))
            {
                return $"<color=#{passedColorCode}>{title}</color>";
            }
            else
            {
                return title;
            }
        }

        private static char[][] TranslateTextToBuffer(string text)
        {
            var lines = text.Split("\n");
            var charsTable = lines.Select((line) => line.ToCharArray().ToArray()).ToArray();
            return charsTable;
        }

        private static string TranslateBufferToEditorText(IReadOnlyList<char[]> buffer, int cursorX, int cursorY, string cursorFgColorCode)
        {
            if (buffer.Count == 0 || cursorY < 1 || buffer.Count < cursorY || cursorX < 1 || buffer[cursorY - 1].Length < cursorX)
            {
                return string.Join("\n", buffer.Select((chars) => string.Join("", chars)));
            }
            var joinedCharsTable = buffer.Select((_) => "").ToArray();
            for (var i = 0; i < buffer.Count; i++)
            {
                if (i == cursorY - 1)
                {
                    var targetChars = buffer[cursorY - 1].Select((ch) => ch.ToString()).ToList();
                    targetChars.Insert(cursorX - 1, $"<color=#{cursorFgColorCode}>");
                    targetChars.Insert(cursorX + 1, "</color>");
                    joinedCharsTable[i] = string.Join("", targetChars);
                }
                else
                {
                    joinedCharsTable[i] = string.Join("", buffer[i]);
                }
            }
            return string.Join("\n", joinedCharsTable);
        }

        private static string TranslateBufferToEditorCursor(IReadOnlyList<char[]> buffer, int cursorX, int cursorY, string cursorBgColorCode)
        {
            if (buffer.Count == 0 || cursorY < 1 || buffer.Count < cursorY || cursorX < 1 || buffer[cursorY - 1].Length < cursorX)
            {
                return "";
            }
            var joinedCharsTable = buffer.Select((_) => "").ToArray();
            for (var i = 0; i < buffer.Count; i++)
            {
                if (i == cursorY - 1)
                {
                    var targetChars = buffer[i].Select((_) => " ").ToList();
                    targetChars[cursorX - 1] = "_";
                    targetChars.Insert(cursorX - 1, $"<mark=#{cursorBgColorCode} padding=\"5,5,0,0\">");
                    targetChars.Insert(cursorX + 1, "</mark>");
                    joinedCharsTable[i] = string.Join("", targetChars);
                }
                else
                {
                    joinedCharsTable[i] = "";
                }
            }
            return string.Join("\n", joinedCharsTable);
        }

        private static string TranslateCheckListItemsToCheckList(IReadOnlyList<CheckListItem> checkListItems, string passedColorCode)
        {
            var lines = checkListItems.Select((_) => "").ToArray();
            for (var i = 0; i < checkListItems.Count; i++)
            {
                var checkListItem = checkListItems[i];
                if (checkListItem.Passed)
                {
                    lines[i] = $"<color=#{passedColorCode}>[*] {checkListItem.Name}</color>";
                }
                else
                {
                    lines[i] = $"[ ] {checkListItem.Name}";
                }
            }
            return string.Join("\n", lines);
        }
    }
}
