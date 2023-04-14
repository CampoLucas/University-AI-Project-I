using System;
using Game.Entities;
using Game.Items.Weapons;

namespace Game.Interfaces
{
    public interface IAttack : IDisposable
    {
        void Attack();
        void CancelAttack();
    }
}