using System;
using System.Collections;

namespace Assets.Scripts 
{
    public class AttackPattern
    {
        public float Duration { get; set; }
        public Func<IEnumerator> Pattern { get; set; }
        public Action OnEndAction { get; set; }
    }
}
