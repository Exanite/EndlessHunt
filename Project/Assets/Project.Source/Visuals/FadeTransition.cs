using System;
using System.Collections;
using Project.Source;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : SingletonBehaviour<FadeTransition>
{
    public Image image;

    public bool fadeInOnStart = true;
    
    private void Start()
    {
        if (fadeInOnStart)
        {
            StartCoroutine(FadeToClear(0.3f));
        }
    }

    public IEnumerator FadeToBlack(float duration)
    {
        var timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            var alpha = Mathf.Lerp(0, 1, timer / duration);
            var color = image.color;
            color.a = alpha;
            image.color = color;

            yield return null;
        }
    }

    public IEnumerator FadeToClear(float duration)
    {
        var timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            var alpha = Mathf.Lerp(1, 0, timer / duration);
            var color = image.color;
            color.a = alpha;
            image.color = color;

            yield return null;
        }
    }
}