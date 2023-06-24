using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IBoid
    {
        Vector3 Position { get; }
        Vector3 Front { get; }
        float Radius { get; }
    }
}
