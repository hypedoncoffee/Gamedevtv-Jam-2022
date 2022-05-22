using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float fadeModifier)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / fadeModifier;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float fadeModifier)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeModifier;
                yield return null;
            }
        }
    }
}