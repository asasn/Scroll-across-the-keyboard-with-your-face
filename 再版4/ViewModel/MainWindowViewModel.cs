using RootNS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private void VmMainWindow_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void BtnChoose()
        {
            CurrentBook.Name = "改变文字";
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
