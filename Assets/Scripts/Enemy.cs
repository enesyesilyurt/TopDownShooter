using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : LivingEntity
    {
        NavMeshAgent pathFinder;
        Transform target;
        protected override void Start()
        {
            base.Start();
            pathFinder = GetComponent<NavMeshAgent>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            StartCoroutine(UpdatePath());
        }
        IEnumerator UpdatePath()
        {
            float refreshRate = .25f;
            while (target != null)
            {
                Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                    yield return new WaitForSeconds(refreshRate);
                }
            }
        }
    }
}