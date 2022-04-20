using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Version4.Helper;
using Version4.Model;
using Version4.View;

namespace Version4.ViewModel
{
    public class MainWindowViewModel : NotificationObject
    {


        public MainWindowViewModel()
        {
            this.PropertyChanged += VmMainWindow_PropertyChanged;
       
        }

        private void VmMainWindow_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentBook")
            {
                previousBook.SelectedBrush = Brushes.WhiteSmoke;
                previousBook = CurrentBook;
                previousBook.SelectedBrush = Brushes.Orange;
            }
        }




        public void ShowChooseWindow()
        {
            BookChooseWindow bcw = new();
            bcw.ShowDialog();
        }


        private Book previousBook { get; set; } = new();

        private ObservableCollection<Book> _booksBank = new();
        /// <summary>
        /// 书库对象
        /// </summary>
        public ObservableCollection<Book> BooksBank
        {
            get { return _booksBank; }
            set
            {
                _booksBank = value;
                RaisePropertyChanged(nameof(BooksBank));
            }
        }

        private string _windowTitle = AppMain.WindowTitle;
        /// <summary>
        /// 窗口标题
        /// </summary>
        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                RaisePropertyChanged(nameof(WindowTitle));
            }
        }


        private Book _currentBook = new();
        /// <summary>
        /// 当前书籍
        /// </summary>
        public Book CurrentBook
        {
            get { return _currentBook; }
            set
            {
                _currentBook = value;
                RaisePropertyChanged(nameof(CurrentBook));
            }
        }

        private Material _materialBook = new();
        /// <summary>
        /// 资料库（书籍：index.db）
        /// </summary>
        public Material MaterialBook
        {
            get { return _materialBook; }
            set
            {
                _materialBook = value;
                RaisePropertyChanged(nameof(MaterialBook));
            }
        }

    }
}
