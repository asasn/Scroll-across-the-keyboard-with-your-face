using RootNS.Helper;
using RootNS.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static RootNS.Helper.DataPer;

namespace RootNS.Model
{
    public class Gval : NotificationObject
    {
        /// <summary>
        /// 静态事件处理属性更改
        /// </summary>
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;


        /// <summary>
        /// 程序路径
        /// </summary>
        public struct Path
        {
            public static string App { get { return Environment.CurrentDirectory; } }

            public static string Books { get { return Environment.CurrentDirectory + "/books"; } }
            public static string Resourses { get { return Environment.CurrentDirectory + "/Resourses"; } }
            public static string Assets { get { return Environment.CurrentDirectory + "/Assets"; } }

            public static string XshdPath { get { return Environment.CurrentDirectory +"/Assets/Text.xshd"; } }
        }

        private static string _homePage = "https://github.com/asasn/Scroll-across-the-keyboard-with-your-face";

        public static string HomePage
        {
            get { return _homePage; }
            set
            {
                _homePage = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(HomePage)));
            }
        }



        private static string _currentVersion = "1.0.0.0";

        public static string CurrentVersion
        {
            get { return _currentVersion; }
            set
            {
                _currentVersion = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(CurrentVersion)));
            }
        }

        private static string _appAuthor = "不问苍生问鬼神";

        public static string AppAuthor
        {   
            get { return _appAuthor; }
            set
            {
                _appAuthor = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(AppAuthor)));
            }
        }



        private static string _latestVersion = "no checked";

        public static string LatestVersion
        {
            get { return _latestVersion; }
            set
            {
                _latestVersion = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(LatestVersion)));
            }
        }


        public struct View
        {
            public static MainWindow MainWindow { get; set; }
            public static UcShower UcShower { get; set; }
            public static TabControl TabNote { get; set; }
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


        private static Material _materialBook = new Material() { Name = "index" };

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



        private static ObservableCollection<Node> _openingDocList = new ObservableCollection<Node>();
        /// <summary>
        /// 打开文档的集合
        /// </summary>
        public static ObservableCollection<Node> OpeningDocList
        {
            get { return _openingDocList; }
            set
            {
                _openingDocList = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(OpeningDocList)));
            }
        }



        private static bool _flagLoadingCompleted;

        public static bool FlagLoadingCompleted
        {
            get { return _flagLoadingCompleted; }
            set
            {
                _flagLoadingCompleted = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(FlagLoadingCompleted)));
            }
        }



        private static HandyControl.Controls.TabControl _editorTabControl;
        /// <summary>
        /// 编辑器页容器
        /// </summary>
        public static HandyControl.Controls.TabControl EditorTabControl
        {
            get { return _editorTabControl; }
            set
            {
                _editorTabControl = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(EditorTabControl)));
            }
        }


        private static TabControl _selectedChapterTab;

        public static TabControl SelectedChapterTab
        {
            get { return _selectedChapterTab; }
            set
            {
                _selectedChapterTab = value;
            }
        }

        private static TabControl _selectedNoteTab;

        public static TabControl SelectedNoteTab
        {
            get { return _selectedNoteTab; }
            set
            {
                _selectedNoteTab = value;
            }
        }

        private static TabControl _selectedCardTab;

        public static TabControl SelectedCardTab
        {
            get { return _selectedCardTab; }
            set
            {
                _selectedCardTab = value;
            }
        }



        private static TabControl _selectedMaterialTab;

        public static TabControl SelectedMaterialTab
        {
            get { return _selectedMaterialTab; }
            set
            {
                _selectedMaterialTab = value;
            }
        }


        private static TabControl _selectedPublicCardTab;

        public static TabControl SelectedPublicCardTab
        {
            get { return _selectedPublicCardTab; }
            set
            {
                _selectedPublicCardTab = value;
            }
        }




    }
}
