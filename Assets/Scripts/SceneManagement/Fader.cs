using System.Collections;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveCoroutine;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public Coroutine Fade(float target, float time)
        {
            CancelCurrentCoroutine();
            currentActiveCoroutine = StartCoroutine(FadeRoutine(target, time));
            return currentActiveCoroutine;
        }
        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }
        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }
        public void FadeOutImmmediate()
        {
            canvasGroup.alpha = 1;
        }
        private void CancelCurrentCoroutine()
        {
            if (currentActiveCoroutine != null)
            {
                StopCoroutine(currentActiveCoroutine);
            }
        }
        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}
