﻿using System;
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
        public Node(string name)
        {
            NodeName = name;
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
