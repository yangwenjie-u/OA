using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using BD.JC.JS.Common;

namespace BD.JC.JS.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = Environment.CurrentDirectory + "\\BD.JC.JS.Common.dll";
            Assembly dll = Assembly.LoadFile(path);
            Type cal = dll.GetType("BD.JC.JS.Common.ComSetJcyj", true);
            object o = System.Activator.CreateInstance(cal);//创建实例
            MethodInfo method = cal.GetMethod("Calculate");
            string ret = (string)method.Invoke(o, new object[] { "hello" });
            MessageBox.Show(ret); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ComSetJcyj cal = new ComSetJcyj();
            MessageBox.Show(cal.Calculate(textBox1.Text));
        }
    }
}
