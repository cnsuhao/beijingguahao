using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace WindowsFormsApplication5
{
   public class Utils
    {

       private string html = "";
       private List<string> list = new List<string>();//可预约日期地址
       private List<Ysinfo> ysList = new List<Ysinfo>();//可预约医生地址

       private List<Ysinfo> zjList = new List<Ysinfo>();//专家
       private List<Ysinfo> zjsList = new List<Ysinfo>();//正教授
       private List<Ysinfo> fjsList = new List<Ysinfo>();//副教授
       private List<Ysinfo> zzysList = new List<Ysinfo>();//主治医师  
       private List<Ysinfo> zrysList = new List<Ysinfo>();//主任医师
       private List<Ysinfo> yssList = new List<Ysinfo>();//医师
       private List<Ysinfo> qtList = new List<Ysinfo>();//其他


       /// <summary>
       /// 构造方法
       /// </summary>
       /// <param name="str">返回的网页信息</param>
       public Utils(string str) {
           this.html = str;
       }

       /// <summary>
       /// 查找有没有可以预约的时间
       /// </summary>
       public List<string> toYY()
       {
           if (this.html.IndexOf("<div class='greenbg'>") < 0) 
           {
               return list;
           }

           string[] stringsz = html.Split(new string[] { "<div class='greenbg'>" },StringSplitOptions.RemoveEmptyEntries);
           for (int i = 1; i < stringsz.Length; i++) {
               string w = stringsz[i];
               w=w.Substring(w.IndexOf("href='") + 6);
               int st = w.IndexOf("title")-2;
               w = w.Substring(0,st);
               list.Add(@"http://www.bjguahao.gov.cn/comm/"+w);
           }

           return list;

           
       }


       /// <summary>
       /// 查询有没有可以预约的医生
       /// </summary>
       /// <returns></returns>
       public Ysinfo toYS(string sc)
       {
          int count= this.html.IndexOf("<td class='tdtitle'>操作</td>");
          if (count < 0) {
              return null;
          }
          int lcount = this.html.LastIndexOf("</table>");
          html=html.Substring(0, lcount);
          html = html.Substring(count + 31);


          string pat1 = @"<td>(?<text>.*?)</td>";
          Ysinfo ys = null;
          int yc = 1;
          foreach (Match item in Regex.Matches(html, pat1))
          {
              string jg = item.Groups["text"].Value;

              if (yc == 6) {
                  ys = new Ysinfo();
              }
              if (yc == 6) {
                  ys.zhicheng = jg;
              }
              if (yc == 8) {
                  ys.zhuanchang = jg;
              }
              if (yc == 11) {
                  if (jg != "约满") {
                      ys.Url = jg;
                      ysList.Add(ys);

                      string zw = ys.zhicheng;
                      switch (zw) {
                          case "专家": zjList.Add(ys); break;
                          case "正教授": zjsList.Add(ys); break;
                          case "副教授": fjsList.Add(ys); break;
                          case "主任医师": zrysList.Add(ys); break;
                          case "主治医师": zzysList.Add(ys); break;
                          case "医师": yssList.Add(ys); break;
                          default: qtList.Add(ys); break;
                      }
                  }
                  yc = 0;
                 
              }

              yc += 1;
          }
          Ysinfo info = null;
          if (ysList.Count < 1) {
              return null;
          }
          if (ysList.Count == 1) {
              return ysList[0];
          }

           //如果有有筛选条件，则先按照筛选条件筛选专家、正教授、副教授
          if (sc.Length > 0) {
              info = sx(zjList,sc);//筛选专家
              if (info == null)
              {
                  info = sx(zjsList, sc);//筛选正教授
              }

              if (info == null)
              {
                  info = sx(fjsList, sc);//筛选副教授
              }
          }

          

          //if (info == null)
          //{
          //    info = sx(zzysList,sc);//筛选主治医师
          //}

          //if (info == null)
          //{
          //    info = sx(zrysList,sc);//筛选主任医师
          //}

          //if (info == null)
          //{
          //    info = sx(yssList,sc);//筛选医师
          //}




           //如果没有筛选条件或者没有筛选到符合的医生
          if (info == null) {
              info=fh(zjList);

              if (info == null) {
                  info = fh(zjsList);
              }

              if (info == null)
              {
                  info = fh(zjsList);
              }

              if (info == null)
              {
                  info = fh(fjsList);
              }

              if (info == null)
              {
                  info = fh(zzysList);
              }

              if (info == null)
              {
                  info = fh(zrysList);
              }

              if (info == null)
              {
                  info = fh(yssList);
              }

              if(info==null){
                info=fh(qtList);
              }
              if (info==null){
                 if (ysList.Count > 0) 
                 {
                     return ysList[0];
                 }
              }



            
          }






          return info;

          //string[] strlist = html.Split(new string[] { "<tr>" }, StringSplitOptions.RemoveEmptyEntries);
          //for (int i = 0; i < strlist.Length; i++) { 
            
          //}
          
           
       }


       /// <summary>
       /// 筛选颈椎专长的
       /// </summary>
       /// <param name="list"></param>
       /// <returns></returns>
       private Ysinfo sx(List<Ysinfo> list,string sc) {
           if (list.Count < 1)
           {
               return null;
           }

           foreach (Ysinfo info in list) {
               if (info.zhuanchang.IndexOf(sc) >= 0)
               {
                   return info;
               }
           }
           return null;
       }

       private Ysinfo fh(List<Ysinfo> list) {
           if (list.Count > 0) {
               return list[0];
           }
           return null;
       }



      


        


    }
}
