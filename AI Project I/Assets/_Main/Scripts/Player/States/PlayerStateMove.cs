using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateMove<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inAttack;
        private readonly T _inDamage;
        
        public PlayerStateMove(in T inIdle, in T inAttack, in T inDamage)
        {
            _inIdle = inIdle;
            _inAttack = inAttack;
            _inDamage = inDamage;
        }
        
        public override void Awake()
        {
            base.Awake();
            Debug.Log("Move");
        }

        public override void Execute()
        {
            base.Execute();
            if (Inputs.MoveDir == Vector3.zero)
            {
                Fsm.Transitions(_inIdle);
            }

            if (Inputs.FlagAttack)
            {
                Fsm.Transitions(_inAttack);
            }
            
            Model.Move(Inputs.MoveDir);
            Model.Rotate(Inputs.MoveDir);
            View.UpdateMovementValues(Inputs.MoveAmount);
        }
        
        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero);
        }
    }
}