﻿using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateLightAttackOne<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;
        private readonly T _inDamage;
        private readonly T _inDead;

        public PlayerStateLightAttackOne(T inIdle, T inMoving, T inDamage, T inDead)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
            _inDamage = inDamage;
            _inDead = inDead;
        }

        public override void Awake()
        {
            base.Awake();
            Model.LightAttack();
            View.PlayTargetAnimation(Model.CurrentWeapon().GetData().LightAttack01.EventHash);
            var timer = Model.CurrentWeapon().GetData().LightAttack01.Duration;
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.IsAlive())
            {
                if (Model.HasTakenDamage())
                {
                    Fsm.Transitions(_inDamage);
                }
            
                if (Model.GetCurrentTimer() > 0)
                {
                    Model.RunTimer();
                }
                else
                {
                    if (Inputs.MoveDir != Vector3.zero)
                    {
                        Fsm.Transitions(_inMoving);
                    }
                    else
                    {
                        Fsm.Transitions(_inIdle);
                    }
                }
            }
            else
            {
                Fsm.Transitions(_inDead);
            }
        }
        
        public override void Sleep()
        {
            base.Sleep();
            Model.SetTimer(0);
        }
    }
}