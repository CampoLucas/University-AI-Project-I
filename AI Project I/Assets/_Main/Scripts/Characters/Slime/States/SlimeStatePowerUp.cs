namespace Game.Entities.Slime.States
{
    public sealed class SlimeStatePowerUp<T> : SlimeStateBase<T>
    {
        private bool _pass;
        
        public override void Awake()
        {
            base.Awake();
            
            if(Model.HasReachedMaxLevel())
                Tree.Execute();
            
            Model.IncreaseLevel();
            Model.SetTimer(2f);
        }

        public override void Execute()
        {
            base.Execute();
            if (Model.GetTimerComplete())
            {
                Model.RunTimer();
                Model.IncreaseSize();
            }
            else if(Model.HasTargetSize())
            {
                Tree.Execute();
            }
        }

        public override void Sleep()
        {
            base.Sleep();
        }
    }
}