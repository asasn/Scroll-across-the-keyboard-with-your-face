using RootNS.Brick;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RootNS.Model
{
    public static class Gval
    {
        /// <summary>
        /// 程序路径
        /// </summary>
        public struct Path
        {
            public static string App { get { return Environment.CurrentDirectory; } }

            public static string Books { get { return Environment.CurrentDirectory + "/books"; } }

            public static string Resourses { get { return Environment.CurrentDirectory + "/Resourses"; } }
        }

        public static Book CurrentBook { get; set; } = new Book();
        public static Material Material { get; set; } = new Material();
        public static ObservableCollection<Book> BooksBank { get; set; } = new ObservableCollection<Book>();

        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 打开文档的集合
        /// </summary>
        public static ObservableCollection<Node> OpenedDocList { set; get; } = new ObservableCollection<Node>();

        public static HandyControl.Controls.TabControl EditorTabControl;
    }
}
