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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Xml;
using System.Xml.Linq;

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

        private void button1_Click(object sender, EventArgs e)          // функция кнопки "поиска"
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
                if (Directory.GetParent(path) != null)
                {
                    listView1.Items.Insert(0, new ListViewItem(new string[] { "...", "", "", "" }));               // выводим троеточие если существует путь наверх (назад по папкам)
                }                
                try
                {
                    string[] dirs = Directory.GetDirectories(path);
                    foreach (string directory in dirs)
                    {                       
                        DirectoryInfo dir = new DirectoryInfo(directory);
                        
                        if (CalculateFolderSize(dir.FullName) < 0)                                                  // проверка на доступность папки
                        {
                            listView1.Items.Add(new ListViewItem(new string[] { dir.Name, "-", "Папка", "Нет доступа" })).BackColor = Color.Gainsboro;
                        }
                        else
                        {
                            listView1.Items.Add(new ListViewItem(new string[] { dir.Name, "-", "Папка", $"{CalculateFolderSize(dir.FullName)} Кб" })).BackColor = Color.Gainsboro;
                        }
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

        private void textBox1_KeyUp(object sender, KeyEventArgs e) // для обработки нажатия enter
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
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
                return -1;                                  // возвращаем -1 если папка недоступна
            }
            
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            
            foreach (DirectoryInfo dir in directories)
            {
                if (CalculateFolderSize(dir.FullName) > 0)               // если значение размера отрицательное то не складываем его
                {
                    totalSize += CalculateFolderSize(dir.FullName);
                }
            }
            return totalSize;           
        }

        private void button1_Click_1(object sender, EventArgs e)
        {           

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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void button1_Click_3(object sender, EventArgs e)                // сохранение listview1 в файл xml
        {
            if (listView1.Items.Count != 0)                                     // проверка на пустоту listview1
            {
                string savefilepath = "base.xml";              
                XDocument doc = new XDocument(                                  // Создаем файл
                new XElement("folder",
                    from ListViewItem item in listView1.Items                   // проходимся по строчкам
                    select new XElement("item",
                        from int i in Enumerable.Range(0, item.SubItems.Count)     
                        select new XElement($"column{i}", item.SubItems[i].Text))));
                doc.Save(savefilepath);

                MessageBox.Show(
                   "Файл сохранен!",
                   "Сообщение",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information,
                   MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.DefaultDesktopOnly);
            }
            else                                                                //если listview1 пустой
            {
                MessageBox.Show(
                    "Список пуст",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);                        
            }
           
            /* XDocument doc = new XDocument();
                 XElement folder = new XElement("Folder");
                 doc.Add(folder);
             foreach (ListViewItem item in listView1.Items)
             {
                 XElement items = new XElement("item");
                 for (int i = 0; i < item.SubItems.Count; i++)
                 {
                     new XElement(listView1.Items[i].SubItems[1].Text);
                     new XElement(listView1.Items[i].SubItems[2].Text);
                     new XElement(listView1.Items[i].SubItems[3].Text);
                     new XElement(listView1.Items[i].SubItems[4].Text);

                 }
             }*/



        }
    }
}




