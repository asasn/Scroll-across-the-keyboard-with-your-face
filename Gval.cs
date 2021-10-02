using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        /// <summary>
        /// 当前打开的书籍/文档来源信息
        /// </summary>
        public struct CurrentBook
        {
            /// <summary>
            /// 基本书籍目录文件夹路径（只读）：app/books
            /// </summary>
            public static string BooksPath { get { return Base.AppPath + "/books"; } }

            /// <summary>
            /// 指向当前书籍文件夹路径
            /// </summary>
            public static string curBookPath;

            /// <summary>
            /// 指向当前分卷文件夹路径
            /// </summary>
            public static string curVolumePath;

            /// <summary>
            /// 指向当前章节文档路径全名
            /// </summary>
            public static string curTextFullName;

            /// <summary>
            /// 指向节点所在的控件
            /// </summary>
            public static TreeView curTv;

            /// <summary>
            /// 来源控件
            /// </summary>
            public static object originUc;

            /// <summary>
            /// 指向的节点item
            /// </summary>
            public static TreeViewItem curItem;

            /// <summary>
            /// 指向当前书籍节点
            /// </summary>
            public static TreeViewItem curBookItem;

            /// <summary>
            /// 指向当前分卷节点
            /// </summary>
            public static TreeViewItem curVolumeItem;
        }
    }
}
