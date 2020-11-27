/*
Copyright 2009-2017 Intel Corporation

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.IO;
using System.Windows.Forms;

namespace MeshCentralRouter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            // Setup settings & visual style
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Properties.Settings.Default.Upgrade();

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(ExceptionSink);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionEventSink);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException, true);

            foreach (string arg in args)
            {
                if (arg.Length > 3 && string.Compare(arg.Substring(0, 3), "-l:", true) == 0) {
                    try { System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(arg.Substring(3)); } catch (ArgumentException) { }
                }
            }

            MainForm main;
            System.Globalization.CultureInfo currentCulture;
            do
            {
                currentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
                main = new MainForm(args);
                if (main.forceExit == false) { Application.Run(main); }
            }
            while (currentCulture.Equals(System.Threading.Thread.CurrentThread.CurrentUICulture) == false);
        }

        public static void Debug(string msg) { try { File.AppendAllText("debug.log", msg + "\r\n"); } catch (Exception) { } }

        public static void ExceptionSink(object sender, System.Threading.ThreadExceptionEventArgs args)
        {
            Debug("ExceptionSink: " + args.Exception.ToString());
        }

        public static void UnhandledExceptionEventSink(object sender, UnhandledExceptionEventArgs args)
        {
            Debug("UnhandledExceptionEventSink: " + ((Exception)args.ExceptionObject).ToString());
        }
    }
}
