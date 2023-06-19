using System;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities
{
    public class Movement : IMovement
    {
        public Rigidbody Rigidbody => _rb;
        public float Speed => _speed;
        public float LerpSpeed => _lerpSpeed;

        private Rigidbody _rb;
        private Vector3 _normalVector;
        private Vector3 _cachedProjectedVelocity;
        private float _speed;
        public float _lerpSpeed;

        public Movement(float speed, float lerpSpeed, Rigidbody rb)
        {
            _speed = speed;
            _lerpSpeed = lerpSpeed;
            _rb = rb;
        }

        public Movement(Movement other)
        {
            _speed = other._speed;
            _lerpSpeed = other._lerpSpeed;
            _rb = other._rb;
        }

        public void Move(Vector3 dir)
        {
            var targetVelocity = dir * _speed;
            _cachedProjectedVelocity = Vector3.ProjectOnPlane(targetVelocity, _normalVector);
            
            _rb.velocity = Vector3.Lerp(_rb.velocity, _cachedProjectedVelocity, _lerpSpeed);
        }

        public void Dispose()
        {
            _rb = null;
        }
    }
}