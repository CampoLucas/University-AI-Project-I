namespace Game.Enemies.States
{
    public class WizardStateHeavyAttack<T> : WizardStateLightAttack<T>
    {
        protected override void Attack()
        {
            Model.HeavyAttack();
            View.PlayTargetAnimation(Model.CurrentWeapon().GetData().HeavyAttack01.EventHash);
            var timer = Model.CurrentWeapon().GetData().HeavyAttack01.Duration;
            Model.SetTimer(timer);
        }

        protected override void CancelAttack()
        {
            base.CancelAttack();
            Model.CancelHeavyAttack();
        }
    }
}