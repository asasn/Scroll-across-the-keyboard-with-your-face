using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RootNS.Model
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
            BoxDraft.Clear();
            BoxTemp.Clear();
            BoxPublished.Clear();
            NoteOutline.Clear();
            NoteMemorabilia.Clear();
            NoteClues.Clear();
            NoteTemplate.Clear();
            CardRole.Clear();
            CardOther.Clear();
            CardWorld.Clear();
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

        #region 目录树
        public ObservableCollection<Node> BoxDraft { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> BoxTemp { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> BoxPublished { set; get; } = new ObservableCollection<Node>();
        #endregion

        #region 记事板
        public ObservableCollection<Node> NoteOutline { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> NoteMemorabilia { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> NoteClues { set; get; } = new ObservableCollection<Node>();
        public ObservableCollection<Node> NoteTemplate { set; get; } = new ObservableCollection<Node>();
        #endregion

        #region 信息卡
        public ObservableCollection<Card> CardRole { set; get; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> CardOther { set; get; } = new ObservableCollection<Card>();
        public ObservableCollection<Card> CardWorld { set; get; } = new ObservableCollection<Card>();
        #endregion

    }
}
