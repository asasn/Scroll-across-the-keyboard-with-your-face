using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NSMain.TreeViewPlus
{
    public class CNodeModule
    {

        #region 节点模型
        public class TreeViewNode : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                //未简化的委托调用
                //PropertyChangedEventHandler handler = PropertyChanged;
                //if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

                //if (this.isButton == false && GlobalVal.Flag.Loading == false)
                //{
                //    if (this.IsDir == true && propertyName == "IsExpanded")
                //    {
                //        if (this.IsExpanded == true)
                //        {
                //            this.IconPath = GlobalVal.Path.Resourses + "/图标/目录树/ic_action_folder_open.png";
                //        }
                //        else
                //        {
                //            this.IconPath = GlobalVal.Path.Resourses + "/图标/目录树/ic_action_folder_closed.png";
                //        }
                //    }
                //}

            }

            #region 构造函数
            public TreeViewNode(string _nodeContent = "")
            {
                this.nodeContent = _nodeContent;
            }

            public TreeViewNode(string _uid, string _nodeName, string _nodeContent = "", bool _isDir = false, bool _isButton = false, bool _isExpanded = false, bool _isSelected = false, bool _isChecked = false)
            {
                this.uid = _uid;
                this.nodeName = _nodeName;
                this.nodeContent = _nodeContent;
                this.isDir = _isDir;
                this.isButton = _isButton;
                this.isExpanded = _isExpanded;
                this.isSelected = _isSelected;
                this.isChecked = _isChecked;
            }
            #endregion

            #region 属性字段
            private string uid;
            /// <summary>
            /// 节点ID
            /// </summary>
            public string Uid
            {
                get
                {
                    return uid;
                }
                set
                {
                    uid = value;
                }
            }

            private string pid;
            /// <summary>
            /// 父节点ID
            /// </summary>
            public string Pid
            {
                get
                {
                    return pid;
                }
                set
                {
                    pid = value;
                }
            }

            private TreeViewNode parentNode;
            /// <summary>
            /// 父节点
            /// </summary>
            public TreeViewNode ParentNode
            {
                get
                {
                    return parentNode;
                }
                set
                {
                    parentNode = value;
                }
            }

            private string nodeName;
            /// <summary>
            /// 节点名称
            /// </summary>
            public string NodeName
            {
                get
                {
                    return nodeName;
                }
                set
                {
                    nodeName = value;
                    OnPropertyChanged("NodeName");
                }
            }

            private string nodeContent;
            /// <summary>
            /// 节点内容
            /// </summary>
            public string NodeContent
            {
                get
                {
                    return nodeContent;
                }
                set
                {
                    nodeContent = value;
                    OnPropertyChanged("NodeContent");
                }
            }

            private int wordsCount;
            /// <summary>
            /// 节点字数
            /// </summary>
            public int WordsCount
            {
                get
                {
                    return wordsCount;
                }
                set
                {
                    wordsCount = value;
                    OnPropertyChanged("WordsCount");
                }
            }

            private string iconPath;
            /// <summary>
            /// 节点图标路径
            /// </summary>
            public string IconPath
            {
                get
                {
                    return iconPath;
                }
                set
                {
                    iconPath = value;
                    OnPropertyChanged("IconPath");
                }
            }

            private bool isDir;
            /// <summary>
            /// 是否目录（默认false）
            /// </summary>
            public bool IsDir
            {
                get
                {
                    return isDir;
                }
                set
                {
                    isDir = value;
                }
            }

            private bool isExpanded;
            /// <summary>
            /// 是否展开（默认fale）
            /// </summary>
            public bool IsExpanded
            {
                get
                {
                    return isExpanded;
                }
                set
                {
                    isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }

            private bool isSelected;
            /// <summary>
            /// 是否选中节点（默认fale）
            /// </summary>
            public bool IsSelected
            {
                get
                {
                    return isSelected;
                }
                set
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }

            private bool? isChecked = false;
            /// <summary>
            /// 是否勾选（默认fale）
            /// </summary>
            public bool? IsChecked
            {
                get
                {
                    return isChecked;
                }
                set
                {
                    isChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }

            private bool isDel = false;
            /// <summary>
            /// 是否已经被删除（默认fale）
            /// </summary>
            public bool IsDel
            {
                get
                {
                    return isDel;
                }
                set
                {
                    isDel = value;
                    OnPropertyChanged("IsDel");
                }
            }

            private bool isButton;
            /// <summary>
            /// 是否按钮
            /// </summary>
            public bool IsButton
            {
                get
                {
                    return isButton;
                }
                set
                {
                    isButton = value;
                }
            }

            private TreeViewItem theItem;
            /// <summary>
            /// 控件对象
            /// </summary>
            public TreeViewItem TheItem
            {
                get
                {
                    return theItem;
                }
                set
                {
                    theItem = value;
                    OnPropertyChanged("TheItem");
                }
            }

            #endregion

            private ObservableCollection<TreeViewNode> childNodes;
            /// <summary>
            /// 子节点数据
            /// </summary>
            public ObservableCollection<TreeViewNode> ChildNodes
            {
                get
                {
                    if (childNodes == null)
                    {
                        childNodes = new ObservableCollection<TreeViewNode>();
                        childNodes.CollectionChanged += new NotifyCollectionChangedEventHandler(OnMoreStuffChanged);
                    }
                    return childNodes;
                }
                set
                {
                    childNodes = value;
                }
            }

            private void OnMoreStuffChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    TreeViewNode stuff = (TreeViewNode)e.NewItems[0];
                    this.isDir = true;
                    stuff.Pid = this.Uid;
                    stuff.ParentNode = this;
                    this.WordsCount += 1;
                    //if (stuff.isButton == true)
                    //{
                    //    stuff.IconPath = GlobalVal.Path.Resourses + "/图标/目录树/ic_action_add.png";
                    //}
                    //else
                    //{
                    //    if (stuff.isDir == true)
                    //    {
                    //        if (stuff.IsExpanded == true)
                    //        {
                    //            stuff.IconPath = GlobalVal.Path.Resourses + "/图标/目录树/ic_action_folder_open.png";
                    //        }
                    //        else
                    //        {
                    //            stuff.IconPath = GlobalVal.Path.Resourses + "/图标/目录树/ic_action_folder_closed.png";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        stuff.iconPath = GlobalVal.Path.Resourses + "/图标/目录树/ic_action_document.png";
                    //    }
                    //}

                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    TreeViewNode stuff = (TreeViewNode)e.OldItems[0];
                    this.WordsCount -= 1;
                    if (stuff.Pid == this.Uid)
                    {
                        stuff.Pid = string.Empty;
                    }
                }
            }


        }
        #endregion

    }
}
