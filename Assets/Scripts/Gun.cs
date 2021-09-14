using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour
    {
        public enum FireMode { Auto,Burst,Single};
        public FireMode fireMode;

        public Transform[] projexttileSpawn;
        public Projecttile projecttile;
        public float msBetweenShots = 100;
        public float muzzleVelocity = 35;
        public int burstCount;

        public Transform shell;
        public Transform shellEjection;
        Muzzleflash muzzleflash;

        bool triggerReleasedSinceLastShot;
        int shotsRemainingInBurst;

        private void Start()
        {
            muzzleflash = GetComponent<Muzzleflash>();
            shotsRemainingInBurst = burstCount;
        }

        float nextShootTime;
        void Shoot()
        {
            if (Time.time > nextShootTime)
            {
                if (fireMode==FireMode.Burst)
                {
                    if (shotsRemainingInBurst == 0)
                    {
                        return;
                    }
                    shotsRemainingInBurst--;
                }
                else if (fireMode==FireMode.Single)
                {
                    if (!triggerReleasedSinceLastShot)
                    {
                        return;
                    }
                }
                for (int i = 0; i < projexttileSpawn.Length; i++)
                {
                    nextShootTime = Time.time + msBetweenShots / 1000;
                    Projecttile newProjecttile = Instantiate(projecttile, projexttileSpawn[i].position, projexttileSpawn[i].rotation) as Projecttile;
                    newProjecttile.SetSpeed(muzzleVelocity);
                }
                Instantiate(shell, shellEjection.position, shellEjection.rotation);
                muzzleflash.Activate();
            }

        }
        public void OntriggerHold()
        {
            Shoot();
            triggerReleasedSinceLastShot = false;
        }
        public void OntriggerRelease()
        {
            triggerReleasedSinceLastShot = true;
            shotsRemainingInBurst = burstCount;
        }
    }
}