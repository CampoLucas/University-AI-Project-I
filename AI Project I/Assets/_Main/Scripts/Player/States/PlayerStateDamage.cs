namespace Game.Player.States
{
    public class PlayerStateDamage<T> : PlayerStateBase<T>
    {
        private readonly T _inIdle;
        private readonly T _inMoving;

        public PlayerStateDamage(in T inIdle, in T inMoving)
        {
            _inIdle = inIdle;
            _inMoving = inMoving;
        }
    }
}