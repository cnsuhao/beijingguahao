using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication5
{
    public class Ysinfo
    {
        public string zhicheng="";//职称
        public string zhuanchang = "";//专长
        private string url = "";//预约挂号地址

        public string Url
        {
            get { return url; }

            set { tourl(value); }
        }

        private void tourl(string w) {
            w=w.Substring(w.IndexOf("href") + 7);
            w=w.Substring(0,w.IndexOf("onclick") - 2);
            this.url = w;
        }

        public Ysinfo()
        {
        }
         
    }
}
