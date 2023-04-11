using UnityEngine;

namespace Game.Interfaces
{
    public interface IRotation : IOnDestroy
    {
        void Rotate(Vector3 dir);
    }
}