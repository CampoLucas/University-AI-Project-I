using Game.Entities;
using Game.Items.Weapons;

namespace Game.Interfaces
{
    public interface IAttack
    {
        void Attack();
        void CancelAttack();
    }
}