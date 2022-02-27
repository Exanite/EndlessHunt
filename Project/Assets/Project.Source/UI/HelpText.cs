using TMPro;
using UnityEngine;

public class HelpText : MonoBehaviour
{
    public PlayerMovement player;
    public TMP_Text text;

    public float fadeStartDistance = 5;
    public float fadeEndDistance = 8;

    private void Update()
    {
        var playerDistance = (player.transform.position - transform.position).magnitude;
        var opacity = (playerDistance - fadeStartDistance) / (fadeEndDistance - fadeStartDistance);

        opacity = Mathf.Clamp01(opacity);
        opacity = 1 - opacity;
        
        var color = text.color;
        color.a = opacity;
        text.color = color;
    }
}