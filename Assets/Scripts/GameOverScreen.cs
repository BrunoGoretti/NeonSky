using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1f;

    private void Awake()
    {
        SetAlpha(0); 
    }

    public void FadeToBlack()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(1f));
    }

    public void FadeToClear()
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(0f));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float a)
    {
        fadeImage.color = new Color(0, 0, 0, a);
    }
}
