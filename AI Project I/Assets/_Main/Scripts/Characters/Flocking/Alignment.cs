using System.Collections.Generic;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public class Alignment : IFlocking
    {
        private readonly float _multiplier;
        private readonly SlimeSO _data;
        
        public Alignment(float multiplier)
        {
            _multiplier = multiplier;
        }

        public Alignment(SlimeSO data)
        {
            _data = data;
        }
        
        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            Vector3 front = Vector3.zero;
            for (int i = 0; i < boids.Count; i++)
            {
                front += boids[i].Front;
            }
            return front.normalized * _data.AlignmentMultiplier;
        }
    }
}
