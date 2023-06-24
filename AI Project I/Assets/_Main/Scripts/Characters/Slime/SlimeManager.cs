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

        private void Start()
        {
            SetRandomNode();
        }
        
        private void SetRandomPos()
        {
            var posX = Random.Range(-range.x, range.x);
            var posZ = Random.Range(-range.y,range.y);
            _randPos = new Vector3(posX, 0, posZ);
        }
        
        private void SetRandomNode()
        {
            SetRandomPos();
            
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