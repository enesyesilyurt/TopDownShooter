﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Projecttile : MonoBehaviour
    {
        public LayerMask collisionMask;
        float speed = 10;
        float damage = 1;

        float lifetime = 3;
        float skinWidth = .1f;
        private void Start()
        {
            Destroy(gameObject, lifetime);

            Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
            if (initialCollisions.Length > 0)
            {
                OnHitObject(initialCollisions[0],transform.position);
            }
        }
        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }
        void Update()
        {
            float moveDistance = speed * Time.deltaTime;
            CheckCollisions(moveDistance);
            transform.Translate(speed * Time.deltaTime * Vector3.forward);
        }
        void CheckCollisions(float moveDistance)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, moveDistance+skinWidth, collisionMask, QueryTriggerInteraction.Collide))
            {
                OnHitObject(hit.collider,hit.point);
            }
        }
        
        void OnHitObject(Collider collider,Vector3 hitPoint)
        {
            IDamageable damageableObject = collider.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                damageableObject.TakeHit(damage,hitPoint,transform.forward);
            }
            GameObject.Destroy(gameObject);
        }
    }
}