using System;

namespace EngineLite.Util
{
    public static class Logger
    {
        public static void Log(object message) => WriteLog(ConsoleColor.White, "LOG", message?.ToString());

        public static void Log(object sender, object message) => WriteLog(ConsoleColor.White, sender?.GetType().Name ?? "Static", message?.ToString());

        public static void Error(object message) => WriteLog(ConsoleColor.Red, "ERROR", message?.ToString());

        public static void Error(object sender, object message) => WriteLog(ConsoleColor.Red, sender?.GetType().Name ?? "Static", message?.ToString());

        public static void Warning(object message) => WriteLog(ConsoleColor.Yellow, "WARNING", message?.ToString());

        public static void Warning(object sender, object message) => WriteLog(ConsoleColor.Yellow, sender?.GetType().Name ?? "Static", message?.ToString());

        private static readonly object LogLock = new object();

        private static void WriteLog(ConsoleColor color, string prefix, string message)
        {
            lock (LogLock)
            {
                Console.ForegroundColor = color;
                Console.Write($"[{prefix}] ");
                Console.ResetColor();
                Console.WriteLine(message);
            }
        }
    }
}
