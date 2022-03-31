using MVVMAPP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMAPP.Models
{
    /// <summary>
    /// 书籍类
    /// </summary>
    public class Book : NotificationObject
    {
        public Book()
        {
            Initialize();
        }

        /// <summary>
        /// 向外暴露的初始化方法，清空RootNode属性以生成一个新的书籍对象
        /// </summary>
        public void Initialize()
        {
            foreach (var item in RootNodes)
            {
                item.Clear();
            }
        }

        private string _uid;

        public string Uid
        {
            get { return _uid; }
            set
            {
                _uid = value;
                this.RaisePropertyChanged("Uid");
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        private double _pirce;

        public double Price
        {
            get { return _pirce; }
            set
            {
                _pirce = value;
                this.RaisePropertyChanged("Price");
            }
        }

        private long _currentYear;

        public long CurrentYear
        {
            get { return _currentYear; }
            set
            {
                _currentYear = value;
                this.RaisePropertyChanged("CurrentYear");
            }
        }

        private string _introduce;

        public string Introduce
        {
            get { return _introduce; }
            set
            {
                _introduce = value;
                this.RaisePropertyChanged("Introduce");
            }
        }

        private string _coverpath;

        public string CoverPath
        {
            get { return _coverpath; }
            set
            {
                _coverpath = value;
                this.RaisePropertyChanged("CoverPath");
            }
        }

        private static ObservableCollection<Node> rootNode0 = new ObservableCollection<Node>();
        private static ObservableCollection<Node> rootNode1 = new ObservableCollection<Node>();
        private static ObservableCollection<Node> rootNode2 = new ObservableCollection<Node>();

        public ObservableCollection<ObservableCollection<Node>> RootNodes { get; set; } = new ObservableCollection<ObservableCollection<Node>> { rootNode0, rootNode1, rootNode2 };
    }
}
