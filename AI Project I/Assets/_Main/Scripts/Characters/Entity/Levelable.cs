namespace Game.Entities
{
    public class Levelable
    {
        private readonly int _maxLevel;
        
        public Levelable(int initLevel, int maxLevel)
        {
            CurrentLevel = initLevel;
            _maxLevel = maxLevel;
        }
        public int CurrentLevel { get; private set; }

        public void IncreaseLevel()
        {
            CurrentLevel++;
        }

        public bool HasReachedMaxLevel()
        {
            return CurrentLevel >= _maxLevel;
        }
    }
}