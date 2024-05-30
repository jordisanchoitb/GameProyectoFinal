using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractClasses
{
    public abstract class AEnemy : AEntity
    {
        public override void Start() { }
        public override void Update() { }
        public virtual void Behaviour() { }
    }
}
