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

        NavMeshAgent pathFinder;
        Transform target;

        float attackDistanceThreshold = 1.5f;
        float timeBetweenAttacks = 1;
        float nextAttackTime;
        float myCollisionRadius;
        float targetCollisionRadius;
        protected override void Start()
        {
            base.Start();
            pathFinder = GetComponent<NavMeshAgent>();

            currentState = State.Chasing;
            target = GameObject.FindGameObjectWithTag("Player").transform;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }
        void Update()
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold+myCollisionRadius+targetCollisionRadius, 2))
                {
                    nextAttackTime = timeBetweenAttacks + Time.time;
                    StartCoroutine(Attack());
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
            while (percent <= 1)
            {
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
            while (target != null)
            {
                if (currentState == State.Chasing)
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;

                    Vector3 targetPosition = target.position-dirToTarget*(myCollisionRadius+targetCollisionRadius+attackDistanceThreshold/2);
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