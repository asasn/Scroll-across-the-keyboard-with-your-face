using MVVMAPP.Commands;
using MVVMAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAPP.ViewModels
{
    public class ChooseBookWindowViewModel
    {

        public Book CurrentBook { get; set; } = new Book();

        public ChooseBookWindowViewModel(Book currentBook)
        {
            CurrentBook = currentBook;

            this.CommondButton_Click = new DelegateCommand();
            this.CommondButton_Click.ExecutAction = new Action<object>(this.Button_Click);
        }

        public DelegateCommand CommondButton_Click { get; set; }
        private void Button_Click(object parameter)
        {
            CurrentBook.Name = parameter.ToString();
        }

    }
}
