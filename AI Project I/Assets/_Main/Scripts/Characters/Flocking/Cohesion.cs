using System.Collections.Generic;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Flocking
{
    public class Cohesion : IFlocking
    {
        private readonly float _multiplier;
        private readonly SlimeSO _data;

        public Cohesion(float multiplier)
        {
            _multiplier = multiplier;
        }

        public Cohesion(SlimeSO data)
        {
            _data = data;
        }
        
        public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            Vector3 center = Vector3.zero;
            Vector3 dir = Vector3.zero;
            for (int i = 0; i < boids.Count; i++)
            {
                center += boids[i].Position;
            }
            if (boids.Count > 0)
            {
                center /= boids.Count;
                dir = center - self.Position;
            }
            return dir.normalized * _data.CohesionMultiplier;
        }

        /*public Vector3 GetDir(List<IBoid> boids, IBoid self)
        {
            Vector3 center = Vector3.zero;
            Vector3 dir = Vector3.zero;
            for (int i = 0; i < boids.Count; i++)
            {
                center += boids[i].Position;
            }
            if (boids.Count > 0)
            {
                center /= boids.Count;
                dir = center - self.Position;
            }
            return dir.normalized * _multiplier;
        }*/
    }
}
