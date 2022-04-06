using RootNS.Behavior;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Model
{
    public class Node : NotificationObject
    {
        public Node()
        {
            ChildNodes.CollectionChanged += new NotifyCollectionChangedEventHandler(OnMoreStuffChanged);
            this.PropertyChanged += Node_PropertyChanged;
        }

        private void Node_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsDel")
            {
                if (this.IsDel == true)
                {
                    foreach (Node node in this.ChildNodes)
                    {
                        node.IsDel = true;
                    }
                }
            }
        }

        private string _title;
        /// <summary>
        /// 节点标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        private string _uid;
        /// <summary>
        /// 节点标识码
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
        /// 父节点标识码
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

        private double _pointX;

        public double PointX
        {
            get { return _pointX; }
            set
            {
                _pointX = value;
                this.RaisePropertyChanged("PointX");
            }
        }

        private double _pointY;

        public double PointY
        {
            get { return _pointY; }
            set
            {
                _pointY = value;
                this.RaisePropertyChanged("PointY");
            }
        }



        private Node _parentNode;
        /// <summary>
        /// 父节点对象
        /// </summary>
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

        private string _text;
        /// <summary>
        /// 节点文字内容
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                this.RaisePropertyChanged("Text");
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
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                this.RaisePropertyChanged("IsChecked");
            }
        }

        private bool _isSelected;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.RaisePropertyChanged("IsSelected");
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

        private string _tabName;
        /// <summary>
        /// 页面名称
        /// </summary>
        public string TabName
        {
            get { return _tabName; }
            set
            {
                _tabName = value;
                this.RaisePropertyChanged(nameof(TabName));
            }
        }



        private int _index;
        /// <summary>
        /// 索引序号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                this.RaisePropertyChanged("Index");
            }
        }


        private string _summary;
        /// <summary>
        /// 摘要说明
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set
            {
                _summary = value;
                this.RaisePropertyChanged("Summary");
            }
        }

        private Node _rootNode;

        public Node RootNode
        {
            get { return _rootNode; }
            set
            {
                _rootNode = value;
                this.RaisePropertyChanged("RootNode");
            }
        }


        public Node AddChild(string title = "新节点")
        {
            Node node = new Node();
            node.Title = title;
            node.Uid = Gval.NewGuid();
            this.ChildNodes.Add(node);
            DataOut.CreateNewNode(node);
            return node;
        }

        public void RemoveItSelf(string title = "新节点")
        {
            if (this.ParentNode != null)
            {
                this.ParentNode.ChildNodes.Remove(this);
            }
        }

        private ObservableCollection<Node> _childNodes = new ObservableCollection<Node>();
        /// <summary>
        /// 子节点动态数据集合
        /// </summary>
        public ObservableCollection<Node> ChildNodes
        {
            get
            {
                if (_childNodes == null)
                {
                    _childNodes = new ObservableCollection<Node>();
                }
                return _childNodes;
            }
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
                stuff.TabName = this.TabName;
                stuff.ParentNode = this;
                stuff.Index = this.ChildNodes.Count - 1;
                Node pNode = stuff;
                if (this.IsDel == true)
                {
                    stuff.IsDel = true;
                }
                if (this.ParentNode == null)
                {
                    stuff.RootNode = this;
                }
                else
                {
                    stuff.RootNode = this.RootNode;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Node stuff = (Node)e.OldItems[0];
                this.WordsCount -= 1;
                stuff.IsDel = true;
                for (int i = stuff.Index; i < this.ChildNodes.Count; i++)
                {
                    this.ChildNodes[i].Index -= 1;
                }
            }
        }


    }
}
