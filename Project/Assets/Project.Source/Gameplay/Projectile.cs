using UnityEngine;

namespace Project.Source.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        [Header("Dependencies")]
        public UnitOffensiveStats owner;

        [Header("Runtime")]
        public Faction faction;

        public float damage;
        public float speed;
        public float distanceRemaining;
        public float lifetime;

        private bool hasInitialized;

        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            faction = owner.faction;

            speed = owner.projectileSpeed;
            damage = owner.projectileDamage;
            distanceRemaining = owner.projectileMaxDistance;
            lifetime = owner.projectileLifetime;

            hasInitialized = true;
        }

        private void FixedUpdate()
        {
            rb.velocity = transform.right * speed;

            var distanceTraveled = rb.velocity.magnitude * Time.deltaTime;
            distanceRemaining -= distanceTraveled;

            if (distanceRemaining < 0)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!hasInitialized)
            {
                return;
            }

            if (other.attachedRigidbody)
            {
                if (faction == Faction.Enemy && other.attachedRigidbody.TryGetComponent(out Player player))
                {
                    player.TakeDamage(damage);
                }
                else if (faction == Faction.Player && other.attachedRigidbody.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(damage);
                }
                else
                {
                    return;
                }
            }

            Despawn();
        }

        public void Despawn()
        {
            Destroy(gameObject);
        }
    }
}