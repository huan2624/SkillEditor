using UnityEngine;
using System.Collections;
using System;

public class TimeUtil {

    /// <summary>
    /// 时间戳转为C#格式时间
    /// </summary>
    /// <param name="timeStamp">时间值</param>
    /// <param name="isSecond">是否是秒否则为毫秒</param>
    /// <returns></returns>
    public static DateTime StampToDateTime(long timeStamp, bool isSecond = false)
    {
        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long tempTime = isSecond ? timeStamp * 1000 : timeStamp;
        TimeSpan toNow = TimeSpan.FromMilliseconds(tempTime);

        return dateTimeStart.Add(toNow);
    }

    // DateTime时间格式转换为Unix时间戳格式,返回豪秒
    public static long DateTimeToStamp(System.DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (long)(time - startTime).TotalMilliseconds;
    }

    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <returns>返回毫秒</returns>
    public static long GetCurrentTimeStamp()
    {
        DateTime now = DateTime.Now;
        return DateTimeToStamp(now);
    }

    /// <summary>
    /// 判断给定时间戳是否是今天
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static bool IsToday(long timeStamp)
    {
        DateTime today = DateTime.Today;
        DateTime targetDate = StampToDateTime(timeStamp);
        if (today.Year == targetDate.Year && today.Month == targetDate.Month && today.Day == targetDate.Day)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 判断时间戳是否在指定日期之间
    /// </summary>
    /// <param name="timeStamp">毫秒</param>
    /// <param name="startTime">开始日期</param>
    /// <param name="endTime">结束日期</param>
    /// <returns></returns>
    public bool IsInTimeInterval(long timeStamp, DateTime startTime, DateTime endTime)
    {
        double startMill = startTime.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds;
        double endMill = endTime.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds;
        //判断时间段开始时间是否小于时间段结束时间，如果不是就交换
        if (startMill > endMill)
        {
            double tempTime = startMill;
            startMill = endMill;
            endMill = tempTime;
        }
        if (timeStamp > startMill && timeStamp < endMill)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 将时间数转换成日期格式
    /// </summary>
    /// <param name="durationTime">秒</param>
    /// <returns></returns>
    public static string ConverTimeStampToDateString(int durationTime)
    {
        TimeSpan ts = TimeSpan.FromSeconds(durationTime);
        string str = "";
        if (ts.Hours > 0)
        {
            str = ts.Hours.ToString() + "小时 " + ts.Minutes.ToString() + "分钟 " + ts.Seconds + "秒";
        }
        if (ts.Hours == 0 && ts.Minutes > 0)
        {
            str = ts.Minutes.ToString() + "分钟 " + ts.Seconds + "秒";
        }
        if (ts.Hours == 0 && ts.Minutes == 0)
        {
            str = ts.Seconds + "秒";
        }
        return str;
    }

    /// <summary>
    /// 获取此时距离某个时间戳的时间字符串
    /// </summary>
    /// <param name="timeStamp">时间戳（毫秒）</param>
    /// <returns></returns>
    public static string GetIntervalTimeString(long timeStamp)
    {
        long curTimeStamp = GetCurrentTimeStamp();
        int interval = (int)Mathf.Abs((timeStamp - curTimeStamp)/1000);
        return ConverTimeStampToDateString(interval);
    }
}
