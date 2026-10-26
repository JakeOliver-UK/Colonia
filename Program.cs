using System;

namespace Colonia
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("--portable", StringComparison.OrdinalIgnoreCase))
            {
                AppInfo.IsPortable = true;
            }

            using App app = new();
            app.Run();
        }
    }
}