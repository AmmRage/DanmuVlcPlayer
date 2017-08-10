using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VlcDemo.Mf;

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
            masterForm.Size = new System.Drawing.Size(600, 480);
            Application.Run(masterForm);
        }
    }
}
