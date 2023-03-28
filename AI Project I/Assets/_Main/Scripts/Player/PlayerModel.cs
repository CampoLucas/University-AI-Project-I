using System;
using Game.Interfaces;
using UnityEngine;

namespace Game.Player
{
    public class PlayerModel : MonoBehaviour
    {
        private IMovement _move;
        private IRotation _rotate;

        private void Awake()
        {
            _move = GetComponent<IMovement>();
            _rotate = GetComponent<IRotation>();
        }

        public void Move(Vector3 dir) => _move?.Move(dir);
        public void Rotate(Vector3 dir) => _rotate?.Rotate(dir);
    }
}