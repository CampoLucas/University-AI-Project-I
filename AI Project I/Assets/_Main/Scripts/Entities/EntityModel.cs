using System;
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
        [SerializeField] private StatSO stats;
        [SerializeField] private Weapon weapon;
        protected Rigidbody Rb;
        private IMovement _move;
        private IRotation _rotate;
        private IAttack _lightAttack;
        private IAttack _heavyAttack;
        private WaitTimer _waitTimer;

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            _move = new Movement(stats, Rb);
            _rotate = new Rotation(transform, stats);
            _lightAttack = GetComponent<LightAttack>();
            _heavyAttack = GetComponent<HeavyAttack>();
            Damageable = GetComponent<Damageable>();
            _waitTimer = new WaitTimer();
        }

        public virtual void Move(Vector3 dir, float moveAmount) => _move?.Move(dir, moveAmount);
        public void Rotate(Vector3 dir) => _rotate?.Rotate(dir);
        public StatSO GetData() => stats;
        public T GetData<T>() where T : StatSO => stats as T;
        public float GetSpeed() => Rb.velocity.magnitude;
        public Vector3 GetForward() => transform.forward;
        public bool IsAlive() => Damageable.IsAlive();
        public bool IsInvulnerable() => Damageable.IsInvulnerable();
        public bool HasTakenDamage() => Damageable.HasTakenDamage();
        public Weapon CurrentWeapon() => weapon;
        public void LightAttack() => _lightAttack.Attack();
        public void CancelLightAttack() => _lightAttack.CancelAttack();
        public void HeavyAttack() => _heavyAttack.Attack();
        public void CancelHeavyAttack() => _heavyAttack.CancelAttack();
        
        #region Timer Methods

        public bool GetTimerComplete() => _waitTimer?.TimerComplete() ?? default;
        public float GetRandomTime(float maxTime) => _waitTimer?.GetRandomTime(maxTime) ?? default;
        public void RunTimer() => _waitTimer?.RunTimer();
        public void SetTimer(float time) => _waitTimer?.SetTimer(time);
        
        #endregion

        protected virtual void OnDestroy()
        {
            _move.Dispose();
            _move = null;
            _rotate.Dispose();
            _rotate = null;
        }
    }
}