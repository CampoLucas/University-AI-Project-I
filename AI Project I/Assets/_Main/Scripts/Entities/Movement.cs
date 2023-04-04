using System;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities
{
    public class Movement : MonoBehaviour, IMovement
    {
        private StatSO _data;
        private Rigidbody _rb;
        private Vector3 _normalVector;
        private Vector3 _cachedProjectedVelocity;

        private void Awake()
        {
            _data = GetComponent<EntityModel>().GetData();
            _rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 dir)
        {
            var targetVelocity = dir * _data.MoveSpeed;
            _cachedProjectedVelocity = Vector3.ProjectOnPlane(targetVelocity, _normalVector);
            
            _rb.velocity = Vector3.Lerp(_rb.velocity, _cachedProjectedVelocity, _data.MoveLerpSpeed);
        }
    }
}