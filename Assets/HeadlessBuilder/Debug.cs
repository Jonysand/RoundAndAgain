/* 
 * Headless Builder
 * (c) Salty Devs, 2020
 * 
 * Please do not publish or pirate this code.
 * We worked really hard to make it.
 * 
 */


#if (HEADLESS && HEADLESS_STRIPLOGGING)
#undef ALLOW_LOGGING_INTERNAL
#else
#define ALLOW_LOGGING_INTERNAL
#endif

using UnityEngine;
using System;


public static class Debug
{
    public static bool isDebugBuild
    {
        get { return UnityEngine.Debug.isDebugBuild; }
    }

    public static bool developerConsoleVisible
    {
        get { return UnityEngine.Debug.developerConsoleVisible; }
        set { UnityEngine.Debug.developerConsoleVisible = value; }
    }

#if UNITY_2017_1_OR_NEWER
    public static ILogger unityLogger
    {
        get { return UnityEngine.Debug.unityLogger; }
    }
#endif


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Assert(bool condition)
    {
        UnityEngine.Debug.Assert(condition);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Assert(bool condition, UnityEngine.Object context)
    {
        UnityEngine.Debug.Assert(condition, context);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Assert(bool condition, object message)
    {
        UnityEngine.Debug.Assert(condition, message);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Assert(bool condition, object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Assert(condition, message, context);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void AssertFormat(bool condition, string format, params object[] args)
    {
        UnityEngine.Debug.AssertFormat(condition, format, args);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void AssertFormat(bool condition, UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.AssertFormat(condition, context, format, args);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Break()
    {
        UnityEngine.Debug.Break();
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void ClearDeveloperConsole()
    {
        UnityEngine.Debug.ClearDeveloperConsole();
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }


    //--[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    //--[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }


    //--[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogAssertion(object message)
    {
        UnityEngine.Debug.LogAssertion(message);
    }

    //--[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogAssertion(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogAssertion(message, context);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(context, format, args);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogException(Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogException(Exception exception, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogException(exception, context);
    }


    //--[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    //--[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(context, format, args);
    }

#if UNITY_2019_1_OR_NEWER
    [System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogFormat(LogType logType, LogOption logOptions, UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(logType, logOptions, context, format, args);
    }
#endif


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }


    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }

    //[System.Diagnostics.Conditional("ALLOW_LOGGING_INTERNAL")]
    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(context, format, args);
    }

}