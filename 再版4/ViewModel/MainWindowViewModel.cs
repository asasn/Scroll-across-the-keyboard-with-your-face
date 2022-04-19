using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Version4.Model;

namespace Version4.ViewModel
{
    public class MainWindowViewModel : NotificationObject
    {


        public MainWindowViewModel()
        {
            CurrentBook = new();
            CurrentBook.Name = "测试";
            this.PropertyChanged += VmMainWindow_PropertyChanged;
            BooksBank.Add(new Book() { Name = "测试1" });
            BooksBank.Add(new Book() { Name = "测试2" });
            BooksBank.Add(new Book() { Name = "测试3" });
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

        private string _windowTitle = AppInfo.WindowTitle;

        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                RaisePropertyChanged(nameof(WindowTitle));
            }
        }


        private Book? _currentBook;

        public Book? CurrentBook
        {
            get { return _currentBook; }
            set
            {
                _currentBook = value;
                RaisePropertyChanged(nameof(CurrentBook));
            }
        }

        private Book? _material;

        public Book? Material
        {
            get { return _material; }
            set
            {
                _material = value;
                RaisePropertyChanged(nameof(Material));
            }
        }

    }
}
