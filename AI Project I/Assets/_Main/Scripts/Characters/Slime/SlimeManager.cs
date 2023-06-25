using System;
using Game.Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Entities.Slime
{
    public class SlimeManager : MonoBehaviour
    {
        [SerializeField] private SlimeModel[] slimes;
        [SerializeField] private Vector2 range;
        private Node _target;
        private Vector3 _randPos;
        private Vector3[] _locations;

        private void Awake()
        {
            SetLocations();
        }

        private void Start()
        {
            SetRandomNode();
        }

        private void SetLocations()
        {
            _locations = new Vector3[10];
            
            for (int i = 0; i < _locations.Length; i++)
            {
                var posX = Random.Range(-range.x, range.x);
                var posZ = Random.Range(-range.y,range.y);
                _locations[i] = new Vector3(posX, 0, posZ);
            }
        }

        private void SetRandomPos()
        {
            var posX = Random.Range(-range.x, range.x);
            var posZ = Random.Range(-range.y,range.y);
            _randPos = new Vector3(posX, 0, posZ);
        }
        
        private void SetRandomNode()
        {
            var rand = Random.Range(0, _locations.Length - 1);
            _randPos = _locations[rand];
            
            _target = NodeGrid.GetInstance().GetClosestNode(_randPos);
            
            if(_target == null) return;
            
            slimes = GetComponentsInChildren<SlimeModel>();
            
            foreach (var slime in slimes)
            {
                slime.SetTarget(_target.transform);
            }
        }
    }
}