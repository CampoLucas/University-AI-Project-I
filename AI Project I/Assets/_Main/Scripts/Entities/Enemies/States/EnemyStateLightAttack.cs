namespace Game.Enemies.States
{
    public class EnemyStateLightAttack<T> : EnemyStateBase<T>
    {
        public override void Awake()
        {
            base.Awake();
            Model.LightAttack();
            View.PlayTargetAnimation(Model.CurrentWeapon().GetData().LightAttack01.EventHash);
            var timer = Model.CurrentWeapon().GetData().LightAttack01.Duration;
            Model.SetTimer(timer);
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.GetCurrentTimer() > 0)
            {
                Model.RunTimer();
            }
        }

        public override void Sleep()
        {
            base.Sleep();
            Model.SetTimer(0);
        }
    }
}