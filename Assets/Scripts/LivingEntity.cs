using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts
{
    public class LivingEntity : MonoBehaviour, IDamageable
    {
        public float startingHealth;
        protected float health;
        protected bool dead;

        public event Action OnDeath;
        protected virtual void Start()
        {
            health = startingHealth;
        }
        public void TakeHit(float damage, RaycastHit hit)
        {
            TakeDamage(damage);
        }
        [ContextMenu("Self Destruct")]
        protected void Die()
        {
            dead = true;
            OnDeath?.Invoke();
            GameObject.Destroy(gameObject);
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }
}
