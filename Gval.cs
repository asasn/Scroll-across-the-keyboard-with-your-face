using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 脸滚键盘
{
    /// <summary>
    /// 通用变量类
    /// </summary>
    static class Gval
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public struct Base
        {
            /// <summary>
            /// 基本文件夹路径（只读）
            /// </summary>
            public static string AppPath { get { return System.Environment.CurrentDirectory; } }

        }
    }
}
