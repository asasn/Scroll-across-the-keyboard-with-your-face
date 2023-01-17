﻿using RootNS.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            if (this.OwnerName == null ||
                Gval.FlagLoadingCompleted == false ||
               (this.IsDir == false && e.PropertyName == nameof(IsExpanded))
               )
            {
                return;
            }
            if (e.PropertyName == nameof(Title) && ReNameing == false && this.GetType() == typeof(Node))
            {
                DataOut.UpdateNodeProperty(this, nameof(Title), this.Title);
            }
            if (e.PropertyName == nameof(Pid) ||
                e.PropertyName == nameof(IsChecked) ||
                e.PropertyName == nameof(IsExpanded))
            {
                object propertyValue = this.GetType().GetProperty(e.PropertyName).GetValue(this, null);
                DataOut.UpdateNodeProperty(this, e.PropertyName, propertyValue.ToString());
            }
            if (e.PropertyName == nameof(IsDir) ||
                e.PropertyName == nameof(IsExpanded))
            {
                RefreshIcon(this);
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

        private string _uid = Guid.NewGuid().ToString();
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

        private string _iconString = "\ue855";
        /// <summary>
        /// 图标字符串
        /// </summary>
        public string IconString
        {
            get { return _iconString; }
            set
            {
                _iconString = value;
                RaisePropertyChanged(nameof(IconString));
            }
        }


        private string RefreshIcon(Node node)
        {
            //if (node.TabName == Book.NoteTabName.大事记.ToString())
            //{
            //    return node.IconString = "\ue88e";
            //}
            //if (node.TabName == Book.NoteTabName.故事.ToString())
            //{
            //    return node.IconString = "\ue857";
            //}
            //if (node.TabName == Book.NoteTabName.场景.ToString())
            //{
            //    return node.IconString = "\ue843";
            //}
            //if (node.TabName == Book.NoteTabName.线索.ToString())
            //{
            //    return node.IconString = "\ue820";
            //}
            if (node.IsDir == true)
            {
                if (node.IsExpanded == true)
                {
                    return node.IconString = "\ue80e";
                }
                else
                {
                    return node.IconString = "\ue80f";
                }
            }
            else
            {
                return node.IconString = "\ue855";
            }
        }


        private string _pid = String.Empty;
        /// <summary>
        /// 父节点标识码
        /// </summary>
        public virtual string Pid
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



        private Node _parent;
        /// <summary>
        /// 父节点对象
        /// </summary>
        public Node Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                this.RaisePropertyChanged("Parent");
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

        /// <summary>
        /// （供搜索功能使用的临时变量）跳转至xx行
        /// </summary>
        public int ToLineNum { get; set; }

        /// <summary>
        /// （供搜索功能使用的临时变量）匹配的字符串数组
        /// </summary>
        public string[] Matches { get; set; }

        /// <summary>
        /// （供搜索功能使用的临时变量）临时标题
        /// </summary>
        public string TempTitle { get; set; }


        private object _tempToolTip;
        /// <summary>
        /// （供搜索功能使用的临时变量）悬浮显示内容
        /// </summary>
        public object TempToolTip
        {
            get { return _tempToolTip; }
            set
            {
                _tempToolTip = value;
                RaisePropertyChanged(nameof(TempToolTip));
            }
        }



        private string _text = String.Empty;
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

        private object _owner;

        public object Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                this.RaisePropertyChanged(nameof(Owner));
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


        private string _ownerName;
        /// <summary>
        /// 页面名称
        /// </summary>
        public string OwnerName
        {
            get { return _ownerName; }
            set
            {
                _ownerName = value;
                this.RaisePropertyChanged(nameof(OwnerName));
            }
        }

        private bool _reNameing;
        /// <summary>
        /// 是否正在命名状态
        /// </summary>
        public bool ReNameing
        {
            get { return _reNameing; }
            set
            {
                _reNameing = value;
                this.RaisePropertyChanged(nameof(ReNameing));
            }
        }

        public void FinishRename()
        {
            this.ReNameing = false;
        }


        private string _summary = String.Empty;
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
        /// <summary>
        /// 根节点
        /// </summary>
        public Node RootNode
        {
            get { return _rootNode; }
            set
            {
                _rootNode = value;
                this.RaisePropertyChanged("RootNode");
            }
        }


        private Node _previousNode;
        /// <summary>
        /// 同级集合中的上一个节点
        /// </summary>
        public Node PreviousNode
        {
            get { return _previousNode; }
            set
            {
                _previousNode = value;
                this.RaisePropertyChanged(nameof(PreviousNode));
            }
        }

        private Node _nextNode;
        /// <summary>
        /// 同级集合中的下一个节点
        /// </summary>
        public Node NextNode
        {
            get { return _nextNode; }
            set
            {
                _nextNode = value;
                this.RaisePropertyChanged(nameof(NextNode));
            }
        }

        private object _extra;
        /// <summary>
        /// 附件
        /// </summary>
        public object Extra
        {
            get { return _extra; }
            set
            {
                _extra = value;
                RaisePropertyChanged(nameof(Extra));
            }
        }


        /// <summary>
        /// 从当前节点添加子节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public Node AddChildNode(Node node)
        {
            if (node.Title == null)
            {
                node.Title = "新" + this.TabName;
            }
            //this.IsExpanded = true;
            this.ChildNodes.Add(node);
            DataOut.CreateNewNode(node);
            node.IsSelected = true;
            return node;
        }

        /// <summary>
        /// 删除节点以及节点之下的所有子节点
        /// </summary>
        public void RealRemoveItSelfAndAllChildNodes()
        {
            if (this.Parent != null)
            {
                string sqlDel = string.Empty;
                ArrayList arrayList = new ArrayList();
                GelChildNodesListByDeleteNode(this, arrayList);
                this.Parent.ChildNodes.Remove(this);
                sqlDel += string.Format("DELETE FROM {0} WHERE Uid='{1}';", this.TabName, this.Uid);
                foreach (var item in arrayList)
                {
                    sqlDel += string.Format("DELETE FROM {0} WHERE Uid='{1}';", (item as Node).TabName, (item as Node).Uid);
                }
                SqliteHelper.PoolDict[this.OwnerName].ExecuteNonQuery(sqlDel);
                string sql = String.Empty;
                for (int i = 0; i < this.Parent.ChildNodes.Count; i++)
                {
                    this.Parent.ChildNodes[i].Index = i;
                    sql += string.Format("UPDATE {0} SET [{1}]='{2}' WHERE Uid='{3}' AND EXISTS(select * from sqlite_master where name='{0}' and sql like '%{1}%');", this.TabName.Replace("'", "''"), "Index", i, this.Parent.ChildNodes[i].Uid);
                }
                SqliteHelper.PoolDict[this.Parent.OwnerName].ExecuteNonQuery(sql);
            }
            ReSelect();
        }

        public void ChangeDelFlag(bool flag)
        {
            ArrayList arrayList = new ArrayList();
            GetChildNodesListByChangeNode(this, arrayList);
            string sqlDel = string.Empty;
            this.IsDel = flag;
            sqlDel += string.Format("UPDATE {0} SET [{1}]='{2}' WHERE Uid='{3}' AND EXISTS(select * from sqlite_master where name='{0}' and sql like '%{1}%');", this.TabName.Replace("'", "''"), nameof(Node.IsDel), this.IsDel, this.Uid);
            foreach (var item in arrayList)
            {
                (item as Node).IsDel = flag;
                sqlDel += string.Format("UPDATE {0} SET [{1}]='{2}' WHERE Uid='{3}' AND EXISTS(select * from sqlite_master where name='{0}' and sql like '%{1}%');", (item as Node).TabName.Replace("'", "''"), nameof(Node.IsDel), (item as Node).IsDel, (item as Node).Uid);
            }
            SqliteHelper.PoolDict[this.OwnerName].ExecuteNonQuery(sqlDel);
        }

        /// <summary>
        /// 获取改变的子节点列表
        /// </summary>
        /// <param name="selectedSection"></param>
        private void GetChildNodesListByChangeNode(Node curNode, ArrayList arrayList)
        {
            for (int i = 0; i < curNode.ChildNodes.Count; i++)
            {
                Node stuff = curNode.ChildNodes[i];
                GetChildNodesListByChangeNode(stuff, arrayList);
                arrayList.Add(stuff);
            }
        }

        /// <summary>
        /// 获取删除的子节点列表
        /// </summary>
        /// <param name="selectedSection"></param>
        private void GelChildNodesListByDeleteNode(Node curNode, ArrayList arrayList)
        {
            for (int i = 0; i < curNode.ChildNodes.Count;)
            {
                Node stuff = curNode.ChildNodes[curNode.ChildNodes.Count - 1];
                GelChildNodesListByDeleteNode(stuff, arrayList);
                curNode.ChildNodes.Remove(stuff);
                arrayList.Add(stuff);
            }
        }

        /// <summary>
        /// 删除当前节点值后重新选择新节点
        /// </summary>
        private void ReSelect()
        {
            int i = 0;
            if (this.Parent.ChildNodes.Count > 0)
            {
                if (this.Index >= 0)
                {
                    i = this.Index;
                }
                if (this.Index == this.Parent.ChildNodes.Count)
                {
                    i = this.Index - 1;
                }
                this.Parent.ChildNodes[i].IsSelected = true;
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
                this.IconString = RefreshIcon(this);
                stuff.IconString = RefreshIcon(stuff);
                stuff.Pid = this.Uid;
                stuff.TabName = this.TabName;
                stuff.OwnerName = this.OwnerName;
                stuff.Owner = this.Owner;
                stuff.Parent = this;
                stuff.Index = this.ChildNodes.IndexOf(stuff);
                this.WordsCount += 1;
                if (stuff.Index > 0)
                {
                    stuff.PreviousNode = this.ChildNodes[stuff.Index - 1];
                    stuff.PreviousNode.NextNode = stuff;
                }
                if (this.Parent == null)
                {
                    stuff.RootNode = this;
                }
                else
                {
                    stuff.RootNode = this.RootNode;
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Node stuff = (Node)e.OldItems[0];
                this.WordsCount -= 1;
            }
        }

        /// <summary>
        /// 向下递归改变子节点标记
        /// </summary>
        public void CheckChildNodes()
        {
            CheckChildNodes(this);
        }

        private void CheckChildNodes(Node cNode)
        {
            foreach (Node node in cNode.ChildNodes)
            {
                CheckChildNodes(node);
                node.IsChecked = cNode.IsChecked;
            }
            cNode.IsExpanded = !cNode.IsChecked;
        }

        /// <summary>
        /// 向上改变父节点标记
        /// </summary>
        public void CheckParentNodes()
        {
            CheckParentNodes(this);
        }

        private void CheckParentNodes(Node cNode)
        {
            if (cNode.IsChecked == false)
            {
                while (cNode.Parent != null)
                {
                    cNode = cNode.Parent as Node;
                    cNode.IsChecked = false;
                }
            }
            else
            {
                bool tag = true;
                //兄弟节点当中有任意一个未选择，则改变标志
                foreach (Node node in ((cNode.Parent as Node)).ChildNodes)
                {
                    if (node.IsChecked == false)
                    {
                        tag = false;
                        break;
                    }
                }
                //根据标志改变父节点选中状态
                (cNode.Parent as Node).IsChecked = tag;
                (cNode.Parent as Node).IsExpanded = !tag;
            }
        }


        /// <summary>
        /// 添加至指定根节点的树末尾
        /// </summary>
        /// <param name="rootNode">指定根节点</param>
        public void AddToTreeEnd(Node rootNode)
        {
            if (rootNode.ChildNodes.Count > 0 && rootNode.ChildNodes.Last<Node>().IsDir == true)
            {
                rootNode.ChildNodes.Last<Node>().AddChildNode(this);
            }
            else
            {
                rootNode.AddChildNode(this);
            }
        }

        public void MoveUp()
        {
            if (this.Index <= 0)
            {
                return;
            }
            UpDown(this.Index, this.Index - 1);
        }

        public void MoveDown()
        {
            if (this.Index >= (this.Parent as Node).ChildNodes.Count - 1)
            {
                return;
            }
            UpDown(this.Index, this.Index + 1);
        }

        private void UpDown(int a, int b)
        {
            this.Parent.ChildNodes.Move(a, b);
            this.Parent.ChildNodes[a].Index = a;
            this.Parent.ChildNodes[b].Index = b;
            DataOut.UpdateNodeProperty(this.Parent.ChildNodes[a], "Index", a.ToString());
            DataOut.UpdateNodeProperty(this.Parent.ChildNodes[b], "Index", (b).ToString());
        }

        public void DragDropNode(Node dropNode)
        {
            this.RealRemoveItSelfAndAllChildNodes();
            dropNode.AddChildNode(this);
        }

        public void Import()
        {
            Node selectedNode;
            if (this.IsDir == false)
            {
                if (this.Parent == null)
                {
                    if (this.TabName.Contains("草稿") || this.TabName.Contains("作品相关"))
                    {
                        selectedNode = this;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    selectedNode = this.Parent;
                }
            }
            else
            {
                selectedNode = this;
            }
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "文本文件(*.txt, *.book)|*.txt;*.book|所有文件(*.*)|*.*",
                Multiselect = true
            };

            string[] files;
            // 打开选择框选择
            if (dlg.ShowDialog() == true)
            {
                files = dlg.FileNames;
            }
            else
            {
                return;
            }
            string sqlImport = string.Empty;
            foreach (string srcFullFileName in files)
            {
                string title = System.IO.Path.GetFileNameWithoutExtension(srcFullFileName);

                Node newNode = new Node
                {
                    Title = title,
                    Text = IOHelper.ReadFromTxt(srcFullFileName)
                };
                newNode.WordsCount = EditorHelper.CountWords(newNode.Text);
                selectedNode.ChildNodes.Add(newNode);
                sqlImport += string.Format("INSERT OR IGNORE INTO {0} ([Index], Uid, Pid, Title, Text, Summary, WordsCount, IsDir, IsExpanded, IsChecked, IsDel) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');", newNode.TabName.Replace("'", "''"), newNode.Index, newNode.Uid, newNode.Pid, newNode.Title.Replace("'", "''"), newNode.Text.Replace("'", "''"), newNode.Summary.Replace("'", "''"), newNode.WordsCount, newNode.IsDir, newNode.IsExpanded, newNode.IsChecked, newNode.IsDel);
            }
            SqliteHelper.PoolDict[this.OwnerName].ExecuteNonQuery(sqlImport);
        }

        public void Export()
        {
            Node selectedNode = this;
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            folder.Description = "选择文件所在文件夹目录";  //提示的文字
            folder.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (selectedNode.IsDir == true)
                {
                    string path = String.Format("{0}/{1}", folder.SelectedPath, selectedNode.Title);
                    if (IOHelper.IsFolderExists(path) == false)
                    {
                        IOHelper.CreateFolder(path);
                    }
                    foreach (Node node in selectedNode.ChildNodes)
                    {
                        ExportMethod(node, path);
                    }
                }
                else
                {
                    ExportMethod(selectedNode, folder.SelectedPath);
                }
            }
        }

        private void ExportMethod(Node node, string path)
        {
            string fullFileName = String.Format("{0}/{1}.txt", path, node.Title);
            int n = 1;
            while (IOHelper.IsFileExists(fullFileName) == true)
            {
                fullFileName = String.Format("{0}/{1} - {2}.txt", path, node.Title, n);
                n++;
            }
            IOHelper.WriteToTxt(fullFileName, node.Text);
        }


    }
}
