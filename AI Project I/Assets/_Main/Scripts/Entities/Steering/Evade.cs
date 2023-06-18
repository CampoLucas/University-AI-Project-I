using System.Collections;
using System.Collections.Generic;
using Game.Entities;
using UnityEngine;

namespace Game.Entities.Steering
{
    public class Evade : Pursuit
    {
        public Evade(Transform origin, EntityModel target, float time): base(origin, target, time) { }

        /// <summary>
        /// A method that gets the evade direction.
        /// </summary>
        /// <returns> Returns the opposite direction of pursuit</returns>
        public override Vector3 GetDir()
        {
            return - base.GetDir();
        }
    }
}
