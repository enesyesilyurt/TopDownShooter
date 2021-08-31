﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Projecttile : MonoBehaviour
    {
        public LayerMask collisionMask;
        float speed = 10;
        float damage = 1;
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
            if(Physics.Raycast(ray,out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
            {
                OnHitObject(hit);
            }
        }
        void OnHitObject(RaycastHit hit)
        {
            IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                damageableObject.TakeHit(damage, hit);
            }
            GameObject.Destroy(gameObject);
        }
    }
}