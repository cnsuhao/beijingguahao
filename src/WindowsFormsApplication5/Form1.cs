using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = uname.Text.Trim();
            string password = upwd.Text.Trim();
            string referer = @"http://www.9miao.com/";
            string seccodeverify = textBox1.Text.Trim();
            string sechash = "SAQ3mm3P0";
            string questionid = "0";
            string loginfield = "username";
            string formhash = "84c80598";
            string answer = "";
            string url = "http://www.9miao.com/member.php?mod=logging&action=login&loginsubmit=yes&handlekey=login&loginhash=LozHm&inajax=1";
            string data = "username=" + username + "&password=" + password + "&referer=" + referer + "&seccodeverify="
                + seccodeverify + "&sechash=" + sechash + "&questionid=" + questionid + "&loginfield=" + loginfield
                + "&formhash=" + formhash + "&answer=" + answer + "";
            string html=Miao9.sends(url, data, "POST", referer);
            html=System.Text.Encoding.GetEncoding("UTF-8").GetString(System.Text.Encoding.GetEncoding("GB2312").GetBytes(html));
            if (html.IndexOf("欢迎您") > 0)
            {
                //登陆成功
                Form2 f2 = new Form2();
                //f2.ShowDialog();
                f2.Show(this);
                this.Visible = false;
            }
            else {
                MessageBox.Show("登陆失败", "提示");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image= Miao9.LoadCodeImg();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Exit();
            Application.ExitThread();

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.9miao.com/member.php?mod=register");      
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.9miao.com/thread-44216-1-1.html");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox1.Image = Miao9.LoadCodeImg();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://www.9miao.com/thread-44216-1-1.html");
        }

    }
}
