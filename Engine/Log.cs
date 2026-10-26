using System;
using System.IO;

namespace Colonia.Engine
{
    internal static class Log
    {
        public static string DirectoryPath => Path.Combine(AppInfo.AppDataDirectoryPath, "Logs");
        public static string CurrentFilePath => _currentFilePath;
        public static string PreviousFilePath => Path.Combine(DirectoryPath, "previous.log");

        private static string _currentFilePath;

        public static void Write(LogLevel level, string message, bool includeTimestamp)
        {
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

            if (_currentFilePath == null)
            {
                _currentFilePath = Path.Combine(DirectoryPath, "current.log");
                if (File.Exists(_currentFilePath)) File.Move(_currentFilePath, PreviousFilePath, true);
                string header = $"{AppInfo.Name} - v{AppInfo.Version} - {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}{Environment.NewLine}{Environment.NewLine}";
                File.WriteAllText(_currentFilePath, header);
            }

            string logLevel = level == LogLevel.None ? string.Empty : $"{level.ToString().ToUpper()}: ";
            string timestamp = includeTimestamp ? $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}] " : string.Empty;
            string output = $"{timestamp}{logLevel}{message}";
            File.AppendAllText(_currentFilePath, output);
        }

        public static void Write(LogLevel level, string message) => Write(level, message, true);
        public static void Write(string message) => Write(LogLevel.None, message, false);
        public static void WriteLine(LogLevel level, string message, bool includeTimestamp) => Write(level, message + Environment.NewLine, includeTimestamp);
        public static void WriteLine(LogLevel level, string message) => Write(level, message + Environment.NewLine, true);
        public static void WriteLine(string message) => Write(LogLevel.None, message + Environment.NewLine, false);
        public static void WriteLine() => Write(LogLevel.None, Environment.NewLine, false);
    }

    internal enum LogLevel
    {
        None,
        Debug,
        Info,
        Warning,
        Error,
        Critical
    }
}
