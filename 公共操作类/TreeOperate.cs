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

namespace 脸滚键盘.公共操作类
{
    class TreeOperate
    {
        #region 节点模型
        public class TreeViewNode : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
                if (this.isButton == false && Gval.Flag.Loading == false)
                {
                    if (this.IsDir == true && propertyName == "IsExpanded")
                    {
                        //SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, Gval.CurrentBook.Name + ".db");
                        //string sql = string.Format("UPDATE Tree_{0} set IsExpanded={1} where Uid = '{2}';", Gval.CurrentBook.Name, this.IsExpanded, this.Uid);
                        //sqlConn.ExecuteNonQuery(sql);

                        if (this.IsExpanded == true)
                        {
                            this.IconPath = Gval.Path.App + "/Resourse/ic_action_folder_open.png";
                        }
                        else
                        {
                            this.IconPath = Gval.Path.App + "/Resourse/ic_action_folder_closed.png";
                        }
                    }


                }

            }

            #region 构造函数
            public TreeViewNode()
            {
            }

            public TreeViewNode(string _uid, string _nodeName, bool _isDir = false, bool _isButton = false)
            {
                this.uid = _uid;
                this.nodeName = _nodeName;
                this.isDir = _isDir;
                this.isButton = _isButton;
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
            /// 节点图标
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
            /// 是否选中（默认fale）
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
                    if (stuff.isButton == true)
                    {
                        stuff.IconPath = Gval.Path.App + "/Resourse/ic_action_add.png";
                    }
                    else
                    {
                        if (stuff.isDir == true)
                        {
                            if (stuff.IsExpanded == true)
                            {
                                stuff.IconPath = Gval.Path.App + "/Resourse/ic_action_folder_open.png";
                            }
                            else
                            {
                                stuff.IconPath = Gval.Path.App + "/Resourse/ic_action_folder_closed.png";
                            }
                        }
                        else
                        {
                            stuff.iconPath = Gval.Path.App + "/Resourse/ic_action_document.png";
                        }
                    }

                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    TreeViewNode stuff = (TreeViewNode)e.OldItems[0];
                    if (stuff.Pid == this.Uid)
                    {
                        stuff.Pid = string.Empty;
                    }
                }
            }


        }
        #endregion

        #region 添加或者删除节点/按钮
        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="baseNode"></param>
        public static TreeViewNode AddNewNode(ObservableCollection<TreeViewNode> TreeViewNodeList, TreeViewNode baseNode)
        {
            string guid = Guid.NewGuid().ToString();
            TreeViewNode newNode = new TreeViewNode(guid, "新节点");
            newNode.NodeContent = "　　";
            int x;
            if (baseNode.ChildNodes.Count == 0)
            {
                x = 1;
            }
            else
            {
                x = baseNode.ChildNodes.Count;
            }
            if (string.IsNullOrEmpty(baseNode.Uid))
            {
                TreeViewNodeList.Insert(x - 1, newNode);
                AddButtonNode(TreeViewNodeList, newNode);
            }
            baseNode.ChildNodes.Insert(x - 1, newNode);
            return newNode;
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="TreeViewNodeList"></param>
        /// <param name="baseNode"></param>
        /// <returns></returns>
        public static TreeViewNode AddButtonNode(ObservableCollection<TreeViewNode> TreeViewNodeList, TreeViewNode baseNode)
        {
            TreeViewNode button;
            if (baseNode.Uid == "")
            {
                button = new TreeViewNode(null, "双击添加分卷", true, true);
            }
            else
            {
                button = new TreeViewNode(null, "双击添加章节", true, true);
            }
            if (string.IsNullOrEmpty(baseNode.Uid))
            {
                TreeViewNodeList.Add(button);
            }
            baseNode.ChildNodes.Add(button);
            return button;
        }

        #endregion

        #region MyRegion

        #endregion
        
    }
}
