using System;
using System.Collections.Generic;
using Game.Entities.Flocking;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Slime
{
    public sealed class SlimeFlockingManager : FlockingManager
    {
        public SlimeFlockingManager(SlimeSO data, IBoid self, Levelable levelable) : base(self, data)
        {
            var predator = new Predator(data,levelable);
            var alignment = new Alignment(data);
            var cohesion = new Cohesion(data);
            var avoidance = new Avoidance(data);

            Flocking = new IFlocking[] {predator,alignment,cohesion, avoidance};
        }
    }
}