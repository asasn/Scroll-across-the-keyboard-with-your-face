using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 脸滚键盘.公共操作类;
using static 脸滚键盘.公共操作类.TreeOperate;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcTreeBook.xaml 的交互逻辑
    /// </summary>
    public partial class UcTreeBook : UserControl
    {
        public UcTreeBook()
        {
            InitializeComponent();
        }

        readonly string TypeOfTree = "book";
        /// <summary>
        /// 数据源：节点列表
        /// </summary>
        ObservableCollection<TreeViewNode> TreeViewNodeList = new ObservableCollection<TreeViewNode>();

        private void Tv_Loaded(object sender, RoutedEventArgs e)
        {

        }

        void LoadBook()
        {
            if (Gval.CurrentBook.Name == null)
            {
                return;
            }

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
            LoadBySql(Gval.CurrentBook.Name, "book", TreeViewNodeList, TopNode);

            
            Gval.Flag.Loading = false;
        }





        #region 节点相关操作

        #region 节点展开/缩回
        /// <summary>
        /// 节点展开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tv_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewNode selectedNode = (e.OriginalSource as TreeViewItem).DataContext as TreeViewNode;
            if (selectedNode != null)
            {
                TreeOperate.ExpandedCollapsedBySql(Gval.CurrentBook.Name, TypeOfTree, selectedNode);
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
            if (selectedNode != null)
            {
                TreeOperate.ExpandedCollapsedBySql(Gval.CurrentBook.Name, TypeOfTree, selectedNode);
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


            //上下按钮可用/禁用
            if (selectedNode.IsButton == true)
            {
                BtnMoveUp.IsEnabled = false;
                BtnMoveDown.IsEnabled = false;
            }
            else
            {
                BtnMoveUp.Visibility = Visibility.Visible;
                BtnMoveDown.Visibility = Visibility.Visible;
                if (selectedNode.ParentNode.ChildNodes.IndexOf(selectedNode) == 0)
                {
                    BtnMoveUp.IsEnabled = false;
                }
                else
                {
                    BtnMoveUp.IsEnabled = true;
                }
                if (selectedNode.ParentNode.ChildNodes.IndexOf(selectedNode) == selectedNode.ParentNode.ChildNodes.Count - 2)
                {
                    BtnMoveDown.IsEnabled = false;
                }
                else
                {
                    BtnMoveDown.IsEnabled = true;
                }
            }
        }

        private void Tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewNode selectedNode = this.Tv.SelectedItem as TreeViewNode;
            TreeViewItem selectedItem = TreeOperate.GetParentObjectEx<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;

            if (selectedNode != null)
            {
                if (selectedNode.IsButton == true)
                {
                    TreeViewNode newNode = AddNewNode(TreeViewNodeList, selectedNode.ParentNode);
                    TreeOperate.AddNodeBySql(Gval.CurrentBook.Name, TypeOfTree, newNode);
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
                        //string tableName = Gval.CurrentBook.Name + "_" + TypeOfTree;
                        //SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, Gval.CurrentBook.Name + ".db");
                        //string sql = string.Format("UPDATE Tree_{0} set IsExpanded={1} where Uid = '{2}';", tableName, selectedNode.IsExpanded, selectedNode.Uid);
                        //sqlConn.ExecuteNonQuery(sql);
                    }
                    else
                    {
                        //双击选中节点，尝试进入编辑状态
                        //CurNode = selectedNode;

                        //Gval.ucEditor.CurNodeName = selectedItem.Name;

                        //获取节点对应的路径
                        string nodePath = GetPath(selectedNode);
                        Console.WriteLine(nodePath);
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

            TbkName.Visibility = Visibility.Visible;

            Console.WriteLine(selectedNode.NodeName);

            if (selectedNode != null)
            {
                string tableName = Gval.CurrentBook.Name + "_" + TypeOfTree;
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, Gval.CurrentBook.Name + ".db");
                string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, selectedNode.NodeName, selectedNode.Uid);
                sqlConn.ExecuteNonQuery(sql);
            }
        }

        private void Tv_KeyDown(object sender, KeyEventArgs e)
        {
            TreeViewNode selectedNode = this.Tv.SelectedItem as TreeViewNode;
            TreeViewItem selectedItem = e.OriginalSource as TreeViewItem;

            if (selectedItem != null)
            {
                if (e.Key == Key.F2)
                {
                    TextBox TbReName = FindChild<TextBox>(selectedItem as DependencyObject, "TbReName");
                    TbReName.SelectAll();
                    TbReName.Visibility = Visibility.Visible;
                    TbReName.Focus();
                }
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
            TreeOperate.DelNodeBySql(Gval.CurrentBook.Name, TypeOfTree, selectedNode, TreeViewNodeList);

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
            TreeOperate.SwapNodeBySql(Gval.CurrentBook.Name, TypeOfTree, selectedNode, neighboringNode);

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
            TreeOperate.SwapNodeBySql(Gval.CurrentBook.Name, TypeOfTree, selectedNode, neighboringNode);

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


                        string tableName = Gval.CurrentBook.Name + "_" + TypeOfTree;
                        SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, Gval.CurrentBook.Name + ".db");
                        //更新数据库中临近节点记录集
                        DelNodeBySql(Gval.CurrentBook.Name, TypeOfTree, dragNode, TreeViewNodeList);
                        //更换改变pid
                        dragNode.Pid = dropNode.Uid;
                        AddNodeBySql(Gval.CurrentBook.Name, TypeOfTree, dragNode);

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
                        TreeOperate.SwapNodeBySql(Gval.CurrentBook.Name, TypeOfTree, dragNode, dropNode);

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


        #region 书籍目录抽屉

        /// <summary>
        /// 展示书籍目录抽屉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowBooks_Click(object sender, RoutedEventArgs e)
        {
            DrawerLeftInContainer.IsOpen = !DrawerLeftInContainer.IsOpen;
        }


        private void DrawerLeftInContainer_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.CurrentBook.Uid = SettingsOperate.Get("curBookUid");
            string tableName = "books";
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
            string sql = string.Format("CREATE TABLE IF NOT EXISTS Tree_{0} (Uid CHAR PRIMARY KEY, Name CHAR, Price DOUBLE, BornYear INTEGER, CurrentYear INTEGER);", tableName);
            sqlConn.ExecuteNonQuery(sql);
            sql = string.Format("SELECT * FROM Tree_{0};", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                string BookUid = reader["Uid"].ToString();
                string BookName = reader["Name"].ToString();
                HandyControl.Controls.Card bookCard = new HandyControl.Controls.Card();
                WpBooks.Children.Add(bookCard);
                //bookCard.Effect = 
                bookCard.Margin = new Thickness(10, 10, 0, 0);
                bookCard.Width = 180;
                bookCard.Height = 240;
                bookCard.VerticalAlignment = VerticalAlignment.Stretch;
                bookCard.HorizontalAlignment = HorizontalAlignment.Stretch;
                bookCard.Uid = BookUid;
                bookCard.Header = BookName;

                string imgPath = Gval.Path.Books + "/" + BookName + ".jpg";

                bookCard.Content = FileOperate.GetImgObject(imgPath);

                bookCard.MouseLeftButtonDown += CardSelected;

                if (BookUid == Gval.CurrentBook.Uid)
                {
                    ChoseBookChange(bookCard);
                }
            }
            reader.Close();
        }

        /// <summary>
        /// 根据Uid获取当前书籍信息，并填入Gval公共类当中
        /// </summary>
        /// <param name="uid"></param>
        void GetBookInfoForGval(string uid)
        {
            Gval.CurrentBook.Uid = uid;
            string tableName = "books";
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
            string sql = string.Format("SELECT * FROM Tree_{0} where Uid='{1}';", tableName, Gval.CurrentBook.Uid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                TbCurBookName.Text = reader["Name"].ToString();
                TbCurBookPrice.Text = reader["Price"].ToString();
                TbCurBookBornYear.Text = reader["BornYear"].ToString();
                TbCurBookCurrentYear.Text = reader["CurrentYear"].ToString();

                Gval.CurrentBook.Name = reader["Name"].ToString();
                Gval.CurrentBook.Price = Convert.ToDouble(reader["Price"]);
                Gval.CurrentBook.BornYear = Convert.ToInt32(reader["BornYear"]);
                Gval.CurrentBook.CurrentYear = Convert.ToInt32(reader["CurrentYear"]);
            }
            reader.Close();
        }

        /// <summary>
        /// 选择当前书籍卡片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardSelected(object sender, MouseButtonEventArgs e)
        {
            foreach (HandyControl.Controls.Card item in WpBooks.Children)
            {
                item.BorderBrush = null;
                item.BorderThickness = new Thickness(0, 0, 0, 0);
            }
            HandyControl.Controls.Card bookCard = sender as HandyControl.Controls.Card;
            ChoseBookChange(bookCard);
        }

        /// <summary>
        /// 根据选择的书籍卡片执行一些切换书籍的操作
        /// </summary>
        /// <param name="bookCard"></param>
        void ChoseBookChange(HandyControl.Controls.Card bookCard)
        {
            WpBooks.Tag = bookCard;

            bookCard.BorderBrush = Brushes.DodgerBlue;
            bookCard.BorderThickness = new Thickness(0, 5, 0, 5);

            DrawerTbk.Text = "当前书籍：" + bookCard.Header.ToString();
            SettingsOperate.Set("curBookUid", bookCard.Uid);
            SettingsOperate.Set("curBookName", bookCard.Header.ToString());
            GetBookInfoForGval(bookCard.Uid);
            LoadBook();

            //Gval.ucNote.TvLoad();
            //Gval.ucTask.TvLoad();
            //Gval.ucRoleCard.TvLoad();
            //Gval.ucFactionCard.TvLoad();
            //Gval.ucGoodsCard.TvLoad();
            //Gval.ucCommonCard.TvLoad();
            //Gval.ucEditor.IsEnabled = false;
        }

        private void BtnBuild_Click(object sender, RoutedEventArgs e)
        {
            if (TbNewBookName.IsEnabled == false)
            {
                TbNewBookName.IsEnabled = true;
                TbNewBookName.Text = "新书籍";
                TbNewBookName.SelectAll();
                BtnBuild.Content = "取消创建";
            }
            else
            {
                BtnBuild.Content = "创建新书籍";
                TbNewBookName.Clear();
                TbNewBookName.IsEnabled = false;
            }

        }
        private void TbNewBookName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string tableName = "books";
                string guid = Guid.NewGuid().ToString();
                SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
                string sql = string.Format("INSERT INTO Tree_{0} (Uid, Name, Price, BornYear, CurrentYear) VALUES ('{1}', '{2}', {3}, {4}, {5});", tableName, guid, TbNewBookName.Text, 0, 2000, 2021);
                sqlConn.ExecuteNonQuery(sql);

                TbNewBookName.Clear();
                TbNewBookName.IsEnabled = false;
                BtnBuild.Content = "创建新书籍";
                WpBooks.Children.Clear();
                DrawerLeftInContainer_Loaded(null, null);
            }
        }


        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string tableName = "books";
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
            string sql = string.Format("UPDATE Tree_{0} set Name='{1}', Price={2}, BornYear={3}, CurrentYear={4} where Uid = '{5}';", tableName, TbCurBookName.Text, Convert.ToDouble(TbCurBookPrice.Text), Convert.ToInt32(TbCurBookBornYear.Text), Convert.ToInt32(TbCurBookCurrentYear.Text), Gval.CurrentBook.Uid);
            sqlConn.ExecuteNonQuery(sql);

            GetBookInfoForGval(Gval.CurrentBook.Uid);
            DrawerTbk.Text = "当前书籍：" + TbCurBookName.Text;
            (WpBooks.Tag as HandyControl.Controls.Card).Header = TbCurBookName.Text;
        }

        private void BtnDelBook_Click(object sender, RoutedEventArgs e)
        {
            string tableName = "books";
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, "index.db");
            string sql = string.Format("DELETE from Tree_{0} where Uid='{1}';", tableName, Gval.CurrentBook.Uid);
            sqlConn.ExecuteNonQuery(sql);

            WpBooks.Children.Clear();
            DrawerLeftInContainer_Loaded(null, null);
        }












        #endregion


    }


}
