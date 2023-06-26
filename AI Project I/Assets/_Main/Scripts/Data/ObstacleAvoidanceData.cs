using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public class ObstacleAvoidanceData
    {
        public float Range => range;
        public float Angle => angle;
        public int MaxObs => maxObs;
        public float Strength => strength;
        public LayerMask Mask => mask;
    
        [SerializeField] private float range = 0.05f;
        [SerializeField] private float angle = 240f;
        [SerializeField] private int maxObs = 10;
        [SerializeField] private float strength = 2;
        [SerializeField] private LayerMask mask;
    }
}