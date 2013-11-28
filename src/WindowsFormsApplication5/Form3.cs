using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        string html = "";

        public Form3(string html)
        {
            this.html = html;
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = html;
            webBrowser1.ScriptErrorsSuppressed = false;
            this.webBrowser1.Document.Window.Error+= new HtmlElementErrorEventHandler(Window_Error); 

        }
        
            //对错误进行处理  
        void Window_Error(object sender, HtmlElementErrorEventArgs e)  
        {  
             // 自己的处理代码  
            e.Handled = true;  
        }  
    }
}
