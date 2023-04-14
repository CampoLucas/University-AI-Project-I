using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class Flee : Seek
    {
        public Flee(Transform origin, Transform target) : base(origin, target)
        {
        }

        public override Vector3 GetDir()
        {
            return -base.GetDir();
        }
    }
}
