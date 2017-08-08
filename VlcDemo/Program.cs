using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var masterForm = new FormMf();
            Application.Run(masterForm);
        }
    }
}
