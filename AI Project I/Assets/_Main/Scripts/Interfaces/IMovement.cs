using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IMovement : IDisposable
    {
        void Move(Vector3 dir, float moveAmount);
    }
}