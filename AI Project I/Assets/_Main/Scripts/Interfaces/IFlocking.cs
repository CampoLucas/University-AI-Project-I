using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IFlocking
    {
        Vector3 GetDir(List<IBoid> boids, IBoid self);
    }
}
