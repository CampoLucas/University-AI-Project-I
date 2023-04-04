using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "AnimationEvent", menuName = "SO/AnimationEvent", order = 2)]
    public class AnimationEventSO : ScriptableObject
    {
        public string EventName => eventName;
        public float Duration => duration;
        
        [SerializeField] private string eventName;
        [SerializeField] private float duration;
    }
}