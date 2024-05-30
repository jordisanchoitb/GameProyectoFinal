using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractClasses
{
    public abstract class AEntity : MonoBehaviour
    {
        public abstract string Name { get; set; }
        public abstract int HitPoints { get; set; }
        public abstract float Speed { get; set; }
        public abstract AWeapon Weapon { get; set; }
        public abstract int Score { get; set; }

        public virtual void Start() { }
        public virtual void Update() { }
        public abstract bool IsDead();
        public abstract void Move(Vector3 vector);
        public virtual void UseWeapon() { }
    }
}
