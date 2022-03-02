using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public EnemyController myController;

    private float bulletSpeed = 1f;
    private Rigidbody2D myRigidbody;
    private float bulletDamage = 1f;
    private float bulletTime = 3f;
    private float timer;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        bulletSpeed = myController.bulletSpeed;
        bulletTime = myController.bulletTime;
        bulletDamage = myController.attackDamage;
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
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out EnemyController enemyController))
        {
            return;
        }

        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out PlayerMovement player))
        {
            player.takeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}