
namespace VST.SelfServeClient.Helper
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class Logger
    {
        private static AsyncLogger mLog = null;

        private static string guid = string.Empty;
        /// <summary>
        /// Trace计数
        /// </summary>
        private static int mTraceIndex = 0;
        /// <summary>
        /// Error计数
        /// </summary>
        private static int mErrorIndex = 0;
        /// <summary>
        /// Debug计数
        /// </summary>
        private static int mDebugIndex = 0;
        /// <summary>
        /// warn计数
        /// </summary>
        private static int mWarnIndex = 0;
        /// <summary>
        /// 路径
        /// </summary>
        private static string mLogPath = System.Windows.Forms.Application.StartupPath + @"\Log\";
        /// <summary>
        /// 文本写入类
        /// </summary>
        private static TextWriter mLogWriter = null;
        /// <summary>
        /// 保留日志天数
        /// </summary>
        private static int mSaveDays = 7;
        /// <summary>
        /// 同步上下文
        /// </summary>
        private static SynchronizationContext mContext = null;

        /// <summary>
        /// 当前日期
        /// </summary>
        private static DateTime mDate;

        /// <summary>
        /// 设置/获取能够写入文件的日志最低级别
        /// </summary>
        public static LogLevel CanWriteLeval { get; set; } = LogLevel.Error;

        /// <summary>
        /// 设置能够写入文件的日志最低级别
        /// </summary>
        /// <param name="level"></param>
        public static void SetCanWriteLeval(string level)
        {
            switch (level)
            {
                case "debug":
                    CanWriteLeval = LogLevel.Debug;
                    break;
                case "trace": CanWriteLeval = LogLevel.Trace; break;
                case "warn": CanWriteLeval = LogLevel.Warn; break;
                case "error": CanWriteLeval = LogLevel.Error; break;
                default: CanWriteLeval = LogLevel.Error; break;
            }
        }

        /// <summary>
        /// 写入trace日志
        /// </summary>
        /// <param name="message"></param>
        public static void Trace(string message)
        {
            Write(LogLevel.Trace, message);
        }

        /// <summary>
        /// 写入trace日志
        /// </summary>
        public static void Trace(string format, params object[] vargs)
        {
            Write(LogLevel.Trace, format, vargs);
        }

        /// <summary>
        /// 写入error日志
        /// </summary>
        public static void Error(string message, [CallerMemberName] string callerName = "")
        {
            Write(LogLevel.Error, callerName + ":" + message);
        }


        /// <summary>
        /// 写入error日志
        /// </summary>
        public static void Error(string format, params object[] vargs)
        {
            Write(LogLevel.Error, format, vargs);
        }


        /// <summary>
        /// 写入debug日志
        /// </summary>
        public static void Debug(string message)
        {
            Write(LogLevel.Debug, message);
        }

        /// <summary>
        /// 写入debug日志
        /// </summary>
        public static void Debug(string format, params object[] vargs)
        {
            Write(LogLevel.Debug, format, vargs);
        }

        /// <summary>
        /// 写入warn日志
        /// </summary>
        public static void Warn(string message, [CallerMemberName] string callerName = "")
        {
            Write(LogLevel.Warn, callerName + ":" + message);
        }

        /// <summary>
        /// 写入warn日志
        /// </summary>
        public static void Warn(string format, params object[] vargs)
        {
            Write(LogLevel.Warn, format, vargs);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        public static void Write(LogLevel leval, string format, params object[] vargs)
        {
            Write(leval, string.Format(format, vargs));
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        public static void Write(LogLevel leval, string message)
        {
            try
            {
                string levString = "";
                int indx = 0;
                switch (leval)
                {
                    case LogLevel.Debug:
                        levString = "debug"; indx = mDebugIndex++;
                        ConsoleLogHelper.WriteLineDebug(message);
                        break;
                    case LogLevel.Trace:
                        levString = "trace"; indx = mTraceIndex++;
                        ConsoleLogHelper.WriteLineDebug(message);
                        break;
                    case LogLevel.Warn:
                        levString = "warn"; indx = mWarnIndex++;
                        ConsoleLogHelper.WriteLineWarn(message);
                        break;
                    case LogLevel.Error:
                        levString = "error"; indx = mErrorIndex++;
                        ConsoleLogHelper.WriteLineError(message);
                        break;
                }
                string msg = String.Format("{0} {1}[{2}]: {3}", GetDateNowMsString(), levString, indx, message);

                System.Diagnostics.Trace.WriteLine(msg, levString);
                if (leval >= CanWriteLeval)
                {
                    Write(msg);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        private static string GetDateNowMsString()
        {
            DateTime dt = DateTime.Now;
            return String.Format("{0}.{1}", dt.ToString(), dt.Millisecond);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg">写入日志内容</param>
        private static void Write(string msg)
        {

            try
            {
                mLog.BeginWrite(msg);

                //mContext.Send((SendOrPostCallback)delegate
                //{
                //    try
                //    {
                //        if (mLogWriter != null)
                //        {
                //            CheckCreate();
                //            mLogWriter.WriteLine(msg);
                //            mLogWriter.WriteLine();
                //            mLogWriter.Flush();
                //        }
                //    }
                //    catch
                //    { }
                //}, null);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    System.Diagnostics.Trace.WriteLine(e.InnerException.Message);
                    System.Diagnostics.Trace.WriteLine(e.InnerException.StackTrace);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                    System.Diagnostics.Trace.WriteLine(e.StackTrace);
                }
            }
        }

        /// <summary>
        /// 检查日志日期，如果日期改变创建新日志
        /// </summary>
        private static void CheckCreate()
        {
            DateTime dt = DateTime.Now;
            if (dt.Date == mDate.Date)
            {
                return;
            }
            CreateLog();
        }

        /// <summary>
        /// 创建日志
        /// </summary>
        private static void CreateLog()
        {
            try
            {
                mDate = DateTime.Now;

                FileStream fs = new FileStream(mLogPath + mDate.ToString("yyyyMMdd") + (string.IsNullOrEmpty(guid) ? "" : "-" + guid) + ".log", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                fs.Seek(0, SeekOrigin.End);

                //先关闭旧有日志
                mLogWriter?.Close();

                mLogWriter = new StreamWriter(fs);
                mLogWriter.WriteLine("\r\n====================================================================================================================\r\n");

                RemoveOldLog();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 删除过期日志
        /// </summary>
        private static void RemoveOldLog()
        {
            DirectoryInfo di = new DirectoryInfo(mLogPath);
            foreach (var item in di.GetFiles())
            {
                TimeSpan ts = DateTime.Now - item.CreationTime;
                if (ts.Days >= mSaveDays)
                {
                    var fileDelete = item.FullName;
                    File.Delete(fileDelete);
                }
            }
        }

        public static void Open(string gid)
        {
            try
            {
                guid = gid;
                if (!Directory.Exists(mLogPath))
                    Directory.CreateDirectory(mLogPath);

                mContext = SynchronizationContext.Current;

                // CreateLog();
                mLog = new AsyncLogger();
                mLog.LogPath = mLogPath;
                mLog.GuidPath = guid;

                mLog.Open();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 打开日志
        /// </summary>
        public static void Open()
        {
            Open(string.Empty);
        }

        /// <summary>
        /// 关闭日志
        /// </summary>
        public static void Close()
        {
            if (mLogWriter == null)
            {
                return;
            }
            mLogWriter.Close();
            mLogWriter = null;

            if (mLog != null)
            {
                mLog.Close();
            }
        }
    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试
        /// </summary>
        Debug,
        /// <summary>
        /// 消息
        /// </summary>
        Trace,
        /// <summary>
        /// 警告
        /// </summary>
        Warn,
        /// <summary>
        /// 错误
        /// </summary>
        Error
    }

}
