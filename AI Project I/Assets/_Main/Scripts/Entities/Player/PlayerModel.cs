using System;
using Game.Entities;
using Game.Interfaces;
using Game.Items.Weapons;
using UnityEngine;

namespace Game.Player
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        private IMovement _move;
        private IRotation _rotate;
        private IAttack _lightAttack;
        private IAttack _heavyAttack;

        private void Awake()
        {
            _move = GetComponent<IMovement>();
            _rotate = GetComponent<IRotation>();
            _lightAttack = GetComponent<LightAttack>();
            _heavyAttack = GetComponent<HeavyAttack>();
        }

        public void Move(Vector3 dir) => _move?.Move(dir);
        public void Rotate(Vector3 dir) => _rotate?.Rotate(dir);
        public void LightAttack(PlayerView anim) => _lightAttack.Attack(weapon, anim);
        public void HeavyAttack(PlayerView anim) => _heavyAttack.Attack(weapon, anim);
    }
}