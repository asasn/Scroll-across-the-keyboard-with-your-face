using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using 脸滚键盘.公共操作类;
using 脸滚键盘.控件方法类;
using static 脸滚键盘.控件方法类.UTreeView;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcTreeBook.xaml 的交互逻辑
    /// </summary>
    public partial class UcontrolTreeTask : UserControl
    {
        public UcontrolTreeTask()
        {
            InitializeComponent();
        }
        TextBox TbReName;
        string TypeOfTree;
        string CurBookName;

        public TreeViewNode TopNode
        {
            get { return (TreeViewNode)GetValue(TopNodeProperty); }
            set { SetValue(TopNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopNodeProperty =
            DependencyProperty.Register("TopNode", typeof(TreeViewNode), typeof(UcontrolTreeTask), new PropertyMetadata(null));

        /// <summary>
        /// 数据源：节点列表
        /// </summary>
        ObservableCollection<TreeViewNode> TreeViewNodeList = new ObservableCollection<TreeViewNode>();

        private void Tv_Loaded(object sender, RoutedEventArgs e)
        {

        }

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

            //数据源加载节点列表
            Tv.ItemsSource = TreeViewNodeList;

            //初始化顶层节点数据
            TopNode = new TreeViewNode
            {
                Uid = "",
                IsDir = true
            };


            Gval.Flag.Loading = true;

            //AddButtonNode(TreeViewNodeList, TopNode);

            //从数据库中载入数据
            LoadBySql(CurBookName, TypeOfTree, TreeViewNodeList, TopNode);


            Gval.Flag.Loading = false;

            //滚动至末尾
            //Sv.ScrollToEnd();
        }





        #region 节点相关操作

        #region 节点展开/缩回
        /// <summary>
        /// 点击图标伸展/缩回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewNode selectedNode = (sender as Image).DataContext as TreeViewNode;
            if (selectedNode != null)
            {

            }
        }

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
                selectedNode.IconPath = Gval.Path.Resourses + "/图标/目录树/ic_action_attachment_2.png";
                ExpandedCollapsedBySql(CurBookName, TypeOfTree, selectedNode);
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
                selectedNode.IconPath = Gval.Path.Resourses + "/图标/目录树/ic_action_attachment.png";
                ExpandedCollapsedBySql(CurBookName, TypeOfTree, selectedNode);
            }
        }
        #endregion

        #region 节点选择/双击

        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = e.OriginalSource as TreeViewItem;
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;

            //载入节点的数据上下文以便调用
            selectedNode.TheItem = selectedItem;

            //Grid grid = FindChild<Grid>(selectedItem as DependencyObject, "GridLine");
            //TextBlock showNameTextBox = FindChild<TextBlock>(selectedItem as DependencyObject, "TbkName");
            //TextBox TbReName = FindChild<TextBox>(selectedItem as DependencyObject, "TbReName");


        }

        private void Tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewNode selectedNode = this.Tv.SelectedItem as TreeViewNode;
            TreeViewItem selectedItem = GetParentObjectEx<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;

            if (selectedNode != null)
            {
                //对抗双击引起的双击/展开
                selectedNode.IsExpanded = !selectedNode.IsExpanded;

                //尝试进入编辑状态
                CheckBox Ck = FindChild<CheckBox>(selectedItem as DependencyObject, "Ck");
                if (Ck.IsChecked == false)
                {
                    TbReName = FindChild<TextBox>(selectedItem as DependencyObject, "TbReName");
                    TbReName.SelectAll();
                    TbReName.Visibility = Visibility.Visible;
                    TbReName.Focus();
                }
            }
        }
        #endregion

        #region 节点重命名

        private void TbReName_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox TbReName = sender as TextBox;
            if (e.Key == Key.F2 ||
                 e.Key == Key.Enter
                 )
            {
                TbReName.Visibility = Visibility.Hidden;
                e.Handled = true;
            }
        }

        private void TbReName_LostFocus(object sender, RoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            TextBox TbReName = sender as TextBox;
            Grid grid = GetParentObjectEx<Grid>(TbReName as DependencyObject) as Grid;
            TextBlock TbkName = FindChild<TextBlock>(grid as DependencyObject, "TbkName");

            if (selectedNode != null)
            {
                TbReName.Visibility = Visibility.Hidden;
                TbkName.Visibility = Visibility.Visible;

                string tableName = TypeOfTree;
                SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
                string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, selectedNode.NodeName.Replace("'", "''"), selectedNode.Uid);
                sqlConn.ExecuteNonQuery(sql);
                
            }
        }

        private void Tv_KeyDown(object sender, KeyEventArgs e)
        {
            TreeViewNode selectedNode = this.Tv.SelectedItem as TreeViewNode;
            TreeViewItem selectedItem = e.OriginalSource as TreeViewItem;
            if (selectedItem != null && selectedNode.IsButton == false)
            {
                if (e.Key == Key.F2)
                {
                    CheckBox Ck = FindChild<CheckBox>(selectedItem as DependencyObject, "Ck");
                    if (Ck.IsChecked == false)
                    {
                        TbReName = FindChild<TextBox>(selectedItem as DependencyObject, "TbReName");
                        TbReName.SelectAll();
                        TbReName.Visibility = Visibility.Visible;
                        TbReName.Focus();
                    }
                }
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
                {
                    BtnMoveUp_Click(null, null);
                }
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
                {
                    BtnMoveDown_Click(null, null);
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
            DelNodeBySql(CurBookName, TypeOfTree, selectedNode, TreeViewNodeList);

            //在视图中删除节点，这里注意删除和获取索引号的先后顺序
            TreeViewNode parentNode = selectedNode.ParentNode;
            int n = parentNode.ChildNodes.IndexOf(selectedNode);
            TreeViewNodeList.Remove(selectedNode);
            parentNode.ChildNodes.Remove(selectedNode);
            if (parentNode.ChildNodes.Count >= 2)
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
        private void BtnMoveUp_Click(object sender, RoutedEventArgs e)
        {
            UTreeView.NodeMoveUp(CurBookName, TypeOfTree, (TreeViewNode)this.Tv.SelectedItem, this.TreeViewNodeList);
        }

        /// <summary>
        /// 事件：向下调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            UTreeView.NodeMoveDown(CurBookName, TypeOfTree, (TreeViewNode)this.Tv.SelectedItem, this.TreeViewNodeList);
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
                    orginLevel = GetLevel(dragNode);
                }
                if (orginItem == new TreeViewItem())
                {
                    orginItem = container;
                }
                dropLevel = GetLevel(dragNode);


                if (container != null)
                {
                    oldItem = container;
                    if (dragNode.IsButton == false && orginLevel >= dropLevel)
                    {
                        container.Foreground = new SolidColorBrush(Colors.Orange);
                    }
                }
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
                        if (GetLevel(dragNode) > GetLevel(dropNode))
                        {
                            Console.WriteLine("放入目标目录");
                            int m = dropNode.ChildNodes.Count;
                            //原目录
                            if (dropNode.Uid == dragNode.Pid)
                            {
                                m -= 1;
                            }

                            //SqliteOperate sqlConn = Gval.SQLClass.Pools[curBookName];
                            //更新数据库中临近节点记录集
                            DelNodeBySql(CurBookName, TypeOfTree, dragNode, TreeViewNodeList);
                            //更换改变pid
                            dragNode.Pid = dropNode.Uid;
                            AddNodeBySql(CurBookName, TypeOfTree, dragNode);

                            //节点索引交换位置
                            SwapNode(m, dragNode, dropNode, TreeViewNodeList);
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

            MouseMoveMethod(Tv, e, _lastMouseDown);
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
                    ((MenuItem)TreeViewMenu.Items[3]).IsEnabled = false;
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
                TreeViewNode newNode = AddNewNode(TreeViewNodeList, TopNode, TypeOfTree);
                AddNodeBySql(CurBookName, TypeOfTree, newNode);
                newNode.IsSelected = true;
            }
            else
            {
                TreeViewNode newNode = AddNewNode(TreeViewNodeList, selectedNode.ParentNode, TypeOfTree);
                AddNodeBySql(CurBookName, TypeOfTree, newNode);
                newNode.IsSelected = true;
            }
        }

        private void Command_AddChildNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            if (selectedNode.IsDir == true)
            {
                selectedNode.IsExpanded = true;
                TreeViewNode newNode = AddNewNode(TreeViewNodeList, selectedNode, TypeOfTree);
                AddNodeBySql(CurBookName, TypeOfTree, newNode);
                newNode.IsSelected = true;
            }
            else
            {
                TreeViewNode newNode = AddNewNode(TreeViewNodeList, selectedNode.ParentNode, TypeOfTree);
                AddNodeBySql(CurBookName, TypeOfTree, newNode);
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

        }


        #endregion

        private void Ck_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox Ck = sender as CheckBox;
            //Grid grid = GetParentObjectEx<Grid>(Ck as DependencyObject) as Grid;
            //TextBlock TbkName = FindChild<TextBlock>(grid as DependencyObject, "TbkName");
            TreeViewNode selectedNode = Ck.DataContext as TreeViewNode;
            CheckedBySql(CurBookName, TypeOfTree, selectedNode);
        }

        private void Ck_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox Ck = sender as CheckBox;
            //Grid grid = GetParentObjectEx<Grid>(Ck as DependencyObject) as Grid;
            //TextBlock TbkName = FindChild<TextBlock>(grid as DependencyObject, "TbkName");
            TreeViewNode selectedNode = Ck.DataContext as TreeViewNode;
            CheckedBySql(CurBookName, TypeOfTree, selectedNode);
        }

        private void Ck_Click(object sender, RoutedEventArgs e)
        {
            CheckBox Ck = sender as CheckBox;
            TreeViewItem item = GetParentObjectEx<TreeViewItem>(Ck as DependencyObject) as TreeViewItem;
            //Grid grid = GetParentObjectEx<Grid>(Ck as DependencyObject) as Grid;
            //TextBlock TbkName = FindChild<TextBlock>(grid as DependencyObject, "TbkName");
            TreeViewNode selectedNode = Ck.DataContext as TreeViewNode;
            selectedNode.TheItem = item;
            if (Ck.IsChecked == true)
            {
                //当自身选中时，子节点全部进行相应的改变
                foreach (TreeViewNode node in selectedNode.ChildNodes)
                {
                    node.IsChecked = true;
                }

                bool tag = true;
                //兄弟节点当中有任意一个未选择，则改变标志
                foreach (TreeViewNode node in selectedNode.ParentNode.ChildNodes)
                {
                    if (node.IsChecked == false)
                    {
                        tag = false;
                        break;
                    }
                }

                //根据标志改变父节点选中状态
                if (tag == true)
                {
                    selectedNode.ParentNode.IsChecked = true;
                }
                else
                {
                    selectedNode.ParentNode.IsChecked = false;
                }
            }
            else
            {
                //当自身取消选中时，父节点也取消选中
                selectedNode.ParentNode.IsChecked = false;

                //当自身选中时，子节点全部进行相应的改变
                foreach (TreeViewNode node in selectedNode.ChildNodes)
                {
                    node.IsChecked = false;
                }
            }

        }

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
