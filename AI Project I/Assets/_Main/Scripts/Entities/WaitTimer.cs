using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Entities
{
    [System.Serializable]
    public class WaitTimer
    {
        private float _currentTime;
        private UpdateMask _updateMask;

        public WaitTimer(UpdateMask updateMask)
        {
            _updateMask = updateMask;
        }

        public bool TimerComplete() => _currentTime > 0;
        public void RunTimer() => _currentTime -= UpdateManager.Instance.LayerDelta(_updateMask);
        public void SetTimer(float time) => _currentTime = time;
        public float GetRandomTime(float maxTime) => Random.Range(0, maxTime);

    }
}