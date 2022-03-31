using MVVMAPP.Commands;
using MVVMAPP.Models;
using MVVMAPP.Services;
using MVVMAPP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MVVMAPP.ViewModels
{
    public class MainWindowViewModel : NotificationObject
    {
        private static Book _currentBook = new Book();

        public Book CurrentBook
        {
            get { return _currentBook; }
            set
            {
                _currentBook = value;
                this.RaisePropertyChanged("CurrentBook");
            }
        }

        private readonly BookService Service = new BookService(_currentBook);


        public DelegateCommand CommondTabControl_Loaded { get; set; }
        private void TabControl_Loaded(object parameter)
        {
            TabControl_SelectionChanged(parameter);
        }

        public DelegateCommand CommondTabControl_SelectionChanged { get; set; }
        private void TabControl_SelectionChanged(object parameter)
        {
            if (parameter.ToString() == "0")
            {
                Service.LoadToTree0(null);
            }
            if (parameter.ToString() == "1")
            {
                Service.LoadToTree1(null);
            }
            if (parameter.ToString() == "2")
            {
                Service.LoadToTree2(null);
            }
        }

        public DelegateCommand CommondButton_Click { get; set; }
        private void Button_Click(object parameter)
        {
            CurrentBook.Name = DateTime.Now.ToString();
            Window win = new ChooseBookWindow();
            win.DataContext = new ChooseBookWindowViewModel(CurrentBook);
            win.ShowDialog();
        }

        public MainWindowViewModel()
        {
            this.CommondTabControl_Loaded = new DelegateCommand();
            this.CommondTabControl_Loaded.ExecutAction = new Action<object>(this.TabControl_Loaded);
            this.CommondTabControl_SelectionChanged = new DelegateCommand();
            this.CommondTabControl_SelectionChanged.ExecutAction = new Action<object>(this.TabControl_SelectionChanged);
            this.CommondButton_Click = new DelegateCommand();
            this.CommondButton_Click.ExecutAction = new Action<object>(this.Button_Click);
        }


    }
}
