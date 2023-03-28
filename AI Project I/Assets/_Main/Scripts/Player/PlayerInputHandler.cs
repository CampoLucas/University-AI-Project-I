using System;
using Game.InputActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector3 MoveDir { get; private set; }
        public float MoveAmount { get; private set; }
        public bool FlagAttack { get; private set; }

        private PlayerInputs _input;

        private void OnEnable()
        {
            _input ??= new PlayerInputs();

            _input.Player.Movement.performed += MovementPerformedHandler;
            _input.Player.Movement.canceled += MovementPerformedHandler;
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Player.Movement.performed -= MovementPerformedHandler;
            _input.Player.Movement.canceled -= MovementPerformedHandler;
            _input.Disable();
        }

        private void Update()
        {
            HandleInput();
        }

        #region InputEventHandler
        private void MovementPerformedHandler(InputAction.CallbackContext context)
        {
            MoveDir = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        }
        
        private void MovementCanceledHandler(InputAction.CallbackContext context)
        {
            MoveDir = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        }
        #endregion

        private void HandleInput()
        {
            MoveInput();
        }
        
        private void MoveInput()
        {
            MoveAmount = Mathf.Clamp01(Mathf.Abs(MoveDir.x) + Mathf.Abs(MoveDir.z));
        }

        private void AttackInput()
        {
            
        }
    }
}