using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public TMP_Text text;
    public Image fill;
    public Player player;

    public float smoothTime = 0.3f;
    private float smoothVelocity;
    
    private void Update()
    {
        if (!player)
        {
            UpdateDisplay(0, 0);

            return;
        }

        UpdateDisplay(player.health, player.maxHealth);
    }

    private void UpdateDisplay(float health, float maxHealth)
    {
        if (health < 0)
        {
            health = 0;
        }

        health = Mathf.Ceil(health - 0.1f);
        maxHealth = Mathf.Ceil(maxHealth - 0.1f);

        text.text = $"{health}/{maxHealth}";

        var targetHealthRatio = health / maxHealth;
        
        var anchorMax = fill.rectTransform.anchorMax;
        anchorMax.x = Mathf.SmoothDamp(anchorMax.x, targetHealthRatio, ref smoothVelocity, smoothTime);
        fill.rectTransform.anchorMax = anchorMax;
    }
}