using Colonia.Engine.Utils;
using System.Linq;

namespace Colonia
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args.Contains("--portable")) AppInfo.IsPortable = true;
            }

            App app = new();
            app.Run();
        }
    }
}