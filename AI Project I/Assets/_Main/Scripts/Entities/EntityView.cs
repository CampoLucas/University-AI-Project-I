using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

namespace Game.Entities
{
    public class EntityView : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int MoveAmount = Animator.StringToHash("MoveAmount");
        private static readonly int Interacting = Animator.StringToHash("IsInteracting");
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public bool IsInteracting() => _animator.GetBool(Interacting);
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
        
        public void PlayTargetAnimation(string stateName, bool isInteracting = false)
        {
            _animator.SetBool(Interacting, isInteracting);
            _animator.CrossFade(stateName, 0.2f);
        }
    }
}