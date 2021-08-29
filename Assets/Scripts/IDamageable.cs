using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IDamageable
    {
        void TakeHit(float damage, RaycastHit hit);
    }
}