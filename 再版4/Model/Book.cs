using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Version4.Model
{
    public class Book : BaseBook
    {
        public Book()
        {
            this.PropertyChanged += Book_PropertyChanged;
        }

        private void Book_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                string imgPath = AppInfo.PathBooks + "/" + Name + ".jpg";
                if (System.IO.File.Exists(imgPath) == true)
                {
                    this.CoverPath = imgPath;
                }
                else
                {
                    this.CoverPath = "../Resourses/nullbookface.jpg";
                }
            }
        }

        private double _price;
        /// <summary>
        /// 书籍单价
        /// </summary>
        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                RaisePropertyChanged(nameof(Price));
            }
        }

        private long _currentYear;
        /// <summary>
        /// 书中的当前年份
        /// </summary>
        public long CurrentYear
        {
            get { return _currentYear; }
            set
            {
                _currentYear = value;
                RaisePropertyChanged(nameof(CurrentYear));
            }
        }


        private string _coverPath = string.Empty;
        /// <summary>
        /// 封面路径
        /// </summary>
        public string CoverPath
        {
            get { return _coverPath; }
            set
            {
                _coverPath = value;
                RaisePropertyChanged(nameof(CoverPath));
            }
        }

        private SolidColorBrush? _selectedBrush = Brushes.WhiteSmoke;
        /// <summary>
        /// 选中时的书籍卡片边框颜色
        /// </summary>
        public SolidColorBrush? SelectedBrush
        {
            get { return _selectedBrush; }
            set
            {
                _selectedBrush = value;
                RaisePropertyChanged(nameof(SelectedBrush));
            }
        }
    }
}
