﻿using Persistent;
using Scene;
using Singleton;
using TMPro;
using UnityEngine;

namespace Scenes.Result
{
    public class ResultManager : MonoBehaviour
    {
        public TextMeshProUGUI archiveContent;
        public AudioClip resultBGM;
        public AudioClip selectSE;
        
        private BGMSingleton _bgm;
        private SESingleton _se;
        private FadeSingleton _fade;
        private PersistentArchive _archive;
        private bool _inTransition;

        private void Start()
        {
            _archive = PersistentArchive.LoadPersistentArchiveFromPlayerPrefs();
            UpdateArchiveText();
            _bgm = BGMSingleton.GetInstance();
            _bgm.Play(resultBGM, 2f);
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
            if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M)))
            {
                BackToMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftBracket)))
            {
                BackToMenu();
            }
        }

        private void BackToMenu()
        {
            _inTransition = true;
            _se.Play(selectSE);
            _fade.FadeOut();
            StartCoroutine(SceneTransition.LoadSceneWithDelay(SceneTransition.MenuScene));
        }

        private void UpdateArchiveText()
        {
            const string passedMark = "*";
            const string unPassedMark = " ";
            archiveContent.text = string.Join("\n",
                $"[{(_archive.firstCoursePassed ? passedMark : unPassedMark)}] 1st course ---",
                $"[{(_archive.secondCoursePassed ? passedMark : unPassedMark)}] 2nd course ---",
                "");
        }
    }
}