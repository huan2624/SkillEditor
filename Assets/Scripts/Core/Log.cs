using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 用来输出Debug信息
/// </summary>
class Log 
{
    public enum LogType
    {
        Info = 0,
        Warning = 1,
        Error = 2,
        Exception = 3,
    }
    public static bool isDebugMode = true;
    public static string path = Application.persistentDataPath + "/UnityOutLog.txt";
    public static bool launch = true;
    public const string COLOR_START_TAG = "<color={0}>";
    public const string COLOR_END_TAG = "</color>";
    public const string LOG_TYPE_TAG = "[{0}]";
    public const string INFO_DEFAULT_TAG = "Info";
    public const string WARNING_DEFAULT_TAG = "Warning";
    public const string ERROR_DEFAULT_TAG = "Error";
    public const string EXCEPTION_DEFAULT_TAG = "Exception";
    
    public static void Exception(Exception e)
    {
        if ((e == null) || !isDebugMode)
            return;
        Debug.LogException(e);
        WriteToFile(e.ToString(), LogType.Exception);
    }

    /// <summary>
    /// 打印带有默认颜色的log信息
    /// </summary>
    /// <param name="msg"></param>
    public static void Info(string msg)
    {
        Log.Info(null, msg);
    }

    /// <summary>
    /// 打印带有颜色的log信息
    /// </summary>
    /// <param name="msgColor"></param>
    /// <param name="msg"></param>
    public static void Info(ConsoleColor msgColor, string msg)
    {
        Log.Info(null, msgColor, msg);
    }

    /// <summary>
    /// 打印带有Tag的log信息
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="msg"></param>
    public static void Info(string tag, string msg)
    {
        Log.Info(tag, ConsoleColor.Green, msg);
    }

    /// <summary>
    /// 打印带有颜色Tag的log信息
    /// </summary>
    /// <param name="tag">标记</param>
    /// <param name="msgColor">标记颜色</param>
    /// <param name="msg">信息</param>
    public static void Info(string tag, ConsoleColor msgColor, string msg)
    {
        if (string.IsNullOrEmpty(msg) || !isDebugMode)
            return;
        string tagString = string.Format(LOG_TYPE_TAG, (string.IsNullOrEmpty(tag) ? INFO_DEFAULT_TAG : tag));
        string head = string.Format(COLOR_START_TAG, msgColor.ToString()) + tagString + COLOR_END_TAG;
        string info = head + msg;
        Debug.Log(info);
        WriteToFile(info, LogType.Info);
    }

    /// <summary>
    /// 打印带的错误信息
    /// </summary>
    /// <param name="msg"></param>
    public static void Error(string msg)
    {
        Log.Error(null, msg);
    }

    /// <summary>
    /// 打印带有Tag的错误信息
    /// </summary>
    /// <param name="tag">标记</param>
    /// <param name="msg">错误信息</param>
    public static void Error(string tag, string msg)
    {
        if (string.IsNullOrEmpty(msg) || !isDebugMode)
            return;
        string head = string.Format(LOG_TYPE_TAG, (string.IsNullOrEmpty(tag) ? ERROR_DEFAULT_TAG : tag));
        string info = head + msg;
        Debug.LogError(info);
        WriteToFile(info, LogType.Error);
    }

    /// <summary>
    /// 打印警告信息
    /// </summary>
    /// <param name="msg"></param>
    public static void Warning(string msg)
    {
        Log.Warning(null, msg);
    }

    /// <summary>
    /// 打印带有Tag的警告信息
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="msg"></param>
    public static void Warning(string tag, string msg)
    {
        if (string.IsNullOrEmpty(msg) || !isDebugMode)
            return;
        string head = string.Format(LOG_TYPE_TAG, (string.IsNullOrEmpty(tag) ? WARNING_DEFAULT_TAG : tag));
        string info = head + msg;
        Debug.LogError(info);
        WriteToFile(info, LogType.Warning);
    }

    protected static void WriteToFile(string msg, LogType type = LogType.Info)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if(launch)
        {
            ResetFile();
            launch = false;
        }

        DateTime date = DateTime.Now;
        string timeStr = date.ToString("yyyy-MM-dd HH:mm:ss");

        using(StreamWriter writer = new StreamWriter(path, true, Encoding.UTF8))
        {
            writer.WriteLine(type.ToString() +": "+ msg + "  " + timeStr);
        }
#endif
    }

    protected static void ResetFile()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
       if(System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        } 
#endif
    }
}
