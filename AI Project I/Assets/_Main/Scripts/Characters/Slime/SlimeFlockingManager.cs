﻿using System;
using System.Collections.Generic;
using Game.Entities.Flocking;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Slime
{
    public sealed class SlimeFlockingManager : FlockingManager
    {
        public SlimeFlockingManager(SlimeSO data, IBoid self) : base(self, data.MaxBoids, data.WhatIsBoid, data.FlockingMultiplier)
        {
            var predator = new Predator(data.PredatorMultiplier, data.PredatorRange, data.MaxPredators, data.WhatIsPredator);
            var alignment = new Alignment(data.AlignmentMultiplier);
            var cohesion = new Cohesion(data.CohesionMultiplier);

            Flocking = new IFlocking[] {predator,alignment,cohesion};
        }
    }
}