using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Entities
{
    [System.Serializable]
    public class WaitTimer
    {
        private float _currentTime;

        public bool TimerComplete() => _currentTime > 0;
        public void RunTimer() => _currentTime -= Time.deltaTime;
        public void SetTimer(float time) => _currentTime = time;
        public float GetRandomTime(float maxTime) => Random.Range(0, maxTime);

    }
}