using RootNS.Brick;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RootNS.Model
{
    public class Gval : INotifyPropertyChanged
    {
        /// <summary>
        /// 静态事件处理属性更改
        /// </summary>
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 程序路径
        /// </summary>
        public struct Path
        {
            public static string App { get { return Environment.CurrentDirectory; } }

            public static string Books { get { return Environment.CurrentDirectory + "/books"; } }

            public static string Resourses { get { return Environment.CurrentDirectory + "/Resourses"; } }
        }



        private static Book _currentBook = new Book();

        public static Book CurrentBook
        {
            get { return _currentBook; }
            set
            {
                _currentBook = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(CurrentBook)));
            }
        }


        private static Material _materialBook = new Material();

        public static Material MaterialBook
        {
            get { return _materialBook; }
            set
            {
                _materialBook = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(MaterialBook)));
            }
        }

        private static ObservableCollection<Book> _booksBank = new ObservableCollection<Book>();

        public static ObservableCollection<Book> BooksBank
        {
            get { return _booksBank; }
            set
            {
                _booksBank = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(BooksBank)));
            }
        }

        private static string _workSpace;

        public static string WorkSpace
        {
            get { return _workSpace; }
            set
            {
                _workSpace = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(WorkSpace)));
            }
        }

        private static string _tableName;

        public static string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(TableName)));
            }
        }




        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }


        private static ObservableCollection<Node> _openedDocList = new ObservableCollection<Node>();
        /// <summary>
        /// 打开文档的集合
        /// </summary>
        public static ObservableCollection<Node> OpenedDocList
        {
            get { return _openedDocList; }
            set
            {
                _openedDocList = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(OpenedDocList)));
            }
        }





        public static HandyControl.Controls.TabControl EditorTabControl;
    }
}
