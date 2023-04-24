using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interfaces
{
    public interface ISteering : IDisposable
    {
        /// <summary>
        /// Method that calculates the direction and returns it. 
        /// </summary>
        Vector3 GetDir();
    }
}
