using System;
using UnityEngine;

namespace Project.Source.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        // Todo Add base class / interface
        [Header("Dependencies")]
        public Enemy enemyOwner;
        public Player playerOwner;

        [Header("Runtime")]
        public Faction faction;

        public float speed;
        public float damage;
        public float maxDistance;
        public float lifetime;

        private Rigidbody2D rb;

        private bool hasInitialized;
        
        private void Start()
        {
            hasInitialized = true;
            
            rb = GetComponent<Rigidbody2D>();

            if (enemyOwner)
            {
                faction = Faction.Enemy;

                speed = enemyOwner.bulletSpeed;
                damage = enemyOwner.attackDamage;
                maxDistance = enemyOwner.projectileMaxDistance;
                lifetime = enemyOwner.projectileLifetime;
            }
            else
            {
                faction = Faction.Player;
                
                speed = playerOwner.projectileSpeed;
                damage = playerOwner.basicAttackDamage;
                maxDistance = playerOwner.projectileMaxDistance;
                lifetime = playerOwner.projectileLifetime;
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = transform.right * speed;
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

            Destroy(gameObject);
        }
    }
}