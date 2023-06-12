using System;
using Game.Entities.Flocking;
using Game.Interfaces;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeModel : MonoBehaviour, IBoid
    {
        private Rigidbody _rb;
        private SlimeFlockingManager _slimeFlocking;

        public Vector3 Position => transform.position;
        public Vector3 Front => transform.forward;
        public float Radius => radius;
        
        //Test nor final
        [SerializeField] private float moveSpeed = 1;
        [SerializeField] private float rotSpeed = 1;
        [SerializeField] private float moveLerpSpeed = 10;
        [SerializeField] private float radius;

        public bool hasToMove;
        private bool _isFlockingNull;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _slimeFlocking = GetComponent<SlimeFlockingManager>();
        }
        
        private void Start()
        {
            _isFlockingNull = _slimeFlocking == null;
        }


        public void Move(Vector3 dir)
        {
            var targetVelocity = dir * moveSpeed;
            _rb.velocity = Vector3.Lerp(_rb.velocity, targetVelocity, moveLerpSpeed);
        }

        public void LookDir(Vector3 dir)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
        }

        public Vector3 GetFlockingDir()
        {
            return _isFlockingNull ? Vector3.zero : _slimeFlocking.GetDir();
        }

        public void Dispose()
        {
            
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Position, Radius);
        }
    }
}