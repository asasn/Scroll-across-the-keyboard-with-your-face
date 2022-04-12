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


        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
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

        private static Node _currentDoc;
        /// <summary>
        /// 当前打开的文档
        /// </summary>
        public static Node CurrentDoc
        {
            get { return _currentDoc; }
            set
            {
                _currentDoc = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(CurrentDoc)));
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
