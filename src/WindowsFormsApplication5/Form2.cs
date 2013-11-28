using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace WindowsFormsApplication5
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            this.pictureBox1.Image=HtmlHelp.LoadCodeImg();
            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.Visible = false;
            e.Cancel = true;

        }
        

        string gh = "";
        string ghq = "";//确定预约的参数
        string sfz = "";//身份证号
        string hpid = "";//医院编号
        string keid = "";//科室编号
        string sx = "";//筛选条件
        string hpName = string.Empty;//医院地址名称简写
        string infohtml = "";//短信验证码页面字符串隐藏值
        List<string> yysjList = null;//预约时间的信息

        private void button1_Click(object sender, EventArgs e)
        {
            sfz = textBox2.Text;
            hpid = textBoxHpid.Text;
            keid = textBoxkeid.Text;
            sx = textBox7.Text.Trim();
            label3.Text = "";
            textBox6.Text = "";
            textBox5.Text = "";

            Thread t1 = new Thread(new ThreadStart(lands));
            t1.Start();    //启动线程t1

            
           
        }


        public void lands() {

                MethodInvoker mi = new MethodInvoker(land);//定义一个MethodInvoker用来在线程里面访问主线程的控件
                this.BeginInvoke(mi);//指定这个线程可以通过mi访问主线程。
        }

        public void land() {
            //登陆
            string post = string.Format("sfzhm={0}&truename={1}&yzm={2}", sfz, textBox1.Text, textBox3.Text);
            label3.Text = HtmlHelp.send("http://www.bjguahao.gov.cn/comm/logon.php", post, "POST");
            timer1.Interval = Convert.ToInt32(textBoxTime.Text);
            timer1.Start();
            //yysj(new object(),EventArgs.Empty);
        }


        bool flg = true;
        int strcount = 1;
        /// <summary>
        /// #登陆之后每秒调用一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void yysj(object sender, EventArgs e)
        {

            //获取可以预约的时间
            string url = "http://www.bjguahao.gov.cn/comm/plus/ajax_user.php?act=top_loginform";
            string html = HtmlHelp.sendGets(url, "http://www.bjguahao.gov.cn/comm/yyks-1.html");

            url = "http://www.bjguahao.gov.cn/comm/content.php?hpid=" + hpid + "&keid=" + keid;
            html = HtmlHelp.sendGet(url,"http://www.bjguahao.gov.cn/comm/yyks-1.html");

            Utils utils = new Utils(html);
            List<string> list = utils.toYY();//获取可以预约时间的链接
            if (list.Count!=0)
            {
                textBox6.Text = "已经获取预约时间" + Environment.NewLine;
                yysjList = list;
                timer1.Stop();
                yysjhou(url,hpid,keid);
                flg = false;
            }
            if (flg)//
            {
                textBox6.Text = strcount.ToString() + Environment.NewLine;
                strcount++;

            }
            
        }

        private void yysjhou(string urls,string hpid,string keid) {
            //获取可以预约医生的链接
            string urlt = yysjList[0];
            string yyys = HtmlHelp.sendGet(urlt,urls);
            int startHp=yyys.LastIndexOf("<a href=")+11;
            hpName = yyys.Substring(startHp);
            hpName=hpName.Substring(0,hpName.IndexOf("/"));
            Utils utilsys = new Utils(yyys);
            Ysinfo ys = utilsys.toYS(sx);//获取可以预约医生的链接
            if (ys == null)
            {
                label3.Text += "没有可以预约的医生" + Environment.NewLine;
                textBox6.Text += "没有可以预约的医生" + Environment.NewLine;
            }
            else {
                label3.Text += "预订挂号信息："+ Environment.NewLine+"职称：" + ys.zhicheng + Environment.NewLine+ "专长：" + ys.zhuanchang + Environment.NewLine;
                textBox6.Text += "预订挂号信息：" + Environment.NewLine + "职称：" + ys.zhicheng + Environment.NewLine + "专长：" + ys.zhuanchang + Environment.NewLine;
            }

            string url = @"http://www.bjguahao.gov.cn/comm" + ys.Url;
            string htmls=HtmlHelp.sendGet(url,urlt);
            int start=htmls.IndexOf("/shortmsg/dx_code.php?");
            string newhtml = htmls.Substring(start);
            int newstart = newhtml.IndexOf(",");
            string text = newhtml.Substring(0, newstart);
            text=text.Replace("\"", "");
            text = text.Replace("+hpid+", hpid);

            string ksid = getparam(htmls, "code_ksid");
            string datid = getparam(htmls, "code_datid");


            int infoPostion = htmls.LastIndexOf("html>")+5;
            infohtml = htmls.Substring(infoPostion);
             

            text = text.Replace("+ksid+", keid);
            text = text.Replace("+datid+", datid);
            text = text.Replace("+jiuz+", "");
            text = text.Replace("+ybkh+", "");
            text = text.Replace("+baoxiao","0");
            text = text.Replace("+", "");
            text = @"http://www.bjguahao.gov.cn/comm" + text;
            //label3.Text += text;


            //发送短信验证码
            ghq = "hpid=" + hpid + "&ksid" + ksid + "&datid=" + datid;
            string ghreferer = @"http://www.bjguahao.gov.cn/comm/xiehe/guahao.php?"+ghq;

            gh = ghq + "&jiuz=&ybkh=" + sfz + "&baoxiao=1&tpost=3d4e81798b906881a05e122fbc5351c3";

 
            string valuesstr = HtmlHelp.sendGets(text, ghreferer);
            if (valuesstr.Length >= 1)
            {
                label3.Text += valuesstr + Environment.NewLine;
            }
        }


        private string getparam(string htmls, string prarm) {
            int startksid = htmls.LastIndexOf(prarm);
            string wws = htmls.Substring(startksid);
            startksid = wws.IndexOf("value=");
            string strksid = wws.Substring(startksid + 7);
            int endksid = strksid.IndexOf("\"");
            string ksid = strksid.Substring(0, endksid);
            return ksid;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            //确定预约

            string post = gh + "&dxcode=" + textBox4.Text + getsms(infohtml); //post发送的参数
            string html = HtmlHelp.sends("http://www.bjguahao.gov.cn/comm/"+hpName+"/ghdown.php", post, "POST",ghq);
            if (html.IndexOf("alert") == -1)
            {
                textBox5.Text += "购票成功" + Environment.NewLine + Environment.NewLine;
            }
            else {
                textBox5.Text += "购票失败" + Environment.NewLine + Environment.NewLine;
            }
            textBox5.Text+=html+Environment.NewLine;

            //if (html.Length > 0) 
            //{
            //    string url = "http://www.bjguahao.gov.cn/comm/show_cont.php";
            //    html = HtmlHelp.sendGet(url);
            //    Form3 f3 = new Form3(html);
            //    f3.ShowDialog();
            //}
        }







        /// <summary>
        /// 获取短信验证码中隐藏值
        /// </summary>
        /// <returns></returns>
        public string getsms(string html)
        {
            html = html.Trim();
            string[] es = new string[1];
            es[0] = "<input type=hidden name=";
            string[] it = { "value=" };
            string param = String.Empty;
            //string html = "<input type=hidden name=djweui34t6UjUowhGcLkHNLumghFMuWdXQtDiScgdLXKBKVrlGizznLSkyIJH7363  value=902888>  <input type=hidden name=mscUjUowhGcLkHNLumghFMuWdXQtDiScgdLXKBKVrlGizznLSkyIJH7363  value=902888>  <input type=hidden name=tnndeqWIcnjoMqCsfbeJYvAeupNQbIcbhEaJVzKTnIYL7363  value=902888>  <input type=hidden name=qWIcnjoMqCsfbeJYvAeupNQbIcbhEaJVzKTnIYL7363  value=902888>  <input type=hidden name=UjUowhGcLkHNLumghFMuWdXQtDiScgdLXKBKVrlGizznLSkyIJH7363  value=880780>  <input type=hidden name=tygf6584SYLYPtdfXntMpCtscHWlmbeinRJdVxSMWpGerHxWrwrdjjoEYSzMmkfnqHdPdgZ7363  value=880780>  <input type=hidden name=SYLYPtdfXntMpCtscHWlmbeinRJdVxSMWpGerHxWrwrdjjoEYSzMmkfnqHdPdgZ7363  value=880780>  <input type=hidden name=SYLYPtdfXntMpCtscHWlmbeinRJdVxSMWpGerHxWrwrdjjoEYSzMmkfnqHdPdgZUjUowhGcLkHNLumghFMuWdXQtDiScgdLXKBKVrlGizznLSkyIJH7363  value=910351>  <input type=hidden name=edcrfvSYLYPtdfXntMpCtscHWlmbeinRJdVxSMWpGerHxWrwrdjjoEYSzMmkfnqHdPdgZUjUowhGcLkHNLumghFMuWdXQtDiScgdLXKBKVrlGizznLSkyIJH9021777363  value=902295>  <input type=hidden name=UjUowhGcLkHNLumghFMuWdXQtDiScgdLXKBKVrlGizznLSkyIJHc9021777363  value=902295> ";
            string[] tt = html.Split(es, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in tt)
            {
                string[] kv = item.Split(it, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    kv[1] = kv[1].Substring(0, kv[1].LastIndexOf(">"));
                }
                catch (Exception ex) {
                    break;
                }
                string key = kv[0].Trim();
                string value = kv[1].Trim();
                param += "&" + key + "=" + value;
            }

            return param;

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
             Process.Start(@"http://www.9miao.com");
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("是否关闭！", "提示", MessageBoxButtons.OKCancel) !=
            //DialogResult.OK)
            //{
            this.Visible = false;
                Application.Exit();
            //}
            
        }

        private void 隐藏界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;

        }

        private void 显示界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.Activate();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.bjguahao.gov.cn/comm/index.html");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            int hour = now.Hour;
            int minute = now.Minute;
            int second = now.Second;
        }

        



    }
}
