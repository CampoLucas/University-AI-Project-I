using System;
using System.Collections;
using System.Collections.Generic;
using Game.Shared.SO;
using UnityEngine;

namespace Game.Shared.Weapons
{
    // public class DamageBox : MonoBehaviour
    // {
    //     [SerializeField] private Weapon data;
    //     private Collider _damageCollider;
    //
    //     private void Awake()
    //     {
    //         _damageCollider = GetComponent<Collider>();
    //         _damageCollider.gameObject.SetActive(true);
    //         _damageCollider.isTrigger = true;
    //         _damageCollider.enabled = false;
    //     }
    //     
    //     public void EnableDamageCollider()
    //     {
    //         _damageCollider.enabled = true;
    //     }
    //     
    //     public void DisableDamageCollider()
    //     {
    //         _damageCollider.enabled = false;
    //     }
    //
    //     private void OnTriggerEnter(Collider other)
    //     {
    //         var target = other.GetComponent<Damageable>();
    //
    //         if (target)
    //         {
    //             target.TakeDamage(data.Damage);
    //         }
    //     }
    // }
    
    public class DamageBox : MonoBehaviour
    {
        [SerializeField] private Weapon data;
        [SerializeField] private Vector3 boxSize;
        [SerializeField] private Vector3 boxCenter;
        [SerializeField] private LayerMask damageLayerMask;
        private Collider[] _hits = new Collider[10];
        private int _cachedHits;
        private List<Collider> _collidersInBox = new List<Collider>();
        private List<Collider> _cachedNewColliders;
        private bool _enable;

        public void EnableCollider()
        {
            _enable = true;
            _collidersInBox.Clear();
            _hits = new Collider[10];
            StartCoroutine(OverlapCheck());
        }

        public void DisableCollider()
        {
            _enable = false;
            StopCoroutine(OverlapCheck());
            foreach (var coll in _collidersInBox)
            {
                OverlapExit(coll);
            }
            _collidersInBox.Clear();
        }
        
        private IEnumerator OverlapCheck()
        {
            while (_enable)
            {
                _cachedHits = Physics.OverlapBoxNonAlloc(transform.position + boxCenter, (transform.lossyScale/2) + (boxSize/2), _hits, transform.localRotation, damageLayerMask);
                if (_cachedNewColliders == null)
                    _cachedNewColliders = new List<Collider>();

                // Check for new colliders
                for (var i = 0; i < _cachedHits; i++)
                {
                    var hit = _hits[i];
                    if (hit != null && !_collidersInBox.Contains(hit))
                    {
                        OverlapEnter(hit);
                        _cachedNewColliders.Add(hit);
                    }
                    else if (hit != null)
                    {
                        OverlapStay(hit);
                    }
                }

                // Check for removed colliders
                for (var i = _collidersInBox.Count - 1; i >= 0; i--)
                {
                    var other = _collidersInBox[i];
                    if (!Array.Exists(_hits, element => element == other))
                    {
                        OverlapExit(other);
                        _collidersInBox.RemoveAt(i);
                    }
                }

                // Update _collidersInBox list
                _collidersInBox.AddRange(_cachedNewColliders);
                _cachedNewColliders.Clear();

                yield return null;
            }
        }

        private void OverlapEnter(Collider other)
        {
            var target = other.GetComponent<Damageable>();
            if (target)
            {
                target.TakeDamage(data.Damage);
            }
            
        }

        private void OverlapStay(Collider other)
        {
            
        }

        private void OverlapExit(Collider other)
        {
            
        }

        private void OnDrawGizmos()
        {
            var color = Color.red;
            if (!_enable)
                color = new Color(0.5f, 0, 0, 1);
            Gizmos.color = color;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(boxCenter, boxSize);
        }
    }
}