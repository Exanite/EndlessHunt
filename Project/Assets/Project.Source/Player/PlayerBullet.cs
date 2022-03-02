using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float bulletSpeed = 1f;
    private Rigidbody2D myRigidbody;
    public GameObject playerObject;
    private Player player;
    private float bulletDamage = 1f;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = playerObject.GetComponent<Player>();
        bulletDamage = player.getBasicAttackDamage();
    }

    private void FixedUpdate()
    {
        myRigidbody.velocity = transform.right * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Player player))
        {
            return;
        }

        if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out Enemy enemy))
        {
            enemy.takeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}