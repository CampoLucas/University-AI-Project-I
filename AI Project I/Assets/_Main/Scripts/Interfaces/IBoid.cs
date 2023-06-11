using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IBoid : IDisposable
    {
        void Move(Vector3 dir);
        void LookDir(Vector3 dir);
        Vector3 Position { get; }
        Vector3 Front { get; }
        float Radius { get; }
    }
}
