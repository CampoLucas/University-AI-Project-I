using System;
using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;
using Game.SO;

namespace Game.Entities
{
    public class EntityModel : MonoBehaviour
    {
        public Damageable Damageable { get; private set; }
        [SerializeField] private StatSO stats;
        [SerializeField] private Weapon weapon;
        private IMovement _move;
        private IRotation _rotate;
        private IAttack _lightAttack;
        private IAttack _heavyAttack;
        private WaitTimer _waitTimer;

        protected virtual void Awake()
        {
            _move = GetComponent<IMovement>();
            _rotate = GetComponent<IRotation>();
            _lightAttack = GetComponent<LightAttack>();
            _heavyAttack = GetComponent<HeavyAttack>();
            Damageable = GetComponent<Damageable>();
            _waitTimer = GetComponent<WaitTimer>();
        }

        public virtual void Move(Vector3 dir, float moveAmount) => _move?.Move(dir, moveAmount);
        public void Rotate(Vector3 dir) => _rotate?.Rotate(dir);
        public void LightAttack() => _lightAttack.Attack();
        public void HeavyAttack() => _heavyAttack.Attack();
        /// <summary>
        /// Current life is grater than 0
        /// </summary>
        /// <returns></returns>
        public bool IsAlive() => Damageable.IsAlive();
        public bool IsInvulnerable() => Damageable.IsInvulnerable();
        public bool HasTakenDamage() => Damageable.HasTakenDamage();
        public Weapon CurrentWeapon() => weapon;
        
        // It will be useful when there are different SO that inherit from StatSO
        //public T GetData<T>() where T : StatSO => stats as T;
        public StatSO GetData() => stats;
        
        #region Timer Methods

        public float GetCurrentTimer() => _waitTimer ? _waitTimer.CurrentTime : default;
        public float GetRandomTime() => _waitTimer ? _waitTimer.GetRandomTime() : default;

        public void RunTimer()
        {
            if (_waitTimer)
            {
                _waitTimer.RunTimer();
            }
        }

        public void SetTimer(float time)
        {
            if (_waitTimer)
            {
                _waitTimer.SetTimer(time);
            }
        }

        #endregion
    }
}