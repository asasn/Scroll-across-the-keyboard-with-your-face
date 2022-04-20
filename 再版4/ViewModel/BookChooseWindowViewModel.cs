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

        /// <summary>
        /// 选中书籍卡片，设置当前书籍对象
        /// </summary>
        /// <param name="cbook"></param>
        public void Choose(Book cbook)
        {
            CurrentBook = cbook;
        }


    }
}
