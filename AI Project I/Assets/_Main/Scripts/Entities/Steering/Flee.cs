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

        /// <summary>
        /// A method that gets the flee direction.
        /// </summary>
        /// <returns> Returns the opposite direction of seek</returns>
        public override Vector3 GetDir()
        {
            return -base.GetDir();
        }
    }
}
