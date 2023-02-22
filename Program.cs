using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempCleaner
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            if (args[1] == "-nogui")
            {
                var form = new Form1();
                string[] folders = File.ReadAllLines("data");
                foreach (string folder in folders)
                {
                    form.RecursiveDelete(new DirectoryInfo(folder));
                }
            }
        }
    }
}
