using System;
using UnityEngine;
using Game.Entities;

namespace Game.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [field: SerializeField] public Transform Target { get; private set; }
        private EnemyModel _model;

        private void Awake()
        {
            _model = GetComponent<EnemyModel>();
        }

        private void Update()
        {
            
        }

        //private bool 
        
        //private bool TriesToScape() => _model.TriesToScape();
        //private bool CloseEnoughToAttack() => _model.CloseEnoughToAttack(target);
        private bool SeePlayer() => _model.SeePlayer(Target);
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