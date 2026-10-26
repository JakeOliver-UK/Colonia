using System;
using System.IO;
using System.Reflection;

namespace Colonia
{
    internal class AppInfo
    {
        public static string Name => "Colonia";
        public static int VersionMajor => 0;
        public static int VersionMinor => 1;
        public static int VersionPatch => 0;
        public static string Version => $"{VersionMajor}.{VersionMinor}.{VersionPatch}";
        public static string Author => "Guinea Game Studios";
        public static string Description => "A colony management simulation sandbox game.";
        public static string GameDirectoryPath => AppContext.BaseDirectory ?? Assembly.GetExecutingAssembly().Location;
        public static string AppDataDirectoryPath => IsPortable ? Path.Combine(GameDirectoryPath, "UserData") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Author, Name);
        public static bool IsPortable { get; set; } = false;
    }
}
