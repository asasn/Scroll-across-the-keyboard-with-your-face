using NSMain.Cards;
using NSMain.Notes;
using NSMain.Scenes;
using NSMain.Searcher;
using NSMain.Tools;
using NSMain.TreeViewPlus;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using static NSMain.TreeViewPlus.CNodeModule;

namespace NSMain.Bricks
{
    /// <summary>
    /// 公共变量和操作对象
    /// </summary>
    class GlobalVal
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

            public static string Introduction;

            public static long CurrentYear;

            public static TreeViewNode CurNode;
        }

        public struct Flag
        {
            public static bool IsSqlconnOpening;

            public static bool Loading;
        }

        public struct Uc
        {
            public static MainWindow MainWin;
            public static SplashWindow SpWin;
            public static Grid BooksPanel;
            public static HandyControl.Controls.TabControl TabControl;
            public static USearcher Searcher;
            public static UTreeViewPlus TreeMaterial;
            public static UTreeViewPlus TreeBook;
            public static UTreeViewPlus TreeHistory;
            public static UTreeViewPlus TreeTask;
            public static WNotes Notes;
            public static WScenes Scenes;
            public static WNewProjects NewProjects;
            public static UCards RoleCards;
            public static UCards OtherCards;
            public static UCards WorldCards;
            public static UCards PublicRoleCards;
            public static UCards PublicOtherCards;
            public static UCards PublicWorldCards;
        }

        public struct SQLClass
        {
            public static Dictionary<string, CSqlitePlus> Pools = new Dictionary<string, CSqlitePlus>();
        }

        public struct Threads
        {
            public static Thread Task1;
        }

        public static SplashScreen SpScreen;


    }
}
