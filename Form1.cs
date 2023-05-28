using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TempCleaner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UpdateList();
        }
        private void UpdateList()
        {
            try
            {
                listBox1.Items.Clear();
                string[] folders = File.ReadAllLines("data");
                foreach (var folder in folders)
                {
                    if (!String.IsNullOrWhiteSpace(folder))
                    {
                        listBox1.Items.Add(folder);
                    }
                }
            }
            catch
            {

            }
        }
        private string[] GetItems(ListBox listbox)
        {
            string[] allelements = listbox.Items.OfType<string>().ToArray();
            return allelements;
        }
        public void RecursiveDelete(DirectoryInfo baseDir)
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Add(folderBrowserDialog1.SelectedPath);
                List<string> existingFolders = listBox1.Items.OfType<string>().ToList();
                existingFolders.Add(folderBrowserDialog1.SelectedPath);
                File.WriteAllLines("data", existingFolders.ToArray());
                UpdateList();
            }
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex > -1)
                {
                    string[] allFolders = GetItems(listBox1);
                    List<string> allfolders = new List<string>();
                    foreach (string value in allFolders)
                    {
                        allfolders.Add(value);
                    }
                    allfolders.RemoveAt(listBox1.SelectedIndex);
                    File.Delete("data");
                    File.WriteAllLines("data", allfolders);
                    UpdateList();
                }
            }
            catch
            {

            }
        }

        public void PerformClean_Click(object sender, EventArgs e)
        {
            label1.Text = "Performing recursive clean...";
            string[] folders = File.ReadAllLines("data");
            foreach (string folder in folders)
            {
                RecursiveDelete(new DirectoryInfo(folder));
            }
            label1.Text = "Recursive clean complete";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
