using MVVMAPP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAPP.Models
{
    public class Node : NotificationObject
    {
        public Node(string name)
        {
            _nodeName = name;
            ChildNodes = new ObservableCollection<Node>();
        }

        private string _nodeName;
        /// <summary>
        /// 节点名称/章节标题
        /// </summary>
        public string NodeName
        {
            get { return _nodeName; }
            set
            {
                _nodeName = value;
                this.RaisePropertyChanged("NodeName");
            }
        }

        private string _uid;
        /// <summary>
        /// 自身标识码
        /// </summary>
        public string Uid
        {
            get { return _uid; }
            set
            {
                _uid = value;
                this.RaisePropertyChanged("Uid");
            }
        }

        private string _pid;
        /// <summary>
        /// 父id标识码
        /// </summary>
        public string Pid
        {
            get { return _pid; }
            set
            {
                _pid = value;
                this.RaisePropertyChanged("Pid");
            }
        }

        private Node _parentNode;

        public Node ParentNode
        {
            get { return _parentNode; }
            set
            {
                _parentNode = value;
                this.RaisePropertyChanged("ParentNode");
            }
        }


        private bool _isDir;
        /// <summary>
        /// 是否目录
        /// </summary>
        public bool IsDir
        {
            get { return _isDir; }
            set
            {
                _isDir = value;
                this.RaisePropertyChanged("IsDir");
            }
        }

        private string _content;
        /// <summary>
        /// 节点内容
        /// </summary>
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                this.RaisePropertyChanged("Content");
            }
        }

        private int _wordsCount;

        public int WordsCount
        {
            get { return _wordsCount; }
            set
            {
                _wordsCount = value;
                this.RaisePropertyChanged("WordsCount");
            }
        }

        private bool _isExpanded;
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                this.RaisePropertyChanged("IsExpanded");
            }
        }

        private bool _isChecked;
        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool IsChedked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                this.RaisePropertyChanged("IsChedked");
            }
        }

        private bool _selected;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                this.RaisePropertyChanged("Selected");
            }
        }


        private bool _isDel;
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel
        {
            get { return _isDel; }
            set
            {
                _isDel = value;
                this.RaisePropertyChanged("IsDel");
            }
        }


        private int _indexOfTabItem;
        /// <summary>
        /// 隶属的TabItem容器index
        /// </summary>
        public int IndexOfTabItem
        {
            get { return _indexOfTabItem; }
            set
            {
                _indexOfTabItem = value;
                this.RaisePropertyChanged("IndexOfTabItem");
            }
        }




        private ObservableCollection<Node> _childNodes = new ObservableCollection<Node>();

        public ObservableCollection<Node> ChildNodes
        {
            get
            {
                if (_childNodes == null)
                {
                    _childNodes = new ObservableCollection<Node>();
                    _childNodes.CollectionChanged += new NotifyCollectionChangedEventHandler(OnMoreStuffChanged);
                }
                return _childNodes;  }
            set
            {
                _childNodes = value;
                this.RaisePropertyChanged("ChildNodes");
            }
        }

        private void OnMoreStuffChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Node stuff = (Node)e.NewItems[0];
                this.IsDir = true;
                stuff.Pid = this.Uid;
                stuff.ParentNode = this;
                this.WordsCount += 1;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Node stuff = (Node)e.OldItems[0];
                this.WordsCount -= 1;
                stuff.IsDel = true;
            }
        }
    }
}
