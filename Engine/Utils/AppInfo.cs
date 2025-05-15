using System;
using System.IO;
using System.Reflection;

namespace Colonia.Engine.Utils
{
    internal static class AppInfo
    {
        public static string Name => "Colonia";
        public static int VersionMajor => 0;
        public static int VersionMinor => 1;
        public static int VersionBuild => 0;
        public static string Version => $"{VersionMajor}.{VersionMinor}.{VersionBuild}";
        public static string Author => "Guinea Game Studios";
        public static string ApplicationDirectoryPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static bool IsPortable { get; set; } = false;
        public static string LocalAppDataDirectoryPath => IsPortable ? Path.Combine(ApplicationDirectoryPath, "Config") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Author, Name);
    }
}
