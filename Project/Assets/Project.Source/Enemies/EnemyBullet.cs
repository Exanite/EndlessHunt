using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Enemy owner;

    private float bulletSpeed = 1f;
    private Rigidbody2D myRigidbody;
    private float bulletDamage = 1f;
    private float bulletTime = 3f;
    private float timer;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        bulletSpeed = owner.bulletSpeed;
        bulletTime = owner.bulletTime;
        bulletDamage = owner.attackDamage;
        timer = bulletTime;
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
            timer = bulletTime;
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
            player.takeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}