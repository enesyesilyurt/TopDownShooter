﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class GunController : MonoBehaviour
    {
        public Transform weaponHold;
        public Gun startingGun;
        Gun equippedGun;
        private void Start()
        {
            if (startingGun != null)
            {
                EquipGun(startingGun);
            }
        }
        public void EquipGun(Gun gunToEquip)
        {
            if (equippedGun != null)
            {
                Destroy(equippedGun.gameObject);
            }
            equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
            equippedGun.transform.parent = weaponHold;
        }

        public void OnTriggerHold()
        {
            if (equippedGun != null)
            {
                equippedGun.OntriggerHold();
            }
        }
        public void OnTriggerRelease()
        {
            if (equippedGun != null)
            {
                equippedGun.OntriggerRelease();
            }
        }
        public float GunHeight
        {
            get { return weaponHold.position.y; }
        }
    }
}