using NSMain.Bricks;
using NSMain.Editor;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static NSMain.TreeViewPlus.CNodeModule;

namespace NSMain.TreeViewPlus
{
    /// <summary>
    /// UcTreeBook.xaml 的交互逻辑
    /// </summary>
    public partial class UTreeViewPlus : UserControl
    {
        #region 构造函数和载入方法
        public UTreeViewPlus()
        {
            InitializeComponent();
        }

        private void Tv_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(UTreeViewPlus), new PropertyMetadata(null));




        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(UTreeViewPlus), new PropertyMetadata(null));


        public string UcTitle
        {
            get { return (string)GetValue(UcTitleProperty); }
            set { SetValue(UcTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTitleProperty =
            DependencyProperty.Register("UcTitle", typeof(string), typeof(UTreeViewPlus), new PropertyMetadata(string.Empty));



        /// <summary>
        /// 数据源：节点列表
        /// </summary>
        public ObservableCollection<TreeViewNode> TreeViewNodeList
        {
            get { return (ObservableCollection<TreeViewNode>)GetValue(TreeViewNodeListProperty); }
            set { SetValue(TreeViewNodeListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TreeViewNodeList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TreeViewNodeListProperty =
            DependencyProperty.Register("TreeViewNodeList", typeof(ObservableCollection<TreeViewNode>), typeof(UTreeViewPlus), new PropertyMetadata(new ObservableCollection<TreeViewNode>()));




        public TreeViewNode TopNode
        {
            get { return (TreeViewNode)GetValue(TopNodeProperty); }
            set { SetValue(TopNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopNodeProperty =
            DependencyProperty.Register("TopNode", typeof(TreeViewNode), typeof(UTreeViewPlus), new PropertyMetadata(null));



        public TreeViewNode CurNode
        {
            get { return (TreeViewNode)GetValue(CurNodeProperty); }
            set { SetValue(CurNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurNodeProperty =
            DependencyProperty.Register("CurNode", typeof(TreeViewNode), typeof(UTreeViewPlus), new PropertyMetadata(null));



        public void LoadBook(string curBookName, string typeOfTree)
        {
            if (string.IsNullOrEmpty(curBookName))
            {
                return;
            }
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
            
            //数据初始化
            TreeViewNodeList = new ObservableCollection<TreeViewNode>();
            TopNode = new TreeViewNode { Uid = "", IsDir = true };

            //数据源加载节点列表
            Tv.ItemsSource = TreeViewNodeList;

            if (typeOfTree == "task")
            {

            }


            //AddButtonNode(TreeViewNodeList, TopNode);

            //从数据库中载入数据
            CTreeView.LoadBySql(CurBookName, TypeOfTree, TreeViewNodeList, TopNode);


            if (Tv != null)
            {
                Sv.ScrollToEnd();
            }
        }
        #endregion

        #region 字段和属性

        TextBox TbReName;
        TextBlock TbkName;
        #endregion




        /// <summary>
        /// 节点展开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tv_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewNode selectedNode = (e.OriginalSource as TreeViewItem).DataContext as TreeViewNode;
            if (selectedNode != null && selectedNode.IsDir == true && selectedNode.IsButton == false)
            {
                if (TypeOfTree == "book" || TypeOfTree == "material")
                {
                    selectedNode.IconPath = GlobalVal.Path.Resourses + "/图标/目录树/ic_action_folder_open.png";
                }
                CTreeView.ExpandedCollapsedBySql(CurBookName, TypeOfTree, selectedNode);
            }
        }

        /// <summary>
        /// 节点缩回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tv_Collapsed(object sender, RoutedEventArgs e)
        {
            TreeViewNode selectedNode = (e.OriginalSource as TreeViewItem).DataContext as TreeViewNode;
            if (selectedNode != null && selectedNode.IsDir == true && selectedNode.IsButton == false)
            {
                if (TypeOfTree == "book" || TypeOfTree == "material")
                {
                    selectedNode.IconPath = GlobalVal.Path.Resourses + "/图标/目录树/ic_action_folder_closed.png";
                }
                CTreeView.ExpandedCollapsedBySql(CurBookName, TypeOfTree, selectedNode);
            }
        }

        #region 节点选择/双击

        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = e.OriginalSource as TreeViewItem;
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            CurNode = selectedNode;

            //载入节点的数据上下文以便调用
            selectedNode.TheItem = selectedItem;

            //Grid grid = FindChild<Grid>(selectedItem as DependencyObject, "GridLine");
            //TextBlock showNameTextBox = FindChild<TextBlock>(selectedItem as DependencyObject, "TbkName");
            TextBox TbReName = CTreeView.FindChild<TextBox>(selectedItem as DependencyObject, "TbReName");
            TextBlock TbkName = CTreeView.FindChild<TextBlock>(selectedItem as DependencyObject, "TbkName");
            if (TbReName != null)
            {
                TbReName.Visibility = Visibility.Hidden;
                //TbkName.Visibility = Visibility.Visible;
                
            }


            //上下按钮可用/禁用
            if (selectedNode.IsButton == true)
            {
                GlobalVal.Uc.MainWin.BtnMoveUp.IsEnabled = false;
                GlobalVal.Uc.MainWin.BtnMoveDown.IsEnabled = false;
            }
            else
            {
                GlobalVal.Uc.MainWin.BtnMoveUp.Visibility = Visibility.Visible;
                GlobalVal.Uc.MainWin.BtnMoveDown.Visibility = Visibility.Visible;
                if (selectedNode.ParentNode.ChildNodes.IndexOf(selectedNode) == 0)
                {
                    GlobalVal.Uc.MainWin.BtnMoveUp.IsEnabled = false;
                }
                else
                {
                    GlobalVal.Uc.MainWin.BtnMoveUp.IsEnabled = true;
                }
                if (selectedNode.ParentNode.ChildNodes.IndexOf(selectedNode) == selectedNode.ParentNode.ChildNodes.Count - 1)
                {
                    GlobalVal.Uc.MainWin.BtnMoveDown.IsEnabled = false;
                }
                else
                {
                    GlobalVal.Uc.MainWin.BtnMoveDown.IsEnabled = true;
                }
            }
        }

        private void Tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewNode selectedNode = this.Tv.SelectedItem as TreeViewNode;
            TreeViewItem selectedItem = CTreeView.GetParentObjectEx<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;

            if (selectedNode != null)
            {
                if (selectedNode.IsButton == true)
                {
                    TreeViewNode newNode = CTreeView.AddNewNode(TreeViewNodeList, selectedNode.ParentNode, TypeOfTree);
                    CTreeView.AddNodeBySql(CurBookName, TypeOfTree, newNode);
                    newNode.IsSelected = true;
                    //TextBlock showNameTextBox = FindChild<TextBlock>(newNode.TheItem as DependencyObject, "showName");
                    //showNameTextBox.Visibility = Visibility.Hidden;
                    //reNameTextBox.Text = newNode.NodeName;
                    //reNameTextBox.SelectAll();
                    //reNameTextBox.Visibility = Visibility.Visible;
                    //reNameTextBox.Focus();
                }
                else
                {
                    if (selectedNode.IsDir == true)
                    {
                        //string tableName = TypeOfTree;
                        //SqliteOperate sqlConn = GlobalVal.SQLClass.Pools[curBookName];
                        //string sql = string.Format("UPDATE Tree_{0} set IsExpanded={1} where Uid = '{2}';", tableName, selectedNode.IsExpanded, selectedNode.Uid);
                        //sqlConn.ExecuteNonQuery(sql);
                        //
                    }
                    else
                    {
                        //非目录节点：手动进行一个改变，对抗双击引起的双击/展开
                        selectedNode.IsExpanded = !selectedNode.IsExpanded;

                        //获取节点对应的路径
                        string nodePath = CTreeView.GetPath(selectedNode);

                        if (TypeOfTree == "note" || TypeOfTree == "task")
                        {
                            return;
                        }

                        if (TypeOfTree == "material")
                        {
                            GlobalVal.Uc.TreeMaterial = this;
                            WMaterial win = new WMaterial();
                            win.Left = GlobalVal.Uc.MainWin.Left + GlobalVal.Uc.MainWin.ActualWidth / 2 - win.Width / 2;
                            win.Top = GlobalVal.Uc.MainWin.Top + 25;
                            win.ShowDialog();
                            return;
                        }

                        if (TypeOfTree == "book")
                        {
                            ////载入节点内容
                            //string tableName = TypeOfTree;
                            //SqliteOperate sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
                            //string sql = string.Format("SELECT NodeContent FROM Tree_{0} where Uid='{1}';", tableName, selectedNode.Uid);
                            //SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
                            //while (reader.Read())
                            //{
                            //    selectedNode.NodeContent = reader["NodeContent"].ToString();
                            //}
                            //reader.Close();

                            foreach (HandyControl.Controls.TabItem item in GlobalVal.Uc.TabControl.Items)
                            {
                                if (item.Uid == selectedNode.Uid)
                                {
                                    item.IsSelected = true;
                                    return;
                                }
                            }

                            UEditor ucEditor = new UEditor
                            {
                                DataContext = selectedNode
                            };
                            ucEditor.LoadChapter(CurBookName, TypeOfTree);

                            HandyControl.Controls.TabItem tabItem = new HandyControl.Controls.TabItem
                            {
                                Uid = selectedNode.Uid,
                                DataContext = selectedNode,
                                IsSelected = true,
                                Content = ucEditor
                            };
                            tabItem.Closing += TabItem_Closing;
                            Binding textBinding = new Binding
                            {
                                Source = selectedNode,
                                Path = new PropertyPath("NodeName"),
                                Mode = BindingMode.TwoWay
                            };
                            tabItem.SetBinding(HeaderedItemsControl.HeaderProperty, textBinding);//对绑定目标的目标属性进行绑定     

                            GlobalVal.Uc.TabControl.Items.Add(tabItem);
                            GlobalVal.Uc.RoleCards.MarkNamesInChapter();
                        }

                    }
                }
            }
        }

        private void TabItem_Closing(object sender, EventArgs e)
        {
            HandyControl.Controls.TabItem tabItem = sender as HandyControl.Controls.TabItem;
            CEditor.TabItemClosing(tabItem, e);
        }
        #endregion

        #region 节点重命名

        private void TbReName_KeyDown(object sender, KeyEventArgs e)
        {
            //TextBox TbReName = sender as TextBox;
            //if (e.Key == Key.F2 ||
            //     e.Key == Key.Enter
            //     )
            //{
            //    TbReName.Visibility = Visibility.Hidden;
            //    e.Handled = true;
            //}
        }

        private void TbReName_LostFocus(object sender, RoutedEventArgs e)
        {
            //TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            //TextBox TbReName = sender as TextBox;
            //Grid grid = GetParentObjectEx<Grid>(TbReName as DependencyObject) as Grid;
            //TextBlock TbkName = FindChild<TextBlock>(grid as DependencyObject, "TbkName");

            //if (selectedNode != null)
            //{
            //    TbReName.Visibility = Visibility.Hidden;
            //    TbkName.Visibility = Visibility.Visible;

            //    string tableName = TypeOfTree;
            //    CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
            //    string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, selectedNode.NodeName.Replace("'", "''"), selectedNode.Uid);
            //    sqlConn.ExecuteNonQuery(sql);

            //}
        }

        private void Tv_KeyDown(object sender, KeyEventArgs e)
        {
            TreeViewNode selectedNode = this.Tv.SelectedItem as TreeViewNode;
            TreeViewItem selectedItem = e.OriginalSource as TreeViewItem;
            if (selectedItem != null && selectedNode.IsButton == false)
            {
                if (e.Key == Key.F2)
                {
                    CheckBox Ck = CTreeView.FindChild<CheckBox>(selectedItem as DependencyObject, "Ck");
                    if (Ck != null && Ck.IsChecked == true)
                    {
                        return;
                    }
                    TbReName = CTreeView.FindChild<TextBox>(selectedItem as DependencyObject, "TbReName");
                    TbReName.Visibility = Visibility.Visible;
                    TbReName.Focus();
                    TbReName.Select(TbReName.Text.Length, 0);
                    TbReName.Tag = false;


                }
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
                {
                    BtnMoveUp_Click(null, null);
                }
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
                {
                    BtnMoveDown_Click(null, null);
                }
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.C)
                {
                    Clipboard.SetText(CurNode.NodeName);
                    HandyControl.Controls.Growl.Success("已复制本文档标题到剪贴板！");
                }
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A)
                {
                    Clipboard.SetText(CurNode.NodeContent);
                    HandyControl.Controls.Growl.Success("已复制本文档内容到剪贴板！");
                }
            }
        }

        private void Tv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastMouseDown = e.GetPosition(this);
            //TreeViewNode selectedNode = this.Tv.SelectedItem as TreeViewNode;
            if (TbReName != null)
            {
                TbReName.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region 节点删除
        /// <summary>
        /// 方法：删除节点按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelNode(TreeViewNode selectedNode)
        {
            if (selectedNode == null || selectedNode.IsButton == true)
            {
                return;
            }
            if (selectedNode.IsDir == true)
            {
                MessageBoxResult dr = MessageBox.Show("真的要进行删除吗？\n将会不经回收站直接删除，请进行确认！\n如非必要，请进行取消！", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                if (dr == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            //在数据库中删除节点记录
            CTreeView.DelNodeBySql(CurBookName, TypeOfTree, selectedNode, TreeViewNodeList);

            //在视图中删除节点，这里注意删除和获取索引号的先后顺序
            TreeViewNode parentNode = selectedNode.ParentNode;
            int n = parentNode.ChildNodes.IndexOf(selectedNode);
            TreeViewNodeList.Remove(selectedNode);
            parentNode.ChildNodes.Remove(selectedNode);
            if (parentNode.ChildNodes.Count > 0)
            {
                if (n == parentNode.ChildNodes.Count)
                {
                    n--;
                }
                TreeViewNode node = parentNode.ChildNodes[n];
                if (node != null)
                {
                    node.IsSelected = true;
                }
            }
        }

        #endregion

        #region 向上/向下调整节点


        /// <summary>
        /// 事件：向上调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            CTreeView.NodeMoveUp(CurBookName, TypeOfTree, (TreeViewNode)this.Tv.SelectedItem, this.TreeViewNodeList);
        }

        /// <summary>
        /// 事件：向下调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            CTreeView.NodeMoveDown(CurBookName, TypeOfTree, (TreeViewNode)this.Tv.SelectedItem, this.TreeViewNodeList);
        }


        #endregion

        #region 拖曳操作相关

        /// <summary>
        /// 方法：获取最近的控件
        /// </summary>
        private TreeViewItem GetNearestContainer(UIElement element)
        {
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }

        TreeViewItem orginItem = new TreeViewItem();
        TreeViewItem oldItem = new TreeViewItem();
        int orginLevel = -1;
        int dropLevel = -1;
        /// <summary>
        /// 事件：进入拖拽状态，改变字体颜色
        /// </summary>
        private void Tv_DragEnter(object sender, DragEventArgs e)
        {
            if ((sender as TreeView).ContextMenu.IsLoaded == false)
            {
                TreeViewItem container = GetNearestContainer(e.OriginalSource as UIElement);
                TreeViewNode dragNode = container.DataContext as TreeViewNode;
                if (orginLevel == -1)
                {
                    orginLevel = CTreeView.GetLevel(dragNode);
                }
                if (orginItem == new TreeViewItem())
                {
                    orginItem = container;
                }
                dropLevel = CTreeView.GetLevel(dragNode);


                if (container != null)
                {
                    oldItem = container;
                    if (dragNode.IsButton == false && orginLevel >= dropLevel)
                    {
                        container.Foreground = new SolidColorBrush(Colors.Orange);
                    }
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// 事件：离开拖曳状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tv_DragLeave(object sender, DragEventArgs e)
        {
            TreeViewItem container = GetNearestContainer(e.OriginalSource as UIElement);
            container.Foreground = orginItem.Foreground;
            if (container != null)
            {
            }
        }

        /// <summary>
        /// 事件：离开拖曳状态（放下）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tv_Drop(object sender, DragEventArgs e)
        {
            if ((sender as TreeView).ContextMenu.IsLoaded == false)
            {
                TreeViewItem container = GetNearestContainer(e.OriginalSource as UIElement);
                TreeViewNode dragNode = (sender as TreeView).SelectedValue as TreeViewNode;
                TreeViewNode dropNode = container.DataContext as TreeViewNode;
                container.Foreground = orginItem.Foreground;

                if (container != null)
                {
                    //oldItem = container;
                    if (dropNode.IsButton == false && dragNode != dropNode)
                    {
                        //放入目标目录（源级别要更深，才能往浅的目标拖动）
                        if (CTreeView.GetLevel(dragNode) > CTreeView.GetLevel(dropNode))
                        {
                            Console.WriteLine("放入目标目录");
                            int m = dropNode.ChildNodes.Count;
                            //原目录
                            if (dropNode.Uid == dragNode.Pid)
                            {
                                m -= 1;
                            }

                            //更换改变pid
                            dragNode.Pid = dropNode.Uid;
                            //更新数据库中临近节点记录集
                            CTreeView.MoveNodeBySql(CurBookName, TypeOfTree, dragNode, TreeViewNodeList);

                            //节点索引交换位置
                            CTreeView.SwapNode(m, dragNode, dropNode, TreeViewNodeList);
                        }
                        //同级调换
                        //if (GetLevel(dropNode) == GetLevel(dragNode))
                        //{
                        //    Console.WriteLine("同级调换");
                        //    int n = dragNode.ParentNode.ChildNodes.IndexOf(dragNode);
                        //    int m = dropNode.ParentNode.ChildNodes.IndexOf(dropNode);
                        //    if (dragNode.Pid != dropNode.Pid)
                        //    {
                        //        m += 1;
                        //    }
                        //    if (dragNode == null || dropNode == null)
                        //    {
                        //        return;
                        //    }

                        //    //数据库中的处理
                        //    SwapNodeBySql(CurBookName, TypeOfTree, dragNode, dropNode);

                        //    //节点索引交换位置
                        //    SwapNode(m, dragNode, dropNode.ParentNode, TreeViewNodeList);
                        //}
                    }
                }
                orginLevel = -1;
                dropLevel = -1;
                orginItem = new TreeViewItem();
                //TreeOperate.SaveBySql(CurBookName, TypeOfTree, TreeViewNodeList, TopNode);
            }
        }

        Point _lastMouseDown;
        /// <summary>
        /// 事件：鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tv_MouseMove(object sender, MouseEventArgs e)
        {
            if ((sender as TreeView).ContextMenu.IsLoaded == true || (TbReName != null && TbReName.Visibility == Visibility.Visible))
            {
                return;
            }

            CTreeView.MouseMoveMethod(Tv, e, _lastMouseDown);
        }

        /// <summary>
        /// 事件：鼠标经过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tv_DragOver(object sender, DragEventArgs e)
        {
            //TreeViewItem dragItem = e.Data.GetData(typeof(TreeViewItem)) as TreeViewItem;
        }

        #endregion


        #region 右键菜单
        private void TreeViewMenu_Opened(object sender, RoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)Tv.SelectedItem;
            if (selectedNode != null)
            {
                ((MenuItem)TreeViewMenu.Items[0]).IsEnabled = true;
                ((MenuItem)TreeViewMenu.Items[2]).IsEnabled = true;
                if (selectedNode.IsDir == true)
                {
                    ((MenuItem)TreeViewMenu.Items[1]).IsEnabled = true;
                    ((MenuItem)TreeViewMenu.Items[3]).IsEnabled = true;
                }
                else
                {
                    ((MenuItem)TreeViewMenu.Items[1]).IsEnabled = false;
                    ((MenuItem)TreeViewMenu.Items[3]).IsEnabled = false;
                }
            }
            else
            {
                ((MenuItem)TreeViewMenu.Items[2]).IsEnabled = false;
                ((MenuItem)TreeViewMenu.Items[0]).IsEnabled = false;
            }
            if (TopNode != null && TopNode.ChildNodes.Count == 0)
            {
                Command_AddBrotherNode_Executed(null, null);
            }
        }

        private void Command_AddBrotherNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            if (selectedNode == null)
            {
                TreeViewNode newNode = CTreeView.AddNewNode(TreeViewNodeList, TopNode, TypeOfTree);
                CTreeView.AddNodeBySql(CurBookName, TypeOfTree, newNode);
                newNode.IsSelected = true;
            }
            else
            {
                TreeViewNode newNode = CTreeView.AddNewNode(TreeViewNodeList, selectedNode.ParentNode, TypeOfTree);
                CTreeView.AddNodeBySql(CurBookName, TypeOfTree, newNode);
                newNode.IsSelected = true;
            }
        }

        private void Command_AddChildNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            if (selectedNode.IsDir == true)
            {
                selectedNode.IsExpanded = true;
                TreeViewNode newNode = CTreeView.AddNewNode(TreeViewNodeList, selectedNode, TypeOfTree);
                CTreeView.AddNodeBySql(CurBookName, TypeOfTree, newNode);
                newNode.IsSelected = true;
            }
            else
            {
                TreeViewNode newNode = CTreeView.AddNewNode(TreeViewNodeList, selectedNode.ParentNode, TypeOfTree);
                CTreeView.AddNodeBySql(CurBookName, TypeOfTree, newNode);
                newNode.IsSelected = true;
            }
        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            DelNode(selectedNode);
        }

        private void Command_Import_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            if (selectedNode.IsDir == true)
            {
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
                string sql = string.Empty;
                string tableName = TypeOfTree;
                CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[CurBookName];
                foreach (string srcFullFileName in files)
                {
                    string title = System.IO.Path.GetFileNameWithoutExtension(srcFullFileName);

                    TreeViewNode newNode = CTreeView.AddNewNode(TreeViewNodeList, selectedNode, TypeOfTree);
                    newNode.NodeName = title;
                    newNode.NodeContent = CFileOperate.ReadFromTxt(srcFullFileName);
                    newNode.WordsCount = CEditor.CountWords(newNode.NodeContent);
                    //合并提交的SQL语句，使用+=来赋值
                    sql += string.Format("INSERT INTO Tree_{0} (Uid, Pid, NodeName, isDir, NodeContent, WordsCount, IsExpanded, IsChecked) VALUES ('{1}', '{2}', '{3}', {4}, '{5}', {6}, {7}, {8});", tableName, newNode.Uid, newNode.Pid, newNode.NodeName, newNode.IsDir, newNode.NodeContent.Replace("'", "''"), newNode.WordsCount, newNode.IsExpanded, newNode.IsChecked);
                }
                sqlConn.ExecuteNonQuery(sql);

            }
        }
        #endregion






        private void Tv_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,

                Source = sender
            };

            this.Tv.RaiseEvent(eventArg);
        }
    }


}
