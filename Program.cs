using System;
using System.IO;
using Microsoft.Win32;
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
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.SetValue("TempCleaner", "\"C:\\Program Files\\TempCleaner\\TempCleaner.exe\" -nogui");
                key.Close();
            } catch
            {
                string caption = "Autorun setup error";
                string text = "Cannot create autorun entry. Please add TempCleaner to autorun manually";
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBox.Show(text, caption, btn);
            }
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            if (args[1] == "-nogui")
            {
                string[] folders = File.ReadAllLines("data");
                foreach (string folder in folders)
                {
                    Console.WriteLine(folder);
                    RecursiveDelete(new DirectoryInfo(folder));
                }
            }
        }
        internal static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;
            foreach (var dir in baseDir.EnumerateDirectories())
            {
                RecursiveDelete(dir);
            }
            var files = baseDir.GetFiles();
            foreach (var file in files)
            {
                file.IsReadOnly = false;
                file.Delete();
            }
            baseDir.Delete();
        }
    }
}
