using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IRotation : IDisposable
    {
        void Rotate(Vector3 dir);
    }
}