using System;
using System.Collections.Generic;
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
        
        private Rigidbody2D rb;
        private Queue<Collider2D> colliderQueue;

        private void Awake()
        {
            colliderQueue = new Queue<Collider2D>();
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            faction = owner.faction;

            speed = owner.projectileSpeed;
            damage = owner.projectileDamage;
            distanceRemaining = owner.projectileMaxDistance;
            lifetime = owner.projectileLifetime;
        }

        private void FixedUpdate()
        {
            while (colliderQueue.TryDequeue(out var collider))
            {
                OnCollide(collider);
            }
            
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
            colliderQueue.Enqueue(other);
        }

        private void OnCollide(Collider2D other)
        {
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