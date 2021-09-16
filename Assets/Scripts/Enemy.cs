using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : LivingEntity
    {
        public enum State { Idle, Chasing, Attacking };
        State currentState;
        public ParticleSystem DeathEffect;

        NavMeshAgent pathFinder;
        Transform target;
        LivingEntity targetEntity;

        float attackDistanceThreshold = 1.5f;
        float timeBetweenAttacks = 1;
        float nextAttackTime;
        float myCollisionRadius;
        float targetCollisionRadius;
        float damage = 1;

        bool hasTarget;
        void Awake()
        {
            pathFinder = GetComponent<NavMeshAgent>();

            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                hasTarget = true;

                target = GameObject.FindGameObjectWithTag("Player").transform;
                targetEntity = target.GetComponent<LivingEntity>();

                myCollisionRadius = GetComponent<CapsuleCollider>().radius;
                targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
            }
        }
        protected override void Start()
        {
            base.Start(); 

            if (hasTarget)
            {
                currentState = State.Chasing;
                targetEntity.OnDeath += OnTargetDeath;
                StartCoroutine(UpdatePath());
            }
        }
        public void SetCharacteristics(float moveSpeed,int hitsToKillPlayer,float enemyHealth,Color skinColor)
        {
            pathFinder.speed = moveSpeed;
            if (hasTarget)
            {
                damage =Mathf.Ceil( targetEntity.startingHealth / hitsToKillPlayer);

            }
            startingHealth = enemyHealth;
            Material skinMaterial = GetComponent<Renderer>().material;
            skinMaterial.color = skinColor;
            //originalColour = skinMaterial.color;
        }

        public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
        {
            if (damage >= health)
            {
                transform.LookAt(target);
                DeathEffect.Play();
            }
            base.TakeHit(damage, hitPoint, hitDirection);
        }
        void OnTargetDeath()
        {
            hasTarget = false;
            currentState = State.Idle;
        }
        void Update()
        {
            if (hasTarget)
            {
                if (Time.time > nextAttackTime)
                {
                    float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                    if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                    {
                        nextAttackTime = timeBetweenAttacks + Time.time;
                        StartCoroutine(Attack());
                    }
                }
            }
        }
        IEnumerator Attack()
        {
            currentState = State.Attacking;
            pathFinder.enabled = false;

            Vector3 originalPosition = transform.position;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius);

            float attackSpeed = 3;
            float percent = 0;

            bool hasAppliedDamage = false;

            while (percent <= 1)
            {
                if(percent>=.5f && !hasAppliedDamage)
                {
                    hasAppliedDamage = true;
                    targetEntity.TakeDamage(damage);
                }
                percent += Time.deltaTime * attackSpeed;
                float interpolation = (Mathf.Pow(percent, 2) + percent) * 4;
                transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

                yield return null;
            }
            currentState = State.Chasing;
            pathFinder.enabled = true;
        }
        IEnumerator UpdatePath()
        {
            float refreshRate = .25f;
            while (hasTarget)
            {
                if (currentState == State.Chasing)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;

                    Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                    if (!dead)
                    {
                        pathFinder.SetDestination(targetPosition);
                    }
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}