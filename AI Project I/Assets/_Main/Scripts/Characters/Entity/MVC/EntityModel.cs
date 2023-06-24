using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;
using Game.Sheared;
using Game.SO;

namespace Game.Entities
{
    public class EntityModel : MonoBehaviour
    {
        public Damageable Damageable { get; private set; }
        public int Level { get; private set; }
        
        [SerializeField] private bool spawnable;
        [SerializeField] private StatSO stats;
        [SerializeField] private Weapon weapon;

        protected Rigidbody Rigidbody { get; private set; }
        private IMovement _walkMovement;
        private IMovement _runMovement;
        private IMovement _movement;
        private IRotation _rotate;
        private IAttack _lightAttack;
        private IAttack _heavyAttack;
        private WaitTimer _waitTimer;

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();

            _runMovement = new Movement(stats.MoveSpeed, stats.MoveLerpSpeed, Rigidbody);
            _walkMovement = new Movement(stats.WalkSpeed, stats.MoveLerpSpeed, Rigidbody);
            SetMovement(_runMovement);
            SetRotation(new Rotation(transform, stats));
            _lightAttack = GetComponent<LightAttack>();
            _heavyAttack = GetComponent<HeavyAttack>();
            Damageable = GetComponent<Damageable>();
            _waitTimer = new WaitTimer();
            
            //Level = stats.Level;
        }

        public virtual void Move(Vector3 dir) => _movement?.Move(dir);
        public void Rotate(Vector3 dir) => _rotate?.Rotate(dir);
        public StatSO GetData() => stats;
        public T GetData<T>() where T : StatSO => (T)stats;
        public float GetSpeed() => Rigidbody.velocity.magnitude;
        public Vector3 GetForward() => transform.forward;
        public bool IsAlive() => Damageable.IsAlive();
        public bool IsInvulnerable() => Damageable.IsInvulnerable();
        public bool HasTakenDamage() => Damageable.HasTakenDamage();
        public Weapon CurrentWeapon() => weapon;
        public void LightAttack() => _lightAttack.Attack();
        public void CancelLightAttack() => _lightAttack.CancelAttack();
        public void HeavyAttack() => _heavyAttack.Attack();
        public void CancelHeavyAttack() => _heavyAttack.CancelAttack();
        public void IncreaseLevel() => Level++;
        
        #region Timer Methods

        public bool GetTimerComplete() => _waitTimer?.TimerComplete() ?? default;
        public float GetRandomTime(float maxTime) => _waitTimer?.GetRandomTime(maxTime) ?? default;
        public void RunTimer() => _waitTimer?.RunTimer();
        public void SetTimer(float time) => _waitTimer?.SetTimer(time);

        #endregion

        #region Strategy
        
        public IMovement GetWalkingMovement()
        {
            if (_walkMovement == null) return default;
            return _walkMovement;
        }

        public IMovement GetRunningMovement()
        {
            if (_runMovement == null) return default;
            return _runMovement;
        }

        public void SetMovement(IMovement movement)
        {
            if (_movement == movement) return;
            if (_movement != null && !(movement == _walkMovement || movement == _runMovement))
                _movement.Dispose();
            _movement = movement;
        }

        public void SetRotation(IRotation rotation)
        {
            if (_rotate == rotation) return;
            if (_rotate != null)
                _rotate.Dispose();
            _rotate = rotation;
        }
        #endregion

        protected virtual void OnDestroy()
        {
            if (_movement != null)
            {
                _movement.Dispose();
                _movement = null;
            }
            if (_rotate != null)
            {
                _rotate.Dispose();
                _rotate = null;
            }
        }
    }

    public enum MovementType
    {
        Walk, Run
    }
}