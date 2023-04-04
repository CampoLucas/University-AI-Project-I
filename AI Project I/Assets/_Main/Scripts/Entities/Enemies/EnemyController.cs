using System;
using UnityEngine;
using Game.Entities;

namespace Game.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private EnemyModel _model;

        private void Awake()
        {
            _model = GetComponent<EnemyModel>();
        }

        private void Update()
        {
            if (target && SeePlayer())
            {
                _model.FollowTarget(target);
            }
            else
            {
                
            }
        }

        //private bool 
        
        //private bool TriesToScape() => _model.TriesToScape();
        //private bool CloseEnoughToAttack() => _model.CloseEnoughToAttack(target);
        private bool SeePlayer() => _model.SeePlayer(target);
        //private bool IsLifeLow() =>
        
        private void ActionFollowTarget()
        {
            
        }

        private void ActionLightAttack()
        {
            
        }

        private void ActionHeavyAttack()
        {
            
        }

        private void ActionScape()
        {
            
        }

        private void ActionLookAround()
        {
            
        }

        private void ActionPatrol()
        {
            
        }

        private void ActionIdle()
        {
            
        }
    }
}