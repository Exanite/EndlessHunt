using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public TMP_Text text;
    public Image fill;
    public PlayerMovement player;

    private void Update()
    {
        if (!player)
        {
            SetDisplay(0, 0);

            return;
        }

        SetDisplay(player.health, player.maxHealth);
    }

    private void SetDisplay(float health, float maxHealth)
    {
        if (health < 0)
        {
            health = 0;
        }

        text.text = $"{health:N0}/{maxHealth:N0}";
        var anchorMax = fill.rectTransform.anchorMax;
        anchorMax.x = health / maxHealth;
        fill.rectTransform.anchorMax = anchorMax;
    }
}