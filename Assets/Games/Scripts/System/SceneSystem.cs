using System.Collections;
using DG.Tweening;
using TomGustin.GameDesignPattern;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GuraGames.GameSystem
{
    public class SceneSystem : Singleton<SceneSystem>
    {
        [SerializeField] private CanvasGroup fadeCanvas;

        private bool isBusy;
        private bool readyToLoad;

        public const float DURATION_FADE_IN = 0.5f;
        public const float DURATION_FADE_OUT = 0.5f;

        private void Awake()
        {
            OnInitialize();
        }

        public static void ReadytoLoad() { if (Instance) Instance.readyToLoad = true; }

        public static void LoadScene(string scene_name)
        {
            Instance?.LoadScene_Internal(scene_name);
        }

        private void LoadScene_Internal(string scene_name)
        {
            if (isBusy) return;
            StartCoroutine(DOLoadScene(scene_name));
        }

        private IEnumerator DOLoadScene(string scene_name)
        {
            Tween fade_out = fadeCanvas.DOFade(1f, DURATION_FADE_OUT).SetEase(Ease.Linear);
            isBusy = true;
            readyToLoad = false;
            yield return fade_out.WaitForCompletion();
            AsyncOperation async = SceneManager.LoadSceneAsync(scene_name);
            yield return new WaitUntil(() => async.isDone);
            yield return new WaitUntil(() => readyToLoad);

            Tween fade_in = fadeCanvas.DOFade(0f, DURATION_FADE_OUT).SetEase(Ease.Linear);
            yield return fade_in.WaitForCompletion();

            isBusy = false;
        }
    }
}