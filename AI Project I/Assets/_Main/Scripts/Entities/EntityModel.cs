using UnityEngine;
using Game.Interfaces;
using Game.Items.Weapons;
using Game.SO;

namespace Game.Entities
{
    public class EntityModel : MonoBehaviour
    {
        [SerializeField] private StatSO stats;
        [SerializeField] private Weapon weapon;
        private IMovement _move;
        private IRotation _rotate;
        private IAttack _lightAttack;
        private IAttack _heavyAttack;
        private Damageable _damageable;
        private WaitTimer _waitTimer;

        protected virtual void Awake()
        {
            _move = GetComponent<IMovement>();
            _rotate = GetComponent<IRotation>();
            _lightAttack = GetComponent<LightAttack>();
            _heavyAttack = GetComponent<HeavyAttack>();
            _damageable = GetComponent<Damageable>();
            _waitTimer = GetComponent<WaitTimer>();
        }

        public virtual void Move(Vector3 dir) => _move?.Move(dir);
        public void Rotate(Vector3 dir) => _rotate?.Rotate(dir);
        public void LightAttack() => _lightAttack.Attack(weapon);
        public void HeavyAttack() => _heavyAttack.Attack(weapon);
        /// <summary>
        /// Current life is grater than 0
        /// </summary>
        /// <returns></returns>
        public bool IsAlive() => _damageable.IsAlive();
        public bool IsInvulnerable() => _damageable.IsInvulnerable();
        public bool TakesDamage() => _damageable.HasTakenDamage();
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