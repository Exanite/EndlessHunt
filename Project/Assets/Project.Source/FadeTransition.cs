using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    private static FadeTransition instance;

    public Image image;

    public static FadeTransition Instance
    {
        get
        {
            if (instance)
            {
                return instance;
            }

            throw new NullReferenceException("FadeTransition was null. There should be a FadeTransition object somewhere in your scene.");
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);

            return;
        }

        instance = this;
    }

    public IEnumerator FadeIn(float duration)
    {
        Debug.Log("I'm starting fading...");
        
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
    
    public IEnumerator FadeOut(float duration)
    {
        Debug.Log("I'm done fading...");
        
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