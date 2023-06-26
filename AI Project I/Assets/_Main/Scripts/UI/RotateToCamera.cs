using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class RotateToCamera : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
