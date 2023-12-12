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
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
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


            listView1.Items.Clear();
            string path = textBox1.Text;
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();
            for (int numFiles = 0; numFiles < files.Length; numFiles++)

            {
                string nameWithoutExt = Path.GetFileNameWithoutExtension(files[numFiles].Name);


                listView1.Items.Add(nameWithoutExt);
                listView1.Items[numFiles].SubItems.Add(files[numFiles].LastAccessTime.ToString());
                listView1.Items[numFiles].SubItems.Add(files[numFiles].Extension);
                listView1.Items[numFiles].SubItems.Add(files[numFiles].Length.ToString());

            }




        }
    }
}


