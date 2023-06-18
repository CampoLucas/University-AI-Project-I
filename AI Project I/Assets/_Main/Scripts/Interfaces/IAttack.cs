using System;
using Game.Entities;
using Game.Items.Weapons;

namespace Game.Interfaces
{
    public interface IAttack : IDisposable
    {
        /// <summary>
        /// Method that execute the attack logic
        /// </summary>
        void Attack();
        /// <summary>
        /// Method that cancels the attack logic if it was interrupted
        /// </summary>
        void CancelAttack();
    }
}