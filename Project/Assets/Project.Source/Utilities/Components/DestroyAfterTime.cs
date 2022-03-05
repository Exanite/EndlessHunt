using UnityEngine;

namespace Project.Source.Utilities.Components
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public float lifetime = 1;

        private float timer;

        private void Start()
        {
            timer = lifetime;
        }

        private void FixedUpdate()
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}