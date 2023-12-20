using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Input;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        private string previousText;
        int Numberline;
        public Form1()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == previousText)
            {
                MessageBox.Show(
                    "Информация о файлах уже выгружена!",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            previousText = textBox1.Text;

            listView1.Items.Clear();
            string path = textBox1.Text;
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show(
                    "Ничего не введено",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] files = dir.GetFiles();
                DirectoryInfo[] dirs = dir.GetDirectories();
                for (int numDir = 0; numDir < dirs.Length; numDir++)
                {
                    listView1.Items.Add(dirs[numDir].Name);
                    listView1.Items[numDir].SubItems.Add("");
                    listView1.Items[numDir].SubItems.Add("Папка");
                    listView1.Items[numDir].SubItems.Add("");
                    Numberline = numDir + 1;
                }
                for (int numFiles = 0; numFiles < files.Length; numFiles++)
                {
                    string nameWithoutExt = Path.GetFileNameWithoutExtension(files[numFiles].Name);
                    double sizefile = (files[numFiles].Length);
                    sizefile = Math.Ceiling(sizefile / 1024);
                    listView1.Items.Add(nameWithoutExt);
                    listView1.Items[numFiles + Numberline].SubItems.Add(files[numFiles].LastAccessTime.ToString());
                    listView1.Items[numFiles + Numberline].SubItems.Add(files[numFiles].Extension);
                    listView1.Items[numFiles + Numberline].SubItems.Add($"{(sizefile)} Кб");
                    
                }
               
            }
            else
            {
                MessageBox.Show(
                    "Введен неправильный путь",
                    "Сообщение",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error,
                     MessageBoxDefaultButton.Button1,
                     MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string directoryPath = textBox1.Text;
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            DirectoryInfo parentDirectoryInfo = directoryInfo.Parent;
            if (Directory.Exists(textBox1.Text))
            {
                textBox1.Text = parentDirectoryInfo.FullName;
                button1_Click(sender, e);
            }
            
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void listView1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            int ind = listView1.SelectedIndices[0];                        
            string test = listView1.Items[ind].SubItems[2].Text;
            string Namefile = listView1.Items[ind].Text;
            if (test == "Папка")
            {
                textBox1.Text = textBox1.Text + @"\" + Namefile;                 
                button1_Click(sender, e);
            }
                       
               
            

            /*ListViewHitTestInfo hitTest = listView1.HitTest(e.Location);
            if (hitTest.Item != null)
            {
                string selectedPath = hitTest.Item.Text;
                listView1.Items.Clear();

                DirectoryInfo dir = new DirectoryInfo(selectedPath);
                FileInfo[] files = dir.GetFiles();
                DirectoryInfo[] dirs = dir.GetDirectories();
                for (int numFiles = 0; numFiles < files.Length; numFiles++)
                {
                    string nameWithoutExt = Path.GetFileNameWithoutExtension(files[numFiles].Name);
                    double sizefile = (files[numFiles].Length);
                    sizefile = Math.Ceiling(sizefile / 1024);
                    listView1.Items.Add(nameWithoutExt);
                    listView1.Items[numFiles].SubItems.Add(files[numFiles].LastAccessTime.ToString());
                    listView1.Items[numFiles].SubItems.Add(files[numFiles].Extension);
                    listView1.Items[numFiles].SubItems.Add($"{(sizefile)} Кб");
                }

                for (int numDir = 0; numDir < dirs.Length; numDir++)
                {
                    listView1.Items.Add(dirs[numDir].Name);
                    listView1.Items[numDir].SubItems.Add("");
                    listView1.Items[numDir].SubItems.Add("Папка");
                    listView1.Items[numDir].SubItems.Add("");
                }

            }*/

        }
    }

}


