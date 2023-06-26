using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IPathfinder
    {
        List<Vector3> Waypoints { get; }
        int NextPoint { get; }
        Transform Target { get; }


        bool IsTargetInRange();
        void SetTarget(Transform target);
        void Run();
        void SetNextPoint();
        bool SetNodes(Vector3 origin, Vector3 target);
    }
}
