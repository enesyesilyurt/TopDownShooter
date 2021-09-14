using UnityEngine;
using UnityEngine.AI;
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
        public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
        {
            TakeDamage(damage);
        }

        public virtual void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
        [ContextMenu("Self Destruct")]
        protected void Die()
        {
            dead = true;
            OnDeath?.Invoke();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            GameObject.Destroy(gameObject,2);
        }
    }
}
