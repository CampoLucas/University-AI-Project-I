using System;
using UnityEngine;

namespace Game.Interfaces
{
    public interface IFieldOfView : IDisposable
    {
        bool CheckRange(Transform target);
        bool CheckAngle(Transform target);
        bool CheckView(Transform target);
    }
}