using RootNS.Helper;
using RootNS.Model;
using RootNS.Workfolw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RootNS.View
{
    /// <summary>
    /// MyTree.xaml 的交互逻辑
    /// </summary>
    public partial class MyTree : UserControl
    {
        public MyTree()
        {
            InitializeComponent();
        }


        private void TreeViewMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem == null)
            {
                return;
            }
            ((sender as ContextMenu).Items[1] as MenuItem).IsEnabled = true;
            ((sender as ContextMenu).Items[2] as MenuItem).IsEnabled = true;
        }
        #region 命令
        Node _lastReNameNode;
        private void Command_ReName_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Node selectedNode = _lastReNameNode = TreeNodes.SelectedItem as Node;
            //TreeViewItem container = (TreeViewItem)TreeNodes.ItemContainerGenerator.ContainerFromItem(selectedNode);
            if (selectedNode != null)
            {
                selectedNode.ReNameing = !selectedNode.ReNameing;
                TextBox TbReName = ControlHelper.FindChild<TextBox>(selectedItem as DependencyObject, "TbReName");
                TbReName.SelectAll();
                TbReName.Focus();
            }
        }

        private void Command_AddFolder_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if ((TreeNodes.DataContext as Node).OwnerName != "index" &&
                string.IsNullOrEmpty(Gval.CurrentBook.Uid) == true)
            {
                return;
            }
            Node node = new Node();
            node.IsDir = true;
            (TreeNodes.DataContext as Node).AddChildNode(node);
        }

        private void Command_AddDoc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Node selectedNode = TreeNodes.SelectedItem as Node;
            Node rootNode = TreeNodes.DataContext as Node;
            if (selectedNode == null)
            {
                if (rootNode.TabName == Book.ChapterTabName.草稿.ToString() ||
                    rootNode.TabName == Book.ChapterTabName.作品相关.ToString())
                {
                    Node node = new Node();
                    rootNode.AddChildNode(node);
                }
            }
            else
            {
                Node node = new Node();
                if (selectedNode.IsDir == true)
                {
                    selectedNode.AddChildNode(node);
                }
                else
                {
                    (selectedNode.Parent as Node).AddChildNode(node);
                }
            }
        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem == null)
            {
                return;
            }
            Node node = TreeNodes.SelectedItem as Node;
            if (Gval.OpeningDocList.Contains(node))
            {
                FunctionPack.ShowMessageBox("当前文档已打开，请关闭之后再进行操作！");
                return;
            }
            if (node.IsDel == false)
            {
                node.ChangeDelFlag(true);
                node.IsExpanded = false;

            }
            else
            {
                node.RealRemoveItSelfAndAllChildNodes();
            }
        }

        private void Command_UnDel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem == null)
            {
                return;
            }
            (TreeNodes.SelectedItem as Node).ChangeDelFlag(false);
        }

        private void Command_MoveUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem == null)
            {
                return;
            }
            (TreeNodes.SelectedItem as Node).MoveUp();
        }

        private void Command_MoveDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem == null)
            {
                return;
            }
            (TreeNodes.SelectedItem as Node).MoveDown();
        }

        private void Command_Keep_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Node selectedNode = TreeNodes.SelectedItem as Node;
            Node targetRootNode = Gval.CurrentBook.BoxTemp;
            if (selectedNode == null || selectedNode.RootNode == null || targetRootNode == null)
            {
                return;
            }
            Gval.CurrentBook.LoadForAllChapterTabs();
            selectedNode.RealRemoveItSelfAndAllChildNodes();
            selectedNode.AddToTreeEnd(targetRootNode);
        }
        private void Command_Send_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Node selectedNode = TreeNodes.SelectedItem as Node;
            Node targetRootNode = Gval.CurrentBook.BoxPublished;
            if (selectedNode == null || selectedNode.RootNode == null || targetRootNode == null)
            {
                return;
            }
            Gval.CurrentBook.LoadForAllChapterTabs();
            selectedNode.RealRemoveItSelfAndAllChildNodes();
            selectedNode.AddToTreeEnd(targetRootNode);
        }
        private void Command_Import_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Node selectedNode = TreeNodes.SelectedItem as Node;
            if (selectedNode == null)
            {
                selectedNode = (sender as Button).DataContext as Node;
            }
            selectedNode.Import();
        }
        private void Command_Export_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Node selectedNode = TreeNodes.SelectedItem as Node;
            if (selectedNode == null)
            {
                return;
            }
            selectedNode.Export();
        }

        private void Command_CopyTitle_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText((TreeNodes.SelectedItem as Node).Title);
            HandyControl.Controls.Growl.SuccessGlobal("已复制本节点标题到剪贴板！");
        }

        private void Command_CopyText_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText((TreeNodes.SelectedItem as Node).Text);
            HandyControl.Controls.Growl.SuccessGlobal("已复制本节点文本到剪贴板！");
        }
        #endregion

        #region 按钮点击事件

        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            Command_AddFolder_Executed(null, null);
        }

        private void BtnAddDoc_Click(object sender, RoutedEventArgs e)
        {
            Command_AddDoc_Executed(null, null);
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Command_Delete_Executed(null, null);
        }

        private void BtnUnDel_Click(object sender, RoutedEventArgs e)
        {
            Command_UnDel_Executed(null, null);
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            Command_MoveUp_Executed(null, null);
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            Command_MoveDown_Executed(null, null);
        }

        private void BtnKeep_Click(object sender, RoutedEventArgs e)
        {
            Command_Keep_Executed(null, null);
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            Command_Send_Executed(null, null);
        }
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            Command_Import_Executed(sender, null);
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            Command_Export_Executed(null, null);
        }
        #endregion

        #region 拖曳移动节点
        /// <summary>
        /// 方法：drop之后向上获取容器
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

        /// <summary>
        /// 按下：准备拖曳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodes_DragEnter(object sender, DragEventArgs e)
        {
            TreeViewItem container = GetNearestContainer(e.OriginalSource as UIElement);
            Node dragNode = container.DataContext as Node;
            if (container != null)
            {
                container.Foreground = new SolidColorBrush(Colors.Orange);
            }
        }

        private void TreeNodes_DragLeave(object sender, DragEventArgs e)
        {
            TreeViewItem container = GetNearestContainer(e.OriginalSource as UIElement);
            Node dragNode = container.DataContext as Node;
            if (container != null)
            {
                container.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// 松开：完成拖曳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodes_Drop(object sender, DragEventArgs e)
        {
            TreeViewItem container = GetNearestContainer(e.OriginalSource as UIElement);
            Node dragNode = (sender as TreeView).SelectedValue as Node;
            Node dropNode = container.DataContext as Node;

            dragNode.DragDropNode(dropNode);

            _lastMouseLeftDown = new Point();
            TreeNodes_DragLeave(sender, e);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodes_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(TextBox))
            {
                return;
            }
            try
            {
                //获取鼠标选中的节点数据
                Node dragNode = TreeNodes.SelectedItem as Node;
                if (e.LeftButton == MouseButtonState.Pressed && dragNode != null && dragNode.IsDir == false)
                {
                    //获取鼠标移动的距离
                    Point currentPosition = e.GetPosition(TreeNodes);
                    //判断鼠标是否移动
                    if ((Math.Abs(currentPosition.X - _lastMouseLeftDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseLeftDown.Y) > 10.0))
                    {
                        //启动拖放操作
                        DragDropEffects finalDropEffect = DragDrop.DoDragDrop(TreeNodes, TreeNodes.SelectedValue, System.Windows.DragDropEffects.Move);
                        e.Handled = true;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        Point _lastMouseLeftDown;

        /// <summary>
        /// 拖动之前的点位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastMouseLeftDown = e.GetPosition(this);
        }
        private void TreeNodes_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((sender as TreeViewItem) == null && _lastReNameNode != null && _lastReNameNode.ReNameing == true)
            {
                _lastReNameNode.FinishRename();
            }
        }
        #endregion


        TreeViewItem selectedItem;
        private void TreeNodes_Selected(object sender, RoutedEventArgs e)
        {
            selectedItem = e.OriginalSource as TreeViewItem;
            ((this.DataContext as Node).Owner as BookBase).SelectedNode = selectedItem.DataContext as Node;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            //R1.Height = new GridLength(26);
            //TreeNodes.BorderThickness = new Thickness(1);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            //R1.Height = new GridLength(0);
            //TreeNodes.BorderThickness = new Thickness(1,0,1,1);
        }

        private void CommandBinding_CanExecute_AddFolder(object sender, CanExecuteRoutedEventArgs e)
        {
            if (((sender as TreeView).DataContext as Node).TabName == Book.ChapterTabName.草稿.ToString() ||
                ((sender as TreeView).DataContext as Node).TabName == Book.ChapterTabName.作品相关.ToString())
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        private void CommandBinding_CanExecute_ImportExport(object sender, CanExecuteRoutedEventArgs e)
        {
            if (((sender as TreeView).DataContext as Node).TabName == Book.ChapterTabName.草稿.ToString() ||
                ((sender as TreeView).DataContext as Node).TabName == Book.ChapterTabName.作品相关.ToString() ||
                ((sender as TreeView).DataContext as Node).TabName == Book.ChapterTabName.已发布.ToString() ||
                ((sender as TreeView).DataContext as Node).TabName == Material.MaterialTabName.范文.ToString() ||
                ((sender as TreeView).DataContext as Node).TabName == Material.MaterialTabName.资料.ToString())
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void CommandBinding_CanExecute_Keep(object sender, CanExecuteRoutedEventArgs e)
        {
            if (((sender as TreeView).DataContext as Node).TabName == Book.ChapterTabName.草稿.ToString())
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void CommandBinding_CanExecute_Send(object sender, CanExecuteRoutedEventArgs e)
        {
            if (((sender as TreeView).DataContext as Node).TabName == Book.ChapterTabName.草稿.ToString() ||
                ((sender as TreeView).DataContext as Node).TabName == Book.ChapterTabName.作品相关.ToString())
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void TreeNodes_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewAutomationPeer lvap = new TreeViewAutomationPeer(TreeNodes);
            var svap = lvap.GetPattern(PatternInterface.Scroll) as ScrollViewerAutomationPeer;
            if (svap != null)
            {
                var scroll = svap.Owner as ScrollViewer;
                scroll.ScrollToEnd();
            }
        }
    }
}
