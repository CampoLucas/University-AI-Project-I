using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    
    public static class Logging
    {
        [Conditional("ENABLE_LOG")]
        public static void Log(object obj)
        {
            Debug.Log($"LOG: {obj}");
        }

        [Conditional("ENABLE_LOG")]
        public static void LogError(object obj)
        {
            Debug.LogError($"ERROR: {obj}");
        }

        [Conditional("ENABLE_LOG")]
        public static void LogError(object obj, System.Func<bool> condition)
        {
            if (condition())
                LogError(obj);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogWarning(object obj)
        {
            Debug.LogWarning($"WARNING: {obj}");
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogWarning(object obj, System.Func<bool> condition)
        {
            if (condition())
                LogWarning(obj);
        }


        [System.Diagnostics.Conditional("ENABLE_LOG_DESTROY")]
        public static void LogDestroy(object obj)
        {
            Debug.Log(obj);
        }
        
        [System.Diagnostics.Conditional("ENABLE_LOG_PATHFINDER")]
        public static void LogPathfinder(object obj)
        {
            Debug.Log($"LOG_PATHFINDER: {obj}");
        }
        
    }

    public enum LoggingType
    {
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }
    
    public static class LoggingTwo
    {
        private const string definedSymbol = "ENABLE_LOG"; //Default = MY_LOGGER

        private const string infoColorA = "#00E01A";
        private const string infoColorB = "#8BFA5F";
        
        private const string debugColorA = "#3DBAB8";
        private const string debugColorB = "#52FAF7";
        
        private const string warningColorA = "#E0D900";
        private const string warningColorB = "yellow";
        
        private const string errorColorA = "orange";
        private const string errorColorB = "#BD5E00";
        
        private const string criticalColorA = "red";
        private const string criticalColorB = "#BD2A00";
        
        [Conditional(definedSymbol)]
        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log($"{GetSourceInformation()} <color=lightblue><b>INFO:</b> {message}</color>");
#endif
        }
        
        [Conditional(definedSymbol)]
        public static void Log(object message, Object context)
        {
#if UNITY_EDITOR
            Debug.Log($"{GetSourceInformation()} <color=lightblue><b>INFO:</b> {message}</color>", context);
#endif
        }
        
        [Conditional(definedSymbol)]
        public static void Log(object message, LoggingType type = LoggingType.Info, Object context = null)
        {
#if UNITY_EDITOR
            if (!Enum.IsDefined(typeof(LoggingType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(LoggingType));
            
            var color = "light";
            switch (type)
            {
                case LoggingType.Debug:
                    color = IsDarkMode() ? infoColorA : infoColorB;
                    
                    Debug.Log($"{GetSourceInformation()} <color={color}><b>DEBUG:</b> {message}</color>", context);
                    break;
                case LoggingType.Info:
                    color = IsDarkMode() ? debugColorA : debugColorB;
                    Debug.Log($"{GetSourceInformation()} <color={color}><b>INFO:</b> {message}</color>", context);
                    break;
                case LoggingType.Warning:
                    color = IsDarkMode() ? warningColorA : warningColorB;
                    Debug.LogWarning($"{GetSourceInformation()} <color={color}><b>WARNING:</b> {message}</color>", context);
                    break;
                case LoggingType.Error:
                    color = IsDarkMode() ? errorColorA : errorColorB;
                    Debug.LogError($"{GetSourceInformation()} <color={color}><b>ERROR:</b> {message}</color>", context);
                    break;
                case LoggingType.Critical:
                    color = IsDarkMode() ? criticalColorA : criticalColorB;
                    Debug.LogError($"{GetSourceInformation()} <color={color}><b>CRITICAL:</b> {message}</color>", context);
                    Debug.Break();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
#endif
        }

        [Conditional(definedSymbol)]
        public static void Log(object message, Func<bool> condition, LoggingType type = LoggingType.Info, Object context = null)
        {
#if UNITY_EDITOR
            if (!condition()) return;
            Log(message, type, context);
#endif
        }
        
        private static void GetLogInfo(out string timestamp, out string info)
        {
            timestamp = DateTime.Now.ToString("yyyy-MM-dd");
            info = GetSourceInformation();
        }

        private static string GetSourceInformation()
        {
            var stackTrace = new StackTrace();
            var loggingTwoType = typeof(LoggingTwo);

            // Find the first frame that doesn't belong to the LoggingTwo class
            for (var frameIndex = 0; frameIndex < stackTrace.FrameCount; frameIndex++)
            {
                var frame = stackTrace.GetFrame(frameIndex);
                var method = frame.GetMethod();
                var declaringType = method.DeclaringType;

                if (declaringType != loggingTwoType)
                {
                    var className = declaringType?.Name ?? "Un-Class";
                    var methodName = method?.Name ?? "Un-Method";
                    var color = IsDarkMode() ? "white" : "black";
                    return $"<b><color={color}>[{className}.{methodName}()]</color></b>";
                }
            }

            return ""; // If no suitable frame is found, return an empty string
        }

        private static bool IsDarkMode()
        {
#if UNITY_EDITOR
            return EditorGUIUtility.isProSkin;
#else
            return false;
#endif
        }
    }
}
