using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public Player player;
    
    private float speed;
    private float damage;
    
    private Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        speed = player.projectileSpeed;
        damage = player.basicAttackDamage;
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player))
        {
            return;
        }

        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Enemy enemy))
        {
            enemy.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}