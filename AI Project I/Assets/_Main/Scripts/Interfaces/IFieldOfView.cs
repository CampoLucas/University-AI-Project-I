using System;
using UnityEngine;

namespace Game.Interfaces
{
    /// <summary>
    /// Interface for a Field of View system.
    /// </summary>
    public interface IFieldOfView : IDisposable
    {
        /// <summary>
        /// This method checks if the distance between the origin of the field of view and the target.
        /// </summary>
        /// <param name="target"> The target's transform</param>
        /// <returns> It returns a boolean value indicating whether the target is within range or not.</returns>
        bool CheckRange(Transform target);
        /// <summary>
        /// This method calculates the angle between the forward direction of the origin of the field of view and the direction to the target.
        /// </summary>
        /// <param name="target"> The target's transform</param>
        /// <returns> It returns true indicating that the target is within the field of view angle, otherwise it returns false.</returns>
        bool CheckAngle(Transform target);
        /// <summary>
        /// Checks if any objects are obstructing the line of sight between the origin and the target.
        /// </summary>
        /// <param name="target"> The target's transform</param>
        /// <returns> If there are no objects obstructing the line of sight, it returns true, otherwise it returns false</returns>
        bool CheckView(Transform target);
    }
}