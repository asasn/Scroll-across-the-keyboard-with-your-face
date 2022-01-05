using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.自定义控件;
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘.公共操作类
{
    class Gval
    {
        public struct Path
        {
            public static string App { get { return Environment.CurrentDirectory; } }

            public static string Books { get { return Environment.CurrentDirectory + "/books"; } }

            public static string Resourses { get { return Environment.CurrentDirectory + "/Resourses"; } }
        }

        public struct CurrentBook
        {
            public static string Uid;

            public static string Name;

            public static double Price;

            public static int BornYear;

            public static int CurrentYear;

            public static TreeViewNode CurNode;
        }

        public struct Flag
        {
            public static bool IsSqlconnOpening;

            public static bool Loading;
        }

        public struct Uc
        {
            public static MainWindow MWindow;
            public static Grid BooksPanel;
            public static HandyControl.Controls.TabControl TabControl;
            public static UcTreeBook TreeBook;
            public static UcTreeBook TreeMaterial;
            public static UcTreeBook TreeNote;
            public static UcTreeTask TreeTask;
            public static UcCards RoleCards;
            public static UcCards OtherCards;
            public static UcCards PublicRoleCards;
            public static UcCards PublicOtherCards;
        }

        public struct SQLClass
        {
            public static Dictionary<string, SqliteOperate> Pools = new Dictionary<string, SqliteOperate>();
        }
    }
}
