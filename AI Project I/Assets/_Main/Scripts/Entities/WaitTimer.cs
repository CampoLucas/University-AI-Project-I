using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Entities
{
    public class WaitTimer : MonoBehaviour
    {
        [field: SerializeField] public float MaxTime { get; private set; }
        public float CurrentTime { get; private set; }

        public void RunTimer() => CurrentTime -= Time.deltaTime;
        public void SetTimer(float time) => CurrentTime = time;
        public float GetRandomTime() => Random.Range(0, MaxTime);

    }
}