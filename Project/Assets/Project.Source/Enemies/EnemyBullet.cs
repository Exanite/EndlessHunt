using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Enemy owner;

    private float bulletSpeed = 1f;
    private float bulletDamage = 1f;
    private float bulletLifetime = 3f;
    
    private Rigidbody2D myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        
        bulletSpeed = owner.bulletSpeed;
        bulletLifetime = owner.bulletTime;
        bulletDamage = owner.attackDamage;
    }

    private void FixedUpdate()
    {
        bulletLifetime -= Time.deltaTime;
        
        if (bulletLifetime < 0)
        {
            Destroy(gameObject);
        }

        myRigidbody.velocity = transform.right * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Enemy enemy))
        {
            return;
        }

        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player))
        {
            player.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}