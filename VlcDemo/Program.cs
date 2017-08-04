using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vlc.DotNet.Core.Interops;

namespace VlcDemo
{
    static class Program
    {
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var a = new Action<string>((l) =>
            //{
            //    if (!File.Exists(l))
            //        return;
            //    else
            //        Debug.WriteLine(l);
            //    var myLibVlcCoreDllHandle = LoadLibrary(l);

            //    if (myLibVlcCoreDllHandle == IntPtr.Zero)
            //    {
            //        var errCode = Marshal.GetLastWin32Error();
            //        Debug.WriteLine(errCode);
            //    }
            //});

            


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormMain());
            Application.Run(new ControlTest());
        }
    }
}
