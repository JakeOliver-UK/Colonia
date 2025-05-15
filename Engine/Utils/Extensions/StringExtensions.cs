using System;
using System.Security.Cryptography;
using System.Text;

namespace Colonia.Engine.Utils.Extensions
{
    internal static class StringExtensions
    {
        public static int ToSeed(this string str) => BitConverter.ToInt32(SHA256.HashData(Encoding.ASCII.GetBytes(str)), 0);
    }
}
