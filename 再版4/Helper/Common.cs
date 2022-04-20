using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version4.Helper
{
    internal class Common
    {
        /// <summary>
        /// 保持TextBox的输入格式（数值以string格式返回)
        /// </summary>
        /// <typeparam name="T">需要保持的数字类型</typeparam>
        /// <param name="text">输入的内容</param>
        /// <returns>数值转化成为字符串，重新赋予输入框</returns>
        public static string KeepTextType<T>(string text)
        {
            if (typeof(T) == typeof(double))
            {
                _ = double.TryParse(text, out double outValue);
                return outValue.ToString();
            }
            if (typeof(T) == typeof(long))
            {
                _ = long.TryParse(text, out long outValue);
                return outValue.ToString();
            }
            if (typeof(T) == typeof(int))
            {
                _ = int.TryParse(text, out int outValue);
                return outValue.ToString();
            }
            return string.Empty;
        }
    }
}
