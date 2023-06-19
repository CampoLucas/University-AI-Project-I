using System;
using UnityEngine;
using Game.Interfaces;
using Game.SO;

namespace Game.Entities
{
    public class Rotation : IRotation
    {
        private StatSO _data;
        private Transform _transform;

        public Rotation(Transform transform, StatSO data)
        {
            _data = data;
            _transform = transform;
        }

        private Quaternion _targetRotation = Quaternion.identity;
        private Vector3 _lastTargetDir;

        public void Rotate(Vector3 dir)
        {
            _lastTargetDir = dir.normalized;

            if (_lastTargetDir == Vector3.zero)
            {
                _lastTargetDir = _transform.forward;
            }

            var rs = _data.RotSpeed; // rs == rotation speed

            var tr = Quaternion.LookRotation(_lastTargetDir); // tr == target rotation
            _targetRotation = Quaternion.Slerp(_transform.rotation, tr, rs * Time.deltaTime);

            _transform.rotation = _targetRotation;
        }

        public void Dispose()
        {
            _data = null;
            _transform = null;
        }
    }
}