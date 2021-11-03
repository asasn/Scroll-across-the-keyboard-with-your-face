using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //public static uc_Editor ucEditor;
        //public static uc_TreeBooks ucBooks;
        //public static uc_TreeMaterial ucMaterial;
    }
}
