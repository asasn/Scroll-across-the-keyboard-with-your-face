﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace 脸滚键盘.公共操作类
{
    class WebOperate
    {
        public static string GetHtmlText(string urlStr)
        {
            WebRequest request = WebRequest.Create(urlStr);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            return reader.ReadToEnd();
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
