using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;

namespace Game.Sheared
{
    public class MyRandoms
    {
        public static float Range(float min, float max)
        {
            return Random.value * (max - min) + min;
        }
        
        public static float Range(int min, int max)
        {
            return Random.value * (max - min) + min;
        }

        public static T Roulette<T>(Dictionary<T, float> items)
        {
            var total = items.Sum(item => item.Value);
            var random = Range(0, total);
            foreach (var item in items)
            {
                if (random < item.Value)
                {
                    return item.Key;
                }
                else
                {
                    random -= item.Value;
                }
            }
            return default(T);
        }
        
        public static void Shuffle<T>(T[] items, Action<T, T> onSwap)
        {
            for (var i = 0; i < items.Length; i++)
            {
                var random = Random.Range(0, items.Length);
                (items[random], items[i]) = (items[i], items[random]);
            }
        }

        public static void Shuffle<T>(List<T> items, Action<T, T> onSwap = null)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var random = Random.Range(0, items.Count);
                onSwap?.Invoke(items[i], items[random]);
                (items[random], items[i]) = (items[i], items[random]);
            }
        }
    }
}