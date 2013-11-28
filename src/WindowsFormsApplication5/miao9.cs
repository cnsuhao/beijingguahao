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

namespace WindowsFormsApplication5
{
    public class Miao9
    {

        static CookieContainer Cookies = new CookieContainer();
        static HttpWebRequest myHttpWebRequest;
        static byte[] oneData = { };

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="post">sfzhm={0}&truename={1}&yzm={2}</param>
        /// <param name="method">GET,POST</param>
        /// <returns></returns>
        public static string send(string url, string post, string method)
        {
            HttpWebRequest myHttpWebRequest;
            byte[] oneData = { };

            Encoding encoding = Encoding.GetEncoding("UTF-8");
            oneData = encoding.GetBytes(post);

            Uri uri = new Uri(url);
            myHttpWebRequest = (HttpWebRequest)WebRequest.Create(uri);//请求的URL
            myHttpWebRequest.CookieContainer = Cookies;//*发送COOKIE
            myHttpWebRequest.Method = method;
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            myHttpWebRequest.ContentLength = oneData.Length;
            Stream newMyStream = myHttpWebRequest.GetRequestStream();
            newMyStream.Write(oneData, 0, oneData.Length);

            try
            {
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string str = sr.ReadToEnd();

                if (str.Length < 1)
                {

                    return "登陆成功,正在获取可预约信息" + Environment.NewLine;
                }
                return str;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string sends(string url, string post, string method, string referer)
        {
            HttpWebRequest myHttpWebRequest;
            byte[] oneData = { };

            Encoding encoding = Encoding.GetEncoding("UTF-8");
            oneData = encoding.GetBytes(post);

            Uri uri = new Uri(url);
            myHttpWebRequest = (HttpWebRequest)WebRequest.Create(uri);//请求的URL
            myHttpWebRequest.CookieContainer = Cookies;//*发送COOKIE
            myHttpWebRequest.Method = method;
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            myHttpWebRequest.ContentLength = oneData.Length;
            myHttpWebRequest.Referer = referer;
            Stream newMyStream = myHttpWebRequest.GetRequestStream();
            newMyStream.Write(oneData, 0, oneData.Length);

            try
            {
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string str = sr.ReadToEnd();

                return str + Environment.NewLine;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string sendGet(string url)
        {
            HttpWebRequest myHttpWebRequest;

            myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);//请求的URL
            myHttpWebRequest.CookieContainer = Cookies;//*发送COOKIE
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string str = sr.ReadToEnd();

                return str;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string sendGet(string url, string referer)
        {
            HttpWebRequest myHttpWebRequest;

            myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);//请求的URL
            myHttpWebRequest.CookieContainer = Cookies;//*发送COOKIE
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.Referer = referer;

            try
            {
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string str = sr.ReadToEnd();

                return str;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }





        public static string sendGets(string url)
        {
            HttpWebRequest myHttpWebRequest;

            myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);//请求的URL
            myHttpWebRequest.CookieContainer = Cookies;//*发送COOKIE
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.Accept = "*/*";
            myHttpWebRequest.ProtocolVersion = new Version("1.0");
            //myHttpWebRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            myHttpWebRequest.Headers.Set("X-Requested-With", "XMLHttpRequest");
            string nl = url;
            int count = nl.IndexOf("?");
            nl = @"http://www.bjguahao.gov.cn/comm/TG/guahao.php?" + nl.Substring(count + 1);
            count = nl.IndexOf("&jiuz");
            nl = nl.Substring(0, count);

            //myHttpWebRequest.Headers.Set("Referer",nl);
            myHttpWebRequest.Referer = nl;

            try
            {
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string str = sr.ReadToEnd();
                return str;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        public static string sendGets(string url, string referer)
        {
            HttpWebRequest myHttpWebRequest;

            myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);//请求的URL
            myHttpWebRequest.CookieContainer = Cookies;//*发送COOKIE
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.Accept = "*/*";
            myHttpWebRequest.ProtocolVersion = new Version("1.0");
            //myHttpWebRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            myHttpWebRequest.Headers.Set("X-Requested-With", "XMLHttpRequest");
            //myHttpWebRequest.Headers.Set("Referer",nl);
            myHttpWebRequest.Referer = referer;

            try
            {
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string str = sr.ReadToEnd();
                return str;


            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }



        public static Image LoadCodeImg()
        {
            try
            {
                string oneUrl = "http://www.9miao.com/plugin.php?id=cloudcaptcha:get&rand=L8JjWIbJiJ";
                Random rd = new Random();
                oneUrl += rd.NextDouble().ToString();

                myHttpWebRequest = (HttpWebRequest)WebRequest.Create(oneUrl);//请求的URL
                myHttpWebRequest.CookieContainer = Cookies;//*发送COOKIE
                myHttpWebRequest.Method = "GET";
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.Referer = "http://www.9miao.com/";

                //获取返回资源
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.GetResponse();

                //获取流
                Image bitmapImage = Bitmap.FromStream(response.GetResponseStream()) as Bitmap;

                return bitmapImage;
            }
            catch (Exception ex)
            {

                return null;

            }
        }
    }
}