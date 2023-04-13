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
            Debug.Log(obj);
        }
        
        [Conditional("UNITY_ENGINE"), Conditional("ENABLE_LOG_DESTROY")]
        public static void LogDestroy(object obj)
        {
            Debug.Log(obj);
        }
    }
}