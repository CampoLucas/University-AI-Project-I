using UnityEngine;

namespace Game.Player
{
    public class PlayerView : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int MoveAmount = Animator.StringToHash("MoveAmount");
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        
        public void UpdateMovementValues(float moveAmount)
        {
            var amount = moveAmount switch
            {
                > 0f and < 0.55f => 0.5f,
                > 0.55f => 1,
                < 0 and > -0.55f => -0.5f,
                < -0.55f => -1,
                _ => 0
            };

            _animator.SetFloat(MoveAmount, amount, 0.1f, Time.deltaTime);
        }
        
        public void PlayTargetAnimation(string animName)
        {
            //_animator.SetBool(Interacting, isInteracting);
            _animator.CrossFade(animName, 0.2f);
        }
    }
}