

namespace NPatternRecognizer.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    using System.Threading;
    using System.Globalization;

    public enum LogLevel
    {
        None = 0,
        Fatal = 1,
        Error = 2,
        Warn = 4,
        Info = 8,
        Debug = 16,
        Trace = 32,
    }

    public class Logger
    {
        const string BaseFileName = "NPatternRecognizer";
        private string DeclaringType;

        public Logger(Type type)
        {
            if (type != null)
                DeclaringType = type.FullName;
        }

        public static string LogPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "\\" + BaseFileName + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + ".log";

            }
        }

        public static void LogMessage(string message, LogLevel logLevel)
        {
            StreamWriter writer = null;

            try
            {
                FormatMessage(ref message);

                System.Console.WriteLine(message);

                writer = File.AppendText(LogPath);
                writer.WriteLine(message);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (null != writer)
                {
                    writer.Close();
                }
            }

        }

        private static void FormatMessage(ref string message)
        {
            int CurrentProcessId = Process.GetCurrentProcess().Id;
            int CurrentThreadId = Thread.CurrentThread.ManagedThreadId;
            string temp = message;
            string format = "yyyy-MM-dd HH:mm:ss.fff";
            message = String.Format(CultureInfo.InvariantCulture, "{0} [pid:{1}] [tid:{2}] - {3}",
                DateTime.Now.ToString(format, CultureInfo.InvariantCulture),
                CurrentProcessId,
                CurrentThreadId,
                temp
                );
        }

        public static void Info(string format, params object[] args)
        {
            Logger.Info(string.Format(format, args));
        }

        public static void Info(string message)
        {
            Logger.LogMessage(message, LogLevel.Info);
        }

        public static void Error(string message)
        {
            Logger.LogMessage(message, LogLevel.Error);
        }

        public static void Error(string format, params object[] args)
        {
            Logger.Error(string.Format(format, args));
        }
    }
}
