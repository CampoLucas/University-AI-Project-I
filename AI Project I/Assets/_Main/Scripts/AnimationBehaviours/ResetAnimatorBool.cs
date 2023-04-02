using UnityEngine;

namespace Game.AnimationBehaviour
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        [SerializeField] private string boolParameter;
        [SerializeField] private bool state;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(boolParameter, state);
        }
    }
}
