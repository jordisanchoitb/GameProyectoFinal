using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractClasses
{
    public abstract class AWeapon : MonoBehaviour
    {
        public abstract int Damage { get; set; }
        
        public abstract void Action(Vector3 position, Quaternion rotation);
        public abstract void Action(Vector3 position, Quaternion rotation, string instantiator);

    }
}
