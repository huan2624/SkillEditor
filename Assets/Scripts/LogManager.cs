using UnityEngine;
using System.Collections;

public class LogManager {

    public static void Log(System.Object message)
    {
        Debug.Log(message);
    }

    public static void LogError(System.Object message)
    {
        Debug.LogError(message);
    }
}
