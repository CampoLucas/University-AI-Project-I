using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Sheared
{
    public class BoxOverlapTrigger : MonoBehaviour
    {
        public Action<Collider> OnOverlapEnter;
        public Action<Collider> OnOverlapStay;
        public Action<Collider> OnOverlapExit;
        [SerializeField] private Vector3 boxSize;
        [SerializeField] private Vector3 boxCenter;
        [SerializeField] private LayerMask damageLayerMask;
        private Collider[] _hits = new Collider[2];
        private int _cachedHits;
        private readonly List<Collider> _collidersInBox = new();
        private List<Collider> _cachedNewColliders;
        private bool _enable;
        

        private void Update()
        {
            if (_enable)
            {
                var transform1 = transform;
                _cachedHits = Physics.OverlapBoxNonAlloc(transform1.position + boxCenter, (transform1.lossyScale/2) + (boxSize/2), _hits, transform1.localRotation, damageLayerMask);
                if (_cachedNewColliders == null)
                    _cachedNewColliders = new List<Collider>();

                // Check for new colliders
                for (var i = 0; i < _cachedHits; i++)
                {
                    var hit = _hits[i];
                    if (hit != null && !_collidersInBox.Contains(hit))
                    {
                        OnOverlapEnter?.Invoke(hit);
                        _cachedNewColliders.Add(hit);
                    }
                    else if (hit != null)
                    {
                        OnOverlapStay?.Invoke(hit);
                    }
                }

                // Check for removed colliders
                for (var i = _collidersInBox.Count - 1; i >= 0; i--)
                {
                    var other = _collidersInBox[i];
                    if (!Array.Exists(_hits, element => element == other))
                    {
                        OnOverlapExit?.Invoke(other);
                        _collidersInBox.RemoveAt(i);
                    }
                }

                // Update _collidersInBox list
                _collidersInBox.AddRange(_cachedNewColliders);
                _cachedNewColliders.Clear();
            }
        }

        public void EnableCollider()
        {
            _enable = true;
            _collidersInBox.Clear();
            _hits = new Collider[2];
        }

        public void DisableCollider()
        {
            _enable = false;
            foreach (var other in _collidersInBox)
            {
                OnOverlapExit?.Invoke(other);
            }
            _collidersInBox.Clear();
        }

        private void OnDrawGizmos()
        {
            var color = Color.red;
            if (!_enable)
                color = new Color(0.4f, 0, 0, 1);
            if (_collidersInBox.Count > 0)
                color = new Color(0.5f, 0, 1, 1);
            Gizmos.color = color;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawWireCube(boxCenter, boxSize);
        }
    }
}