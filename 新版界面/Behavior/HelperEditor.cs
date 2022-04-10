using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Behavior
{
    public class HelperEditor
    {
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
    }
}
