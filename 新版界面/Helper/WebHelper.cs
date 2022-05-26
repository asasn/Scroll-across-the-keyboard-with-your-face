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
            WebRequest request = WebRequest.Create(urlStr);
            request.Timeout = 1000 * 10;
            try
            {
                WebResponse response = request.GetResponse();
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
