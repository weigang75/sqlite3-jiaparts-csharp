using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;

[assembly: log4net.Config.XmlConfigurator()]
namespace Jiaparts.Common.Log
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Logger
    {
        private static ILog m_infologger = null;
        private static ILog m_errlogger = null;

        private static object loggers_lock = new object();

        private static Dictionary<Type, ILoggerSupport> loggers = 
            new Dictionary<Type, ILoggerSupport>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public static void AddLogger(ILoggerSupport logger)
        {
            if (logger == null)
                return;
            Type type = logger.GetType();

            lock (loggers_lock)
            {  
                if (loggers.ContainsKey(type))
                    return;

                loggers.Add(type, logger);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerType"></param>
        public static void RemoveLogger(Type loggerType)
        {
            lock (loggers_lock)
            {
                if (loggers.ContainsKey(loggerType))
                {
                    loggers.Remove(loggerType);
                }                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public static void RemoveLogger(ILoggerSupport logger)
        {
            if (logger == null)
                return;
            Type type = logger.GetType();

            RemoveLogger(type);
        }

        private static ILog logger
        {
            get
            {
                if (m_infologger == null)
                    m_infologger = LogManager.GetLogger("InfoLog");
                return m_infologger;                
            }
        }

        private static ILog errlogger
        {
            get
            {
                if (m_errlogger == null)
                    m_errlogger = LogManager.GetLogger("ErrorLog");

                return m_errlogger;
            }
        }

        public enum LogType
        {
            Debug = 0,
            Info = 1,
            Warn = 2,
            Error = 3,
            Fatal = 4
        }

        /// <summary>
        /// 上报的帐号（登录后要进行赋值，注销后要进行置空）
        /// </summary>
        public static string ReportUserName = "";


        private static Dictionary<string, int> errors = new Dictionary<string, int>();

        /// <summary>
        /// 上报错误日志到服务端
        /// </summary>
        /// <param name="logCategory">日志类型</param>
        /// <param name="logType">日志级别</param>
        /// <param name="message">日志消息</param>
        public static void ReportErrorLog(string logCategory, LogType logType, string message)
        {
            if(String.IsNullOrEmpty(message))
                return;

            if (errors.ContainsKey(message))
                return;

            lock (errors)
            {
                if (errors.ContainsKey(message))
                    return;

                errors.Add(message, 0);
            }
            Assembly assembly = Assembly.GetEntryAssembly();
            if (logCategory.Length > 10)
            {
                // 日志类型
                logCategory = logCategory.Substring(0, 10);
            }
            // 日志级别
            int logLevel = (int)logType;
            // 客户端版本
            string clientVer = assembly.GetName().Version.ToString();

            //CommonApiService service = HttpApiService.GetService<CommonApiService>();
            //service.ExceptionLog(ReportUserName, clientVer, logCategory, logLevel, message, null, null);
            /*    
== 数据库 ==
PK
客户端帐号  （未登录情况下，可以为空）
客户端IP ？
日志类型 varchar(10) 保留（可以为空）
日志级别 int  1:Info 2:Warn 3:Error 4:Fatal
日志消息 text 内容可能会比较多所以用 text
记录时间 
客户端版本 varchar(15)

             
== 接口参数 == 
客户端帐号
客户端版本
日志类型              
日志级别             
日志消息    
             */


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        private static void WriteLog(LogType logType, string message)
        {
            if (loggers.Count == 0)
                return;

            foreach (Type key in loggers.Keys)
            {
                ILoggerSupport logger = loggers[key];
                switch (logType)
                {
                    case LogType.Debug:
                        logger.Debug(message);
                        break;
                    case LogType.Info:
                        logger.Info(message);
                        break;
                    case LogType.Warn:
                        logger.Warn(message);
                        break;
                    case LogType.Error:
                        logger.Error(message);
                        //ReportErrorLog("Logger",logType, message);//暂时你记录Error，否则容易死循环
                        break;
                    case LogType.Fatal:
                        logger.Fatal(message);
                        ReportErrorLog("Logger", logType, message);
                        break;
                    default:
                        break;
                }
            }            
        }

   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        private static void WriteLog(LogType logType, string message, Exception ex)
        {
            if (loggers.Count == 0)
                return;

            foreach (Type key in loggers.Keys)
            {
                ILoggerSupport logger = loggers[key];
                switch (logType)
                {
                    case LogType.Debug:
                        logger.Debug(message, ex);
                        break;
                    case LogType.Info:
                        logger.Info(message, ex);
                        break;
                    case LogType.Warn:
                        logger.Warn(message, ex);
                        break;
                    case LogType.Error:
                        logger.Error(message, ex);
                        break;
                    case LogType.Fatal:
                        logger.Fatal(message, ex);
                        break;
                    default:
                        break;
                }
            }  
        }

        /// <summary>
        /// 配置日志信息
        /// </summary>
        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static bool IsDebugEnabled
        {
            get
            {
                return logger.IsDebugEnabled;
            }
        }

        public static bool IsInfoEnabled
        {
            get
            {
                return logger.IsInfoEnabled;
            }
        }

        public static bool IsWarnEnabled
        {
            get
            {
                return logger.IsWarnEnabled;
            }
        }

        public static bool IsErrorEnabled
        {
            get
            {
                return errlogger.IsErrorEnabled;
            }
        }

        public static bool IsFatalEnabled
        {
            get
            {
                return errlogger.IsFatalEnabled;
            }
        }

        public static void DebugFormat(String message, params Object[] args)
        {
            string logMsg = String.Format(message, args);
            logger.Debug(logMsg);
            WriteLog(LogType.Debug, logMsg);
        }

        public static void InfoFormat(String message, params Object[] args)
        {
            string logMsg = String.Format(message, args);
            logger.Info(logMsg);
            WriteLog(LogType.Info, logMsg);
        }

        public static void WarnFormat(String message, params Object[] args)
        {
            string logMsg = String.Format(message, args);
            logger.Warn(logMsg);
            WriteLog(LogType.Warn, logMsg);
        }

        public static void ErrorFormat(String message, params Object[] args)
        {
            string logMsg = String.Format(message, args);
            errlogger.Error(logMsg);
            WriteLog(LogType.Error, logMsg);
        }

        public static void ErrorFormat(Exception ex, String message, params Object[] args)
        {
            string logMsg = String.Format(message, args);
            errlogger.Error(logMsg, ex);
            WriteLog(LogType.Error, logMsg, ex);
        }

        public static void FatalFormat(String message, params Object[] args)
        {
            string logMsg = String.Format(message, args);
            errlogger.Fatal(logMsg);
            WriteLog(LogType.Fatal, logMsg);
        }

        public static void Debug(String message)
        {
            logger.Debug(message);
            WriteLog(LogType.Debug, message);
        }

        public static void Info(String message)
        {
            logger.Info(message);
            WriteLog(LogType.Info, message);
        }

        public static void Warn(String message)
        {
            logger.Warn(message);
            WriteLog(LogType.Warn, message);
        }

        public static void Error(String message)
        {
            errlogger.Error(message);
            WriteLog(LogType.Error, message);
        }

        public static void Fatal(String message)
        {
            errlogger.Fatal(message);
            WriteLog(LogType.Fatal, message);
        }


        public static void Debug(String message, Exception ex)
        {
            logger.Debug(message, ex);
            WriteLog(LogType.Debug, message, ex);
        }

        public static void Info(String message, Exception ex)
        {
            logger.Info(message, ex);
            WriteLog(LogType.Info, message, ex);
        }

        public static void Warn(String message, Exception ex)
        {
            logger.Warn(message, ex);
            WriteLog(LogType.Warn, message, ex);
        }

        public static void Error(String message, Exception ex)
        {
            errlogger.Error(message, ex);
            WriteLog(LogType.Error, message, ex);
        }

        public static void Fatal(String message, Exception ex)
        {
            errlogger.Fatal(message, ex);
            WriteLog(LogType.Fatal, message, ex);
        }
    }
}
