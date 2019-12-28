using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace BotdeFumar.Core
{
    public sealed class Logger
    {
        private static bool IsRunning = false;
        private static DateTime Time = DateTime.Now;

        internal static void Write(string Line)
        {
            if (!Logger.IsRunning)
            {
                Console.Write(Line);
            }
        }

        internal static void WriteLine(string Line, Color Color, bool WithTime = true)
        {
            if (!Logger.IsRunning)
            {
                Console.Write((WithTime ? string.Format("[{0}]|", DateTime.Now.ToString("dd/MM/yy HH:mm:ss")) : ""));
                Console.WriteLine(Line, Color);
            }
        }

        internal static void LogException(string logText)
        {
            try
            {
                FileStream fileStream = new FileStream("exceptions.err", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
                {
                    DateTime.Now,
                    ": ",
                    logText,
                    "\r\n\r\n"
                }));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            catch (Exception)
            {
                Logger.WriteLine(DateTime.Now + ": " + logText, Color.Gray);
            }
            Logger.WriteLine("Exception has been saved", Color.Red);
        }

        private static DateTime getStartupTime = DateTime.Now;

        public static string StartupTime()
        {
            return $"{getStartupTime.Year}_{getStartupTime.Month}_{getStartupTime.Day}_{getStartupTime.Hour}_{getStartupTime.Minute}_{getStartupTime.Second}";
        }

        internal static void SaveLog(string logText)
        {
            try
            {
                FileStream fileStream = new FileStream($"Logs\\logs_{StartupTime()}.log", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
                {
                    DateTime.Now,
                    ": ",
                    logText,
                    "\r\n"
                }));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            catch (Exception)
            {
                Logger.WriteLine(DateTime.Now + ": " + logText, Color.Gray);
            }
        }

        internal static void LogCriticalException(string logText)
        {
            try
            {
                FileStream fileStream = new FileStream("criticalexceptions.err", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
                {
                    DateTime.Now,
                    ": ",
                    logText,
                    "\r\n\r\n"
                }));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
                Logger.WriteLine("CRITICAL ERROR LOGGED", Color.Red);
            }
            catch (Exception)
            {
                Logger.WriteLine(DateTime.Now + ": " + logText, Color.Gray);
            }
        }

        internal static void LogCacheError(string logText)
        {
            try
            {
                FileStream fileStream = new FileStream("cacheerror.err", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
                {
                    DateTime.Now,
                    ": ",
                    logText,
                    "\r\n\r\n"
                }));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            catch (Exception)
            {
                Logger.WriteLine(DateTime.Now + ": " + logText, Color.Gray);
            }
            Logger.WriteLine("Critical error saved", Color.Red);
        }

        internal static void LogDDoS(string logText)
        {
            try
            {
                FileStream fileStream = new FileStream("ddos.txt", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
                {
                    DateTime.Now,
                    ": ",
                    logText,
                    "\r\n\r\n"
                }));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            catch
            {
            }
            Logger.WriteLine(DateTime.Now + ": " + logText, Color.Red);
        }

        internal static void LogThreadException(string Exception, string Threadname)
        {
            try
            {
                FileStream fileStream = new FileStream("threaderror.err", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
                {
                    DateTime.Now,
                    ": Error in thread ",
                    Threadname,
                    ": \r\n",
                    Exception,
                    "\r\n\r\n"
                }));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
                Logger.WriteLine("Error in " + Threadname + " caught", Color.Red);
            }
            catch (Exception)
            {
                Logger.WriteLine(DateTime.Now + ": " + Exception, Color.Gray);
            }
        }

        internal static void DisablePrimaryWriting()
        {
            Logger.IsRunning = true;
        }

        internal static void HandleException(string logText)
        {
            throw new NotImplementedException();
        }
    }
}
