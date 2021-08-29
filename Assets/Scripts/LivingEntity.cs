using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class LivingEntity : MonoBehaviour, IDamageable
    {
        public float startingHealth;
        protected float health;
        protected bool dead;
        protected virtual void Start()
        {
            health = startingHealth;
        }
        public void TakeHit(float damage, RaycastHit hit)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
        protected void Die()
        {
            dead = true;
            GameObject.Destroy(gameObject);
        }
    }
}
