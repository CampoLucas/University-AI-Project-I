using UnityEngine;
using Game.Interfaces;

namespace Game.Shared
{
    public class Rotation : MonoBehaviour, IRotation
    {
        [SerializeField] private float speed = 10;
        private Quaternion _targetRotation = Quaternion.identity;
        private Vector3 _lastTargetDir;
        
        public void Rotate(Vector3 dir)
        {
            var targetDir = dir.normalized;

            if (targetDir == Vector3.zero)
            {
                targetDir = transform.forward;
            }

            var rs = speed; // rs == rotation speed

            var tr = Quaternion.LookRotation(targetDir); // tr == target rotation
            _targetRotation = Quaternion.Slerp(_targetRotation, tr, rs * Time.deltaTime);

            transform.rotation = _targetRotation;
        }
    }
}