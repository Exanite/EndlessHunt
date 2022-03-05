using System.Collections.Generic;
using UnityEngine;

public class EntitySpriteFlip : MonoBehaviour
{
    [Header("Assign one")]
    public Enemy enemy;
    public Player player;
    public Slime slime;

    [Header("Configuration")]
    public List<SpriteRenderer> spritesToFlip;
    public List<Transform> transformsToFlip;

    [Header("Runtime")]
    public bool isFacingRight;
    
    private void Update()
    {
        if (enemy)
        {
            UpdateEnemy();
        }
        else if (player)
        {
            UpdatePlayer();
        }
        else if(slime)
        {
            UpdateSlime();
        }
    }

    private void UpdateSlime()
    {
        if (slime.playerTarget)
        {
            var offset = slime.playerTarget.transform.position - slime.transform.position;

            SetFlipDirection(offset.x > 0);
        }
    }

    private void UpdateEnemy()
    {
        if (enemy.target)
        {
            var offset = enemy.target.transform.position - enemy.transform.position;

            SetFlipDirection(offset.x > 0);
        }
    }

    private void UpdatePlayer()
    {
        SetFlipDirection(player.rotationTransform.right.x > 0);
    }

    private void SetFlipDirection(bool isFacingRight)
    {
        this.isFacingRight = isFacingRight;
        
        foreach (var spriteRenderer in spritesToFlip)
        {
            spriteRenderer.flipX = !isFacingRight;
        }

        foreach (var transform in transformsToFlip)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1 : -1);
            transform.localScale = scale;
        }
    }
}