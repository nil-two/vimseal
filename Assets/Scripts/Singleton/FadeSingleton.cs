using UnityEngine;

namespace Singleton
{
    public class FadeSingleton : MonoBehaviour
    {
        private static FadeSingleton _instance;

        private static readonly int FadeInTrigger = Animator.StringToHash("fadeIn");
        private static readonly int FadeOutTrigger = Animator.StringToHash("fadeOut");

        private Animator _animator;

        public static FadeSingleton GetInstance()
        {
            return _instance;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
                _animator = gameObject.GetComponent<Animator>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void FadeIn()
        {
            _animator.SetTrigger(FadeInTrigger);
        }

        public void FadeOut()
        {
            _animator.SetTrigger(FadeOutTrigger);
        }
    }
}
