using TMPro;
using UnityEngine;

public class PressStartToPlay : MonoBehaviour
{
    public AnimationCurve opacityCurve;

    [Header("Runtime")]
    public TMP_Text text;
    
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        var opacity = opacityCurve.Evaluate(Time.time);
        var color = text.color;
        color.a = opacity;
        text.color = color;
    }
}