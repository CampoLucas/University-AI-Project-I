using System;
using UnityEngine;
using Game.Interfaces;
using Game.SO;

namespace Game.Entities
{
    public class Rotation : MonoBehaviour, IRotation
    {
        private StatSO _data;
        private Quaternion _targetRotation = Quaternion.identity;
        private Vector3 _lastTargetDir;

        private void Awake()
        {
            _data = GetComponent<EntityModel>().GetData();
        }

        public void Rotate(Vector3 dir)
        {
            _lastTargetDir = dir.normalized;

            if (_lastTargetDir == Vector3.zero)
            {
                _lastTargetDir = transform.forward;
            }

            var rs = _data.RotSpeed; // rs == rotation speed

            var tr = Quaternion.LookRotation(_lastTargetDir); // tr == target rotation
            _targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * Time.deltaTime);

            transform.rotation = _targetRotation;
        }
    }
}