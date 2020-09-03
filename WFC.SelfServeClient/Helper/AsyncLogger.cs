using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace VST.SelfServeClient.Helper
{
    public class AsyncLogger
    {
        private TextWriter mLogWriter = null;
        private DateTime mDate;
        private bool isRunning = false;
        private Queue<string> LogQueue = new Queue<string>();
        private Thread writeThread;

        public AsyncLogger()
        {
            this.Write();
        }

        public string LogPath
        {
            get;
            set;
        }

        public string GuidPath
        {
            get;
            set;
        }

        public void Open()
        {
            this.isRunning = true;
        }

        public void Close()
        {
            this.isRunning = false;
        }

        public void BeginWrite(string log)
        {
            this.LogQueue.Enqueue(log);
        }

        private void Write()
        {
            writeThread = new Thread(() => {
                while (this.isRunning)
                {
                    try
                    {
                        while (this.LogQueue.Count > 0)
                        {
                            var log = this.LogQueue.Dequeue();
                            try
                            {
                                if (mLogWriter != null)
                                {
                                    this.CheckCreate();
                                    mLogWriter.WriteLine(log);
                                    mLogWriter.WriteLine();
                                    mLogWriter.Flush();
                                }
                            }
                            catch
                            { }
                        }
                        this.CheckCreate();
                        Thread.Sleep(10);
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

            });
            writeThread.Start();
           
        }


        /// <summary>
        /// 检查日志日期，如果日期改变创建新日志
        /// </summary>
        private void CheckCreate()
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
        private void CreateLog()
        {
            try
            {
                mDate = DateTime.Now;

                FileStream fs = new FileStream(LogPath + mDate.ToString("yyyyMMdd") + (string.IsNullOrEmpty(GuidPath) ? "" : "-" + GuidPath) + ".log", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                fs.Seek(0, SeekOrigin.End);

                //先关闭旧有日志
                mLogWriter?.Close();

                mLogWriter = new StreamWriter(fs);
                mLogWriter.WriteLine("\r\n====================================================================================================================\r\n");
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
            }
        }
    }
}
