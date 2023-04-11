using UnityEngine;

namespace Game.Interfaces
{
    public interface IMovement : IOnDestroy
    {
        void Move(Vector3 dir, float moveAmount);
    }
}