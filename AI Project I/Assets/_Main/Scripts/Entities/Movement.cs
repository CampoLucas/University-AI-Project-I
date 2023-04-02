using System;
using Game.Interfaces;
using UnityEngine;

namespace Game.Entities
{
    public class Movement : MonoBehaviour, IMovement
    {
        [SerializeField] private float speed = 5;
        [SerializeField] private float lerpSpeed = 10;
        private Rigidbody _rb;
        private Vector3 _normalVector;
        private Vector3 _cachedProjectedVelocity;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 dir)
        {
            var targetVelocity = dir * speed;
            _cachedProjectedVelocity = Vector3.ProjectOnPlane(targetVelocity, _normalVector);
            
            _rb.velocity = Vector3.Lerp(_rb.velocity, _cachedProjectedVelocity, lerpSpeed);
        }
    }
}