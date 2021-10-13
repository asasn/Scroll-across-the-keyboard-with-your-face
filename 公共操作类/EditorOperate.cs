using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace 脸滚键盘
{
    static class EditorOperate
    {


        /// <summary>
        /// 公共方法：获取最大行数
        /// </summary>
        public static int GetMaxLineNum(TextBox tb)
        {
            int n = 0;
            for (var i = 0; i < tb.LineCount; i++)
            {
                //存在非空字符且不存在换行符（统计段落）
                if (EditorOperate.IsHasWords(tb.GetLineText(i)))
                {
                    if (i > 0)
                    {
                        //第二行的情况，判断上一行是否存在换行
                        if (tb.GetLineText(i - 1).Contains("\n"))
                        {
                            n++;
                        }
                    }
                    else
                    {
                        //第一行的情况
                        n++;
                    }
                }
            }
            return n;
        }

        /// <summary>
        /// 公共方法：文字排版，并重新赋值给编辑框
        /// </summary>
        /// <param name="tb"></param>
        public static void ReformatText(TextEditor tb)
        {
            string reText = "　　"; //开头是两个全角空格
            string[] sArray = tb.Text.Split(new char[] { '\r', '\n', '\t', '　' });
            string[] sArrayNoEmpty = sArray.Where(s => !string.IsNullOrEmpty(s)).ToArray();
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
            tb.ScrollToEnd();
            tb.SelectionStart = tb.Text.Length;
        }

        /// <summary>
        /// 公共方法：字数统计
        /// </summary>
        /// <param name="StrContert"></param>
        /// <returns></returns>
        public static int WordCount(string StrContert)
        {
            if (string.IsNullOrEmpty(StrContert))
            {
                return 0;
            }
            int totalWord = 0;
            char[] q = StrContert.ToCharArray();
            for (int i = 0; i < q.Length; i++)
            {
                if (q[i] > 32 && q[i] != 0xa0 && q[i] != 0x3000) // 非空字符，Unicode编码0x3000为全角空格，
                {
                    totalWord += 1;
                }
            }
            return totalWord;
        }


        /// <summary>
        /// 公共方法：判断是否存在可见字符串
        /// </summary>
        /// <param name="StrContert"></param>
        /// <returns></returns>
        public static bool IsHasWords(string StrContert)
        {
            if (string.IsNullOrEmpty(StrContert))
            {
                return false;
            }
            char[] q = StrContert.ToCharArray();
            for (int i = 0; i < q.Length; i++)
            {
                if (q[i] > 32 && q[i] != 0xa0 && q[i] != 0x3000) // 非空字符，Unicode编码0x3000为全角空格，
                {
                    return true;
                }
            }
            return false;
        }
    }
}
