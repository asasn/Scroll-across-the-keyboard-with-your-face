﻿using System;
using System.Windows;
using System.Windows.Controls;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.自定义控件;

namespace 脸滚键盘.公共操作类
{
    class Gval
    {
        public struct Path
        {
            public static string App { get { return Environment.CurrentDirectory; } }

            public static string Books { get { return Environment.CurrentDirectory + "/books"; } }
        }

        public struct CurrentBook
        {
            public static string Uid;

            public static string Name;

            public static double Price;

            public static int BornYear;

            public static int CurrentYear;
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
            public static HandyControl.Controls.Drawer BooksDrawer;
            public static UcTreeBook TreeBook;
            public static UcTreeMaterial TreeMaterial;
            public static UcTreeTask TreeTask;
            public static UcTreeNote TreeNote;
            public static MaterialWindow MaterialWindow = new MaterialWindow();
            public static UcCards RoleCards;
            public static UcCards OtherCards;
            public static UcCards PublicRoleCards;
            public static UcCards PublicOtherCards;
            public static UcHistoryBar HistoryBar;
        }
    }
}
