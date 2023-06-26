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
        public SlimeFlockingManager(SlimeSO data, IBoid self, Levelable levelable,Transform leaderTrans) : base(self, data)
        {
            var predator = new Predator(data,levelable);
            var alignment = new Alignment(data);
            var cohesion = new Cohesion(data);
            var leader = new Leader(leaderTrans,data);
            var avoidance = new Avoidance(data);

            Flocking = new IFlocking[] {predator,alignment,cohesion,leader, avoidance};
        }
        
        /*public SlimeFlockingManager(SlimeSO data, IBoid self, Levelable levelable) : base(self, data.MaxBoids, data.WhatIsBoid, data.FlockingMultiplier)
        {
            var predator = new Predator(data.PredatorMultiplier, data.PredatorRange, data.MaxPredators, data.WhatIsPredator, levelable);
            var alignment = new Alignment(data.AlignmentMultiplier);
            var cohesion = new Cohesion(data.CohesionMultiplier);

            Flocking = new IFlocking[] {predator,alignment,cohesion};
        }*/
    }
}