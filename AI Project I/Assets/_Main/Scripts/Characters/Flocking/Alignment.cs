using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public class Alignment : IFlocking
    {
        private readonly float _multiplier;
        
        public Alignment(float multiplier)
        {
            _multiplier = multiplier;
        }

        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            Vector3 front = Vector3.zero;
            for (int i = 0; i < boids.Count; i++)
            {
                front += boids[i].Front;
            }
            return front.normalized * _multiplier;
        }
    }
}
