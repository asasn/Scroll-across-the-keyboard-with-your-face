using ICSharpCode.AvalonEdit;
using RootNS.Brick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RootNS.Behavior
{
    public class HelperEditor
    {

        /// <summary>
        /// 字符串转化为字节流
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static MemoryStream ConvertStringToStream(string strContent)
        {
            byte[] array = Encoding.UTF8.GetBytes(strContent);
            MemoryStream stream = new MemoryStream(array);
            return stream;
        }


        /// <summary>
        /// 字数统计
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int CountWords(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return 0;
            }
            int total = 0;
            char[] q = content.ToCharArray();
            for (int i = 0; i < q.Length; i++)
            {
                if (q[i] > 32 && q[i] != 0xA0 && q[i] != 0x3000) // 非空字符，Unicode编码0x3000为全角空格，
                {
                    total += 1;
                }
            }
            return total;
        }


        /// <summary>
        /// 文字排版，并重新赋值给编辑框
        /// </summary>
        /// <param name="tb"></param>
        public static void TypeSetting(TextEditor tb)
        {
            string reText = "　　"; //开头是两个全角空格
            string[] sArray = tb.Text.Split(new char[] { '\r', '\n', '\t' });
            string[] sArrayNoEmpty = sArray.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            foreach (string lineStr in sArrayNoEmpty)
            {
                //当前段落非空时，注意，这里的长度需要-1才是最后一个索引号
                if (Array.IndexOf(sArrayNoEmpty, lineStr) != sArrayNoEmpty.Length - 1)
                {
                    //非末尾的情况
                    reText += lineStr.Trim() + "\n\n　　";
                }
                else
                {
                    //末尾时不添加新行
                    reText += lineStr.Trim();
                }
            }
            //排版完成，重新赋值给文本框
            tb.Text = reText;
            //光标移动至文末 
            tb.ScrollToLine(tb.LineCount);
            tb.SelectionLength = 0;
            tb.SelectionStart = tb.Text.Length;
            tb.ScrollToEnd();
            tb.ScrollToEnd();
        }





    }
}