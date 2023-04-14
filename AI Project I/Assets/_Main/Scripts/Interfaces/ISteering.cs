using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Interfaces
{
    public interface ISteering : IDisposable
    {
        Vector3 GetDir();
    }
}
