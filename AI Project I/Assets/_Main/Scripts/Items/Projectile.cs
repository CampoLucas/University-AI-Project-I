﻿
using System;
using Game.Entities;
using Game.Items.Weapons;
using Game.Sheared;
using Game.SO;
using UnityEngine;

namespace Game.Items
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        private Rigidbody _rb;
        private Movement _movement;
        private DamageBox _damageBox;
        private BoxCastTrigger _overlap;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _movement = new Movement(speed, 1, _rb);
            _damageBox = GetComponent<DamageBox>();
            _overlap = GetComponent<BoxCastTrigger>();
            _overlap.EnableCollider();
        }
        
        public void InitData(WeaponSO data)
        {
            _damageBox.InitData(data);
            
        }

        private void Update()
        {
            _movement.Move(transform.forward);
        }
    }
}