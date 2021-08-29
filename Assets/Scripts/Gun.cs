using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour
    {
        public Transform muzzle;
        public Projecttile projecttile;
        public float msBetweenShots = 100;
        public float muzzleVelocity = 35;

        float nextShootTime;
        public void Shoot()
        {
            if (Time.time > nextShootTime)
            {
                nextShootTime = Time.time + msBetweenShots / 1000;
                Projecttile newProjecttile = Instantiate(projecttile, muzzle.position, muzzle.rotation) as Projecttile;
                newProjecttile.SetSpeed(muzzleVelocity);
            }

        }
    }
}