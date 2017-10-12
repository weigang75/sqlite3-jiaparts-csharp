using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jiaparts.Common.Log;
using System.Runtime.InteropServices;

namespace Jiaparts.Common.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class DateTimeUtil
    {

        /// <summary>
        /// 为了避免客户端的时间不正确，不要直接使用DateTime.Now，今后可能需要从服务器端获取时间，保证时间同步。
        /// </summary>
        public static DateTime CurrentTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 获取友好的日期名称
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static string GetFriendlyName(long ticks)
        {
            return GetFriendlyName(new DateTime(ticks));
        }

        /// <summary>
        /// 获取友好的日期名称
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetFriendlyName(DateTime dt)
        {
            return GetFriendlyName(dt, "yyyy-MM-dd", null);
        }

        public static readonly string[] WEEK_DAYS = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };

        /// <summary>
        /// 获取友好的日期名称
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dateFormat"></param>
        /// <param name="timeFormat"></param>
        /// <returns></returns>
        public static string GetFriendlyName(DateTime dt, string dateFormat, string timeFormat)
        {
            if (String.IsNullOrEmpty(dateFormat))
                dateFormat = "yyyy-MM-dd";

            if (String.IsNullOrEmpty(timeFormat))
                timeFormat = "HH:mm";

            if (CurrentTime.Date == dt.Date)
            {
                return "今天 " + dt.ToString(timeFormat);
            }
            else if (CurrentTime.AddDays(-1).Date == dt.Date)
            {
                return "昨天 " + dt.ToString(timeFormat);
            }
            else if (CurrentTime.AddDays(1).Date == dt.Date)
            {
                return "明天 " + dt.ToString(timeFormat);
            }
            else if (CurrentTime.AddDays(-7).Date <= dt.Date)
            {
                int n = (int)dt.DayOfWeek;
                return WEEK_DAYS[n] + " " + dt.ToString(timeFormat);
            }
            else
            {
                return dt.ToString(dateFormat + " " + timeFormat);
            }
        }

        /// <summary>
        /// 获取友好的日期名称（某年某月某日 星期某 某时某分
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetFriendlyName2(DateTime dt)
        {
            string fridenlydt = dt.ToString("yyyy年MM月dd日 ddddd HH时mm分");
            return fridenlydt;
        }

        /// <summary>
        /// 当前的 Unix Timestamp
        /// </summary>
        public static long NowUnixTimestamp
        {
            get
            {
                return ToUnixTs(CurrentTime);
            }
        }

        #region 时间戳转换方法
        /// <summary>
        /// 时间戳转换方法
        /// DateTime -> Unix Timestamp
        /// </summary>
        /// <param name="time">需要转换的时间</param>
        public static long ToUnixTs(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long a = (long)(time - startTime).TotalSeconds;
            return a;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// Unix Timestamp -> DateTime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTsToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// Unix Timestamp -> DateTime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTsToDateTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = timeStamp * 10000000;
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 日期类型转为格式化的字符类型
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static String UnixDataTimeToString(DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd HH:mm:ss");
        }

        #endregion

        [DllImport("Kernel32.dll")]
        private static extern bool SetLocalTime(ref SystemTime sysTime);
        /// <summary>
        /// 设置客户端的系统时间
        /// </summary>
        /// <param name="timestr"></param>
        /// <returns></returns>
        public static bool SetClientTime(string timestr)
        {
            DateTime sourceDt = Convert.ToDateTime(timestr);
            return SetClientTime(sourceDt);
        }

        /// <summary>
        /// 设置客户端的系统时间
        /// </summary>
        /// <param name="sourceDt"></param>
        /// <returns></returns>
        public static bool SetClientTime(DateTime sourceDt)
        {
            SystemTime sysTime = new SystemTime();

            bool flag = false;

            DateTime clientDt = DateTime.Now;

            TimeSpan ts = sourceDt - clientDt;
            // 
            if (Math.Abs(ts.TotalMinutes) < 10)
            {
                return false;
            }

            sysTime.wYear = Convert.ToUInt16(sourceDt.Year);
            sysTime.wMonth = Convert.ToUInt16(sourceDt.Month);
            sysTime.wDay = Convert.ToUInt16(sourceDt.Day);
            sysTime.wHour = Convert.ToUInt16(sourceDt.Hour);
            sysTime.wMinute = Convert.ToUInt16(sourceDt.Minute);
            sysTime.wSecond = Convert.ToUInt16(sourceDt.Second);
            try
            {
                flag = SetLocalTime(ref sysTime);
            }
            catch (Exception e)
            {
                Logger.Fatal("设置客户端时间失败。", e);
                //Console.WriteLine("SetSystemDateTime函数执行异常" + e.Message);
                throw new Exception("本机日期时间不正确，请手动设置。");
            }
            return flag;
        }
        ///// <summary>
        ///// 把时间段转换成天小时分钟（XX天XX小时XX分钟）
        ///// </summary>
        ///// <param name="timeLong"></param>
        ///// <returns></returns>
        //public static string SetDelayedSpike(TimeSpan timeSpan)
        //{
        //    if (timeSpan.ConvertToInt() < 0)
        //    {
        //        return "0分钟";
        //    }
        //    string strTime = null; 
        //    if (timeSpan.Days != 0)
        //    {
        //        strTime += timeSpan.Days + "天";
        //    }
        //    if (timeSpan.Hours != 0)
        //    {
        //        strTime += timeSpan.Hours + "小时";
        //    }
        //    if (timeSpan.Minutes != 0)
        //    {
        //        strTime += timeSpan.Minutes + "分钟";
        //    }
        //    //if (timeSpan.Minutes != 0)
        //    //{
        //    //    strTime += timeSpan.Seconds + "秒";
        //    //}
        //    return strTime;
        //}
    }







    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMiliseconds;
    }
}
