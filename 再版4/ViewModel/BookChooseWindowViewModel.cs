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
            this.PropertyChanged += BookChooseWindowViewModel_PropertyChanged;
        }

        private void BookChooseWindowViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
          
        }


        private Book previousBook { get; set; } = new();

        /// <summary>
        /// 选中书籍卡片，设置当前书籍对象
        /// </summary>
        /// <param name="cbook"></param>
        public void Choose(Book cbook)
        {
            CurrentBook = cbook;
        }

        public void LoadButton(Book cbook)
        {
            if (cbook.Uid == CurrentBook.Uid)
            {
                CurrentBook = cbook;
            }
        }

    }
}
