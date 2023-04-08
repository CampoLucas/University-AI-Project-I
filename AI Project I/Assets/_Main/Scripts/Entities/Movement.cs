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
        private float _speed;

        private void Awake()
        {
            _data = GetComponent<EntityModel>().GetData();
            _rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 dir, float moveAmount)
        {
            if (moveAmount > 0.5f)
            {
                _speed = _data.MoveSpeed;
            }
            else
            {
                _speed = _data.WalkSpeed;
            }
            
            var targetVelocity = dir * _speed;
            _cachedProjectedVelocity = Vector3.ProjectOnPlane(targetVelocity, _normalVector);
            
            _rb.velocity = Vector3.Lerp(_rb.velocity, _cachedProjectedVelocity, _data.MoveLerpSpeed);
        }
    }
}