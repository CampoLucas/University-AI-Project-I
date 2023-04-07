using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateIdle<T> : PlayerStateBase<T>
    {
        private readonly T _inMoving;
        private readonly T _inLightAttack;
        private readonly T _inHeavyAttack;
        private readonly T _inDamage;
        private readonly T _inDead;

        public PlayerStateIdle(in T inMoving, in T inLightAttack, in T inHeavyAttack, in T inDamage, in T inDead)
        {
            _inMoving = inMoving;
            _inLightAttack = inLightAttack;
            _inHeavyAttack = inHeavyAttack;
            _inDamage = inDamage;
            _inDead = inDead;
        }

        public override void Awake()
        {
            base.Awake();
            //Model.Damageable.OnTakeDamage += TakeDamageHandler;
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.IsAlive())
            {
                if (Model.HasTakenDamage())
                {
                    Debug.Log("Damage Transition");
                    Fsm.Transitions(_inDamage);
                }

                if (Inputs.MoveDir != Vector3.zero)
                {
                    Fsm.Transitions(_inMoving);
                }

                if (Inputs.FlagLightAttack)
                {
                    Fsm.Transitions(_inLightAttack);
                }

                if (Inputs.FlagHeavyAttack)
                {
                    Fsm.Transitions(_inHeavyAttack);
                }
                
                View.UpdateMovementValues(Inputs.MoveAmount);
            }
            else
            {
                Fsm.Transitions(_inDead);
            }
            
        }

        private void TakeDamageHandler()
        {
            Fsm.Transitions(_inDamage);
        }

        public override void Sleep()
        {
            base.Sleep();
            //Model.Damageable.OnTakeDamage -= TakeDamageHandler;
        }
    }
}