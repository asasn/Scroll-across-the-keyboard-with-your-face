using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.公共操作类;
using static 脸滚键盘.公共操作类.TreeOperate;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcTreeBook.xaml 的交互逻辑
    /// </summary>
    public partial class UcTreeMaterial : UserControl
    {
        public UcTreeMaterial()
        {
            InitializeComponent();
        }
        TextBox TbReName;
        readonly string TypeOfTree = "material";
        readonly string CurBookName = "index";

        /// <summary>
        /// 数据源：节点列表
        /// </summary>
        ObservableCollection<TreeViewNode> TreeViewNodeList = new ObservableCollection<TreeViewNode>();

        private void Tv_Loaded(object sender, RoutedEventArgs e)
        {
            //如果处在设计模式中则返回
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) { return; }
            LoadBook();
            Console.WriteLine("载入资料库");
        }

        void LoadBook()
        {
            //数据初始化
            TreeViewNodeList = new ObservableCollection<TreeViewNode>();

            //数据源加载节点列表
            Tv.ItemsSource = TreeViewNodeList;

            //初始化顶层节点数据
            TreeViewNode TopNode = new TreeViewNode
            {
                Uid = "",
                IsDir = true
            };


            Gval.Flag.Loading = true;

            AddButtonNode(TreeViewNodeList, TopNode);

            //从数据库中载入数据
            LoadBySql(CurBookName, TypeOfTree, TreeViewNodeList, TopNode);


            Gval.Flag.Loading = false;
        }





        #region 节点相关操作

        #region 节点展开/缩回
        /// <summary>
        /// 点击图标伸展/缩回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewNode selectedNode = (sender as Image).DataContext as TreeViewNode;
            if (selectedNode != null && selectedNode.IsDir == true && selectedNode.IsButton == false)
            {
                selectedNode.IsExpanded = !selectedNode.IsExpanded;
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
                selectedNode.IconPath = Gval.Path.App + "/Resourse/ic_action_folder_open.png";
                TreeOperate.ExpandedCollapsedBySql(CurBookName, TypeOfTree, selectedNode);
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
                selectedNode.IconPath = Gval.Path.App + "/Resourse/ic_action_folder_closed.png";
                TreeOperate.ExpandedCollapsedBySql(CurBookName, TypeOfTree, selectedNode);
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

            Grid grid = FindChild<Grid>(selectedItem as DependencyObject, "grid");
            TextBlock showNameTextBox = FindChild<TextBlock>(selectedItem as DependencyObject, "TbkName");
            TextBox TbReName = FindChild<TextBox>(selectedItem as DependencyObject, "TbReName");
            Button btnDel = FindChild<Button>(selectedItem as DependencyObject, "btnDel");

            if (TbReName != null)
            {
                TbReName.Visibility = Visibility.Hidden;
            }


        }

        private void Tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewNode selectedNode = this.Tv.SelectedItem as TreeViewNode;
            TreeViewItem selectedItem = TreeOperate.GetParentObjectEx<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;

            if (selectedItem != null)
            {
                if (selectedNode.IsButton == true)
                {
                    TreeViewNode newNode = AddNewNode(TreeViewNodeList, selectedNode.ParentNode, TypeOfTree);
                    TreeOperate.AddNodeBySql(CurBookName, TypeOfTree, newNode);
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
                        //SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
                        //string sql = string.Format("UPDATE Tree_{0} set IsExpanded={1} where Uid = '{2}';", tableName, selectedNode.IsExpanded, selectedNode.Uid);
                        //sqlConn.ExecuteNonQuery(sql);
                        //sqlConn.Close();
                    }
                    else
                    {
                        if (Gval.Uc.MaterialWindow.IsVisible == false)
                        {
                            Gval.Uc.MaterialWindow = new MaterialWindow();
                            Gval.Uc.MaterialWindow.ucEditor.DataContext = selectedNode;
                            Gval.Uc.MaterialWindow.ucEditor.LoadChapter(CurBookName, TypeOfTree);
                            Gval.Uc.MaterialWindow.ShowDialog();
                        }
                    }
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
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
                string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, selectedNode.NodeName, selectedNode.Uid);
                sqlConn.ExecuteNonQuery(sql);
                sqlConn.Close();
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
                    TbReName = FindChild<TextBox>(selectedItem as DependencyObject, "TbReName");
                    TbReName.SelectAll();
                    TbReName.Visibility = Visibility.Visible;
                    TbReName.Focus();
                }
            }
        }


        private void Tv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewNode selectedNode = this.Tv.SelectedItem as TreeViewNode;
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
            TreeOperate.DelNodeBySql(CurBookName, TypeOfTree, selectedNode, TreeViewNodeList);

            //在视图中删除节点，这里注意删除和获取索引号的先后顺序
            TreeViewNode parentNode = selectedNode.ParentNode;
            int n = parentNode.ChildNodes.IndexOf(selectedNode);
            TreeViewNodeList.Remove(selectedNode);
            parentNode.ChildNodes.Remove(selectedNode);
            if (parentNode.ChildNodes.Count >= 2)
            {
                if (n > parentNode.ChildNodes.Count - 2)
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
            //已经采用按钮控件btnUp的可用/禁用来作判断，所以不必再次检查临近节点索引是否合法
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            int n = selectedNode.ParentNode.ChildNodes.IndexOf(selectedNode);
            n--;
            TreeViewNode neighboringNode = selectedNode.ParentNode.ChildNodes[n];
            if (selectedNode == null || neighboringNode == null)
            {
                return;
            }

            //数据库中的处理
            TreeOperate.SwapNodeBySql(CurBookName, TypeOfTree, selectedNode, neighboringNode);

            //节点索引交换位置
            TreeOperate.SwapNode(n, selectedNode, neighboringNode, TreeViewNodeList);
        }

        /// <summary>
        /// 事件：向下调整节点位置按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            int n = selectedNode.ParentNode.ChildNodes.IndexOf(selectedNode);
            n++;
            TreeViewNode neighboringNode = selectedNode.ParentNode.ChildNodes[n];
            if (selectedNode == null || neighboringNode == null)
            {
                return;
            }

            //数据库中的处理
            TreeOperate.SwapNodeBySql(CurBookName, TypeOfTree, selectedNode, neighboringNode);

            //节点索引交换位置
            TreeOperate.SwapNode(n, selectedNode, neighboringNode, TreeViewNodeList);
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
                        if (m > 0)
                        {
                            //原目录
                            if (dropNode.Uid == dragNode.Pid)
                            {
                                m -= 2;
                            }
                            else
                            {
                                m -= 1;
                            }
                        }


                        string tableName = TypeOfTree;
                        //SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
                        //更新数据库中临近节点记录集
                        DelNodeBySql(CurBookName, TypeOfTree, dragNode, TreeViewNodeList);
                        //更换改变pid
                        dragNode.Pid = dropNode.Uid;
                        AddNodeBySql(CurBookName, TypeOfTree, dragNode);

                        //节点索引交换位置
                        TreeOperate.SwapNode(m, dragNode, dropNode, TreeViewNodeList);
                    }
                    //同级调换
                    if (GetLevel(dropNode) == GetLevel(dragNode))
                    {
                        Console.WriteLine("同级调换");
                        int n = dragNode.ParentNode.ChildNodes.IndexOf(dragNode);
                        int m = dropNode.ParentNode.ChildNodes.IndexOf(dropNode);
                        if (dragNode.Pid != dropNode.Pid)
                        {
                            m += 1;
                        }
                        if (dragNode == null || dropNode == null)
                        {
                            return;
                        }

                        //数据库中的处理
                        TreeOperate.SwapNodeBySql(CurBookName, TypeOfTree, dragNode, dropNode);

                        //节点索引交换位置
                        TreeOperate.SwapNode(m, dragNode, dropNode, TreeViewNodeList);
                    }
                }
            }
            orginLevel = -1;
            dropLevel = -1;
            orginItem = new TreeViewItem();
        }


        Point _lastMouseDown;
        /// <summary>
        /// 事件：鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tv_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //获取鼠标移动的距离
                    Point currentPosition = e.GetPosition(Tv);

                    //判断鼠标是否移动
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        //获取鼠标选中的节点数据
                        TreeViewNode draggedNode = (TreeViewNode)Tv.SelectedItem;
                        if (draggedNode != null && draggedNode.IsButton == false)
                        {
                            //启动拖放操作
                            //DragDropEffects finalDropEffect = DragDrop.DoDragDrop(treeView, treeView.SelectedValue,System.Windows.DragDropEffects.Move);
                            DragDrop.DoDragDrop(Tv, Tv.SelectedValue, System.Windows.DragDropEffects.Move);
                            e.Handled = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 事件：鼠标经过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tv_DragOver(object sender, DragEventArgs e)
        {
            TreeViewItem dragItem = e.Data.GetData(typeof(TreeViewItem)) as TreeViewItem;
        }



        #endregion

        #endregion

        #region 右键菜单
        private void TreeViewMenu_Opened(object sender, RoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)Tv.SelectedItem;
            if (selectedNode != null)
            {
                if (selectedNode.IsButton == false)
                {
                    ((MenuItem)TreeViewMenu.Items[0]).IsEnabled = true;
                }
                else
                {
                    ((MenuItem)TreeViewMenu.Items[0]).IsEnabled = false;
                }
            }

        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TreeViewNode selectedNode = (TreeViewNode)this.Tv.SelectedItem;
            DelNode(selectedNode);
        }


        #endregion


    }


}
