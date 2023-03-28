namespace Game.Player.States
{
    public class PlayerStateAttack<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;
        private readonly T _inDamage;

        public PlayerStateAttack(T inIdle, T inMoving, T inDamage)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
            _inDamage = inDamage;
        }
    }
}