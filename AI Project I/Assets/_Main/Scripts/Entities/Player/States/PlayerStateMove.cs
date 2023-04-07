﻿using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateMove<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inLightAttack;
        private readonly T _inHeavyAttack;
        private readonly T _inDamage;
        private readonly T _inDead;
        
        public PlayerStateMove(in T inIdle, in T inLightAttack, in T inHeavyAttack, in T inDamage, in T inDead)
        {
            _inIdle = inIdle;
            _inLightAttack = inLightAttack;
            _inHeavyAttack = inHeavyAttack;
            _inDamage = inDamage;
            _inDead = inDead;
        }

        // public override void Awake()
        // {
        //     base.Awake();
        //     Model.Damageable.OnTakeDamage += TakeDamageHandler;
        // }

        public override void Execute()
        {
            base.Execute();
            if (Model.IsAlive())
            {
                if (Model.HasTakenDamage())
                {
                    Fsm.Transitions(_inDamage);
                }
            
                if (Inputs.MoveDir == Vector3.zero)
                {
                    Fsm.Transitions(_inIdle);
                }

                if (Inputs.FlagLightAttack)
                {
                    Fsm.Transitions(_inLightAttack);
                }
            
                if (Inputs.FlagHeavyAttack)
                {
                    Fsm.Transitions(_inHeavyAttack);
                }
            
                Model.Move(Inputs.MoveDir);
                Model.Rotate(Inputs.MoveDir);
                View.UpdateMovementValues(Inputs.MoveAmount);
            }
            else
            {
                Fsm.Transitions(_inDead);
            }
        }
        
        // private void TakeDamageHandler()
        // {
        //     Fsm.Transitions(_inDamage);
        // }
        
        public override void Sleep()
        {
            base.Sleep();
            //Model.Damageable.OnTakeDamage -= TakeDamageHandler;
            Model.Move(Vector3.zero);
        }
    }
}