using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version4.Model;

namespace Version4.ViewModel
{
    public class BookChooseWindowViewModel : MainWindowViewModel
    {
        public BookChooseWindowViewModel()
        {
            
        }

        public void SelectBook(Book cbook)
        {
            CurrentBook = cbook;
        }
    }
}
