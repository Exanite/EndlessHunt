using UnityEngine;

public class Hardmode : MonoBehaviour
{
    public Transform world;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out PlayerMovement player))
        {
            EnableHardmode();

            gameObject.SetActive(false);
        }
    }

    private void EnableHardmode()
    {
        foreach (var enemy in world.GetComponentsInChildren<EnemyController>())
        {
            enemy.deaggroRadius *= 1.25f;
            enemy.moveSpeed *= 1.5f;
            enemy.cooldown *= 0.75f;

            var color = enemy.outOfViewSprite.color;
            color.a *= 0.5f;
            enemy.outOfViewSprite.color = color;
        }
    }
}