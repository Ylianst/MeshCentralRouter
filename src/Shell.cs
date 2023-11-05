using System;
using System.Diagnostics;

namespace MeshCentralRouter
{
    internal static class Shell
    {
        public static void Start(string fileName)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                UseShellExecute = true,
            });
        }
    }
}
