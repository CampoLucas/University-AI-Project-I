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
        public bool FlagLightAttack { get; private set; }
        public bool FlagHeavyAttack { get; private set; }

        private PlayerInputs _input;
        private bool _inputLightAttack;
        private bool _inputHeavyAttack;

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

        private void LateUpdate()
        {
            FlagLightAttack = false;
            FlagHeavyAttack = false;
            _inputLightAttack = false;
            _inputHeavyAttack = false;
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
            AttackInput();
        }
        
        private void MoveInput()
        {
            MoveAmount = Mathf.Clamp01(Mathf.Abs(MoveDir.x) + Mathf.Abs(MoveDir.z));
        }

        private void AttackInput()
        {
            _input.Player.LightAttack.performed += i => _inputLightAttack = true;
            _input.Player.HeavyAttack.performed += i => _inputHeavyAttack = true;

            if (_inputLightAttack)
            {
                FlagLightAttack = true;
            }

            if (_inputHeavyAttack)
            {
                FlagHeavyAttack = true;
            }
        }
    }
}