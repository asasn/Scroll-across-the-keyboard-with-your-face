using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Helper
{
   
    public class WebHelper
    {
        /// <summary>
        /// 依据输入的url链接获取html源代码
        /// </summary>
        /// <param name="urlStr"></param>
        /// <returns></returns>
        public static string GetHtmlText(string urlStr)
        {

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(urlStr);
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36 Edg/109.0.1518.55";
            myHttpWebRequest.Timeout = 1000 * 10;
            try
            {
                WebResponse response = myHttpWebRequest.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 依据输入的url链接获取html源代码
        /// </summary>
        /// <param name="urlStr"></param>
        /// <returns></returns>
        public static StreamReader GetHtmlReaderObject(string urlStr)
        {

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(urlStr);
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36 Edg/109.0.1518.55";
            myHttpWebRequest.Timeout = 1000 * 10;
            try
            {
                WebResponse response = myHttpWebRequest.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// 网页源代码转码
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string ConvertExtendedASCII(string HTML)
        {
            StringBuilder str = new StringBuilder();
            char c;
            for (int i = 0; i < HTML.Length; i++)
            {
                c = HTML[i];
                if (Convert.ToInt32(c) > 127)
                {
                    str.Append("&#" + Convert.ToInt32(c) + ";");
                }
                else
                {
                    str.Append(c);
                }
            }
            return str.ToString();
        }
    }
}
