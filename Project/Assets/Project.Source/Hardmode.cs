using Project.Source;
using UnityEngine;

public class Hardmode : MonoBehaviour
{
    public string hardmodeEndScene = "HardmodeEnd";
    public bool shouldAutomaticallyActivate;

    public EndOfGameCutscene cutscene;
    public Transform world;

    private void Start()
    {
        if (shouldAutomaticallyActivate)
        {
            EnableHardmode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player))
        {
            EnableHardmode();
        }
    }

    private void EnableHardmode()
    {
        GameSettings.Instance.isHardmode = true;
        
        gameObject.SetActive(false);

        foreach (var enemy in world.GetComponentsInChildren<Enemy>())
        {
            enemy.onHitEnrageAmount = 2;
            enemy.aggroRadius *= 1.25f;
            enemy.deaggroRadius *= 1.5f;
            enemy.moveSpeed *= 1.5f;
            enemy.cooldown *= 0.5f;
            enemy.bulletSpeed *= 1.25f;
            enemy.attackDamage *= 2;

            var color = enemy.outOfViewSprite.color;
            color.a *= 0.30f;
            enemy.outOfViewSprite.color = color;
        }

        cutscene.nextScene = hardmodeEndScene;
    }
}