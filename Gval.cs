using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using 脸滚键盘.信息卡模板;

namespace 脸滚键盘
{
    /// <summary>
    /// 通用变量类
    /// </summary>
    static class Gval
    {
        public static MaterialWindow materialWin = new MaterialWindow(null, null);

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
        /// 主窗口对应的控件对象
        /// </summary>
        public struct MainWindow
        {
            public static TextBox tbPrice;         
        }

        /// <summary>
        /// 编辑器窗口对应的控件对象
        /// </summary>
        public struct Editor
        {
            public static uc_Editor Uc;
        }

        /// <summary>
        /// 信息卡窗口对应的控件对象
        /// </summary>
        public struct InfoCard
        {
            public static TreeView RoleTv;
            public static TreeView FactionTv;
            public static TreeView GoodsTv;
            public static TreeView CommonTv;
        }

        /// <summary>
        /// 当前打开的书籍/文档来源信息
        /// </summary>
        public struct Current
        {
            ///// <summary>
            ///// 基本书籍目录文件夹路径（只读）：app/books
            ///// </summary>
            //public static string BooksPath { get { return Base.AppPath + "/books"; } }

            /// <summary>
            /// 指向节点所在的控件
            /// </summary>
            public static TreeView curTv;

            /// <summary>
            /// 当前控件标志
            /// </summary>
            public static string curUcTag;

            /// <summary>
            /// 指向当前节点的路径
            /// </summary>
            public static string curItemPath;

            /// <summary>
            /// 指向当前书籍分卷路径
            /// </summary>
            public static string curVolumePath;

            /// <summary>
            /// 指向当前书籍文件夹路径
            /// </summary>
            public static string curBookPath;

            /// <summary>
            /// 指向的节点item
            /// </summary>
            public static TreeViewItem curItem;

            /// <summary>
            /// 指向当前分卷节点
            /// </summary>
            public static TreeViewItem curVolumeItem;

            /// <summary>
            /// 指向当前书籍节点
            /// </summary>
            public static TreeViewItem curBookItem;
        }

        public struct DragDrop
        {
            /// <summary>
            /// 来源控件
            /// </summary>
            public static object dragUc;

            /// <summary>
            /// 拖动源书籍文件夹路径
            /// </summary>
            public static string dragBookPath;

            /// <summary>
            /// 拖动源分卷文件夹路径
            /// </summary>
            public static string dragVolumePath;

            /// <summary>
            /// 拖动源章节文档路径全名
            /// </summary>
            public static string dragTextFullName;

            /// <summary>
            /// 拖动源TreeView
            /// </summary>
            public static TreeView dragTreeView;

            /// <summary>
            /// 拖动源item
            /// </summary>
            public static TreeViewItem dragItem;

            /// <summary>
            /// 拖动源item的rootItem
            /// </summary>
            public static TreeViewItem dragBookItem;

            /// <summary>
            /// 拖动源分卷节点
            /// </summary>
            public static TreeViewItem dragVolumeItem;

        }
    }
}
