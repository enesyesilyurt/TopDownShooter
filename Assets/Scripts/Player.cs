using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(GunController))]
    public class Player : LivingEntity
    {
        public float MoveSpeed = 5;
        Camera viewCamera;
        PlayerController controller;
        GunController gunController;
        protected override void Start()
        {
            base.Start();
            viewCamera = Camera.main;
            controller = GetComponent<PlayerController>();
            gunController = GetComponent<GunController>();
        }

        void Update()
        {
            Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Vector3 moveVelocity = moveInput.normalized * MoveSpeed;
            controller.Move(moveVelocity);

            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                controller.LookAt(point);
            }

            if (Input.GetMouseButton(0))
            {
                gunController.Shoot();
            }
        }
    }
}
