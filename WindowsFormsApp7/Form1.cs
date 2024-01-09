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
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        private string previousText;
        public Form1()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.FullRowSelect = true;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)   // строка куда вводим путь
        {

        }


        private void button1_Click(object sender, EventArgs e) // функция клика кнопки "поиска"
        {
            // int Numberline = 0;
            // int NLine = 0;
            

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
                if (Directory.GetParent(path) != null)
                {
                    listView1.Items.Insert(0, new ListViewItem(new string[] { "...", "", "", "" }));                        // выводим троеточие если существует путь наверх (назад по папкам)
                }
                try
                {
                    string[] dirs = Directory.GetDirectories(path);
                    foreach (string directory in dirs)
                    {
                        DirectoryInfo dir = new DirectoryInfo(directory);
                        listView1.Items.Add(new ListViewItem(new string[] { dir.Name, "-", "Папка", $"{CalculateFolderSize(dir.FullName)} Кб" })).BackColor = Color.Gainsboro;         // вывод папок 

                    }
                }
                catch { }

                try
                {
                    string[] files = Directory.GetFiles(path);
                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        double sizeFile = fileInfo.Length;
                        listView1.Items.Add(new ListViewItem(new string[] { fileInfo.Name, fileInfo.LastWriteTime.ToString(), fileInfo.Extension, $"{Math.Ceiling(sizeFile / 1024)} Кб" }));    // вывод файлов и перевод размера в Кб
                    }
                }
                catch { }
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
        private double CalculateFolderSize(string dirpath)    // высчитывание размера папки и перевод в Кб
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dirpath);
            double totalSize = 0;
            double size = 0;
            try
            {
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (FileInfo file in files)
                {
                    size = file.Length;
                    totalSize = totalSize + Math.Ceiling(size / 1024);

                }
            }
            catch 
            {               
                return totalSize;               
            }

            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            foreach (DirectoryInfo dir in directories)
            {
                totalSize += CalculateFolderSize(dir.FullName);
            }
            return totalSize;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            /*  string directoryPath = textBox1.Text;
              DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
              DirectoryInfo parentDirectoryInfo = directoryInfo.Parent;
              if (Directory.Exists(textBox1.Text))
              {
                  textBox1.Text = parentDirectoryInfo.FullName;
                  button1_Click(sender, e);
              }*/

        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void listView1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            int ind = listView1.SelectedIndices[0];
            string test = listView1.Items[ind].SubItems[2].Text;   // информация в столбце "Тип"
            string Namefolder = listView1.Items[ind].Text;        // считывание названия папки

            if (test == "Папка")
            {
                try                                                                              // проверяем доступность поддиректории
                {
                    textBox1.Text = textBox1.Text + @"\" + Namefolder;
                    string directoryPath = textBox1.Text;
                    DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                    DirectoryInfo[] directories = directoryInfo.GetDirectories();

                    button1_Click(sender, e);
                }                                                                   
                catch
                {                                                                                 // если поддиректории недоступны выводим ошибку и возращаем путь    
                    string directoryPath = textBox1.Text;
                    DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                    DirectoryInfo parentDirectoryInfo = directoryInfo.Parent;
                    textBox1.Text = parentDirectoryInfo.FullName;

                    MessageBox.Show(
                    "Ошибка открытия папки",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            else if (ind == 0)                                                                    // проверка клика по первой строчке 
            {
                string directoryPath = textBox1.Text;
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                DirectoryInfo parentDirectoryInfo = directoryInfo.Parent;

                if (Directory.Exists(textBox1.Text))                                              // проверка существования пути
                {
                    textBox1.Text = parentDirectoryInfo.FullName;
                    button1_Click(sender, e);
                }

            }





        }
       

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        

    }
}




