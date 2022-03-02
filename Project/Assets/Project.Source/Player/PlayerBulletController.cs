using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    public float bulletSpeed = 1f;
    private Rigidbody2D myRigidbody;
    public GameObject playerObject;
    private PlayerMovement player;
    private float bulletDamage = 1f;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = playerObject.GetComponent<PlayerMovement>();
        bulletDamage = player.getBasicAttackDamage();
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = transform.right * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out PlayerMovement player))
        {
            return;
        }

        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.takeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}