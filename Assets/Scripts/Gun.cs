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

        public Transform shell;
        public Transform shellEjection;

        float nextShootTime;
        public void Shoot()
        {
            if (Time.time > nextShootTime)
            {
                nextShootTime = Time.time + msBetweenShots / 1000;
                Projecttile newProjecttile = Instantiate(projecttile, muzzle.position, muzzle.rotation) as Projecttile;
                newProjecttile.SetSpeed(muzzleVelocity);

                Instantiate(shell, shellEjection.position,shellEjection.rotation);
            }

        }
    }
}