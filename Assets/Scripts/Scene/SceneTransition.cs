using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public static class SceneTransition
    {
        public const string MenuScene = "MenuScene";
        public const string ResultScene = "ResultScene";
        public const string ConfigScene = "ConfigScene";

        private const float Delay = 0.4f;

        public static IEnumerator LoadSceneWithDelay(string sceneName)
        {
            yield return new WaitForSeconds(Delay);
            SceneManager.LoadScene(sceneName);
        }
        
        public static IEnumerator QuitWithDelay()
        {
            yield return new WaitForSeconds(Delay);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}