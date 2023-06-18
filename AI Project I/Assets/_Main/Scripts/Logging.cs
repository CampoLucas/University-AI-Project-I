using Conditional = System.Diagnostics.ConditionalAttribute;
using Debug = UnityEngine.Debug;

namespace Game
{
    
    public static class Logging
    {
        [Conditional("UNITY_ENGINE"), 
         Conditional("ENABLE_LOG")]
        public static void Log(object obj)
        {
            Debug.Log($"LOG: {obj}");
        }

        [Conditional("UNITY_ENGINE"),
         Conditional("ENABLE_LOG")]
        public static void LogError(object obj)
        {
            Debug.LogError($"ERROR: {obj}");
        }

        [Conditional("UNITY_ENGINE"),
         Conditional("ENABLE_LOG")]
        public static void LogError(object obj, System.Func<bool> condition)
        {
            if (condition())
                LogError(obj);
        }

        [Conditional("UNITY_ENGINE"),
         Conditional("ENABLE_LOG")]
        public static void LogWarning(object obj)
        {
            Debug.LogWarning($"WARNING: {obj}");
        }

        [Conditional("UNITY_ENGINE"),
         Conditional("ENABLE_LOG")]
        public static void LogWarning(object obj, System.Func<bool> condition)
        {
            if (condition())
                LogWarning(obj);
        }


        [Conditional("UNITY_ENGINE"), Conditional("ENABLE_LOG_DESTROY")]
        public static void LogDestroy(object obj)
        {
            Debug.Log(obj);
        }
    }
}