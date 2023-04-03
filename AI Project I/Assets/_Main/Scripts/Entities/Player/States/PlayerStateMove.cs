using UnityEngine;

namespace Game.Player.States
{
    public class PlayerStateMove<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inLightAttack;
        private readonly T _inHeavyAttack;
        private readonly T _inDamage;
        
        public PlayerStateMove(in T inIdle, in T inLightAttack, in T inHeavyAttack, in T inDamage)
        {
            _inIdle = inIdle;
            _inLightAttack = inLightAttack;
            _inHeavyAttack = inHeavyAttack;
            _inDamage = inDamage;
        }

        public override void Execute()
        {
            base.Execute();
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
        
        public override void Sleep()
        {
            base.Sleep();
            Model.Move(Vector3.zero);
        }
    }
}