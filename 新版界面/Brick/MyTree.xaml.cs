using RootNS.Behavior;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace RootNS.Brick
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

        }

        private void Command_AddFolder_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnFolder_Click(null, null);
        }

        private void Command_AddDoc_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnAddDoc_Click(null, null);
        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnDel_Click(null, null);
        }

        private void Command_Import_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }


        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem != null)
            {
                Node node = TreeNodes.SelectedItem as Node;
                int i = 0;
                //if (node.ParentNode.ChildNodes.Count > 0)
                //{
                //    if (node.Index >= 0)
                //    {
                //        i = node.Index;
                //    }
                //    if (node.Index == node.ParentNode.ChildNodes.Count)
                //    {
                //        i = node.Index - 1;
                //    }
                //    node.ParentNode.ChildNodes[i].IsSelected = true;
                //}
            }
        }

        private void BtnRecycle_Click(object sender, RoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem != null)
            {
                (TreeNodes.SelectedItem as Node).ChildNodes.Clear();
            }
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem != null)
            {
                Node node = (TreeNodes.SelectedItem as Node);
                if (node.Index > 0)
                {
                    int i = node.Index;
                    (TreeNodes.SelectedItem as Node).ParentNode.ChildNodes.Move(i, i - 1);
                    (TreeNodes.SelectedItem as Node).ParentNode.ChildNodes[i].Index = i;
                    (TreeNodes.SelectedItem as Node).ParentNode.ChildNodes[i - 1].Index = i - 1;
                }
            }
        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem != null)
            {
                Node node = (TreeNodes.SelectedItem as Node);
                if (node.Index < node.ParentNode.ChildNodes.Count - 1)
                {
                    int i = node.Index;
                    (TreeNodes.SelectedItem as Node).ParentNode.ChildNodes.Move(i, i + 1);
                    (TreeNodes.SelectedItem as Node).ParentNode.ChildNodes[i].Index = i;
                    (TreeNodes.SelectedItem as Node).ParentNode.ChildNodes[i + 1].Index = i + 1;
                }
            }
        }

        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {

            if ((TreeNodes.DataContext as Node).OwnerName != "index" && string.IsNullOrEmpty(Gval.CurrentBook.Uid) == true)
            {
                return;
            }
            Node node = new Node();
            node.IsDir = true;
            (TreeNodes.DataContext as Node).AddNode(node);
        }

        private void BtnAddDoc_Click(object sender, RoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem != null && (TreeNodes.SelectedItem as Node).RootNode != null)
            {
                Node node = new Node();
                if ((TreeNodes.SelectedItem as Node).IsDir == true)
                {
                    (TreeNodes.SelectedItem as Node).AddNode(node);
                }
                else
                {
                    (TreeNodes.SelectedItem as Node).ParentNode.AddNode(node);
                }
            }
        }

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
            Console.WriteLine(dragNode.Pid);
            dragNode.ParentNode.ChildNodes.Remove(dragNode);
            dragNode.IsDel = false;
            dropNode.ChildNodes.Add(dragNode);
            Console.WriteLine(dragNode.Pid);

            _lastMouseDown = new Point();
            TreeNodes_DragLeave(sender, e);
        }


        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodes_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                //获取鼠标选中的节点数据
                Node dragNode = TreeNodes.SelectedItem as Node;
                if (e.LeftButton == MouseButtonState.Pressed && dragNode != null && dragNode.IsDir == false)
                {
                    //获取鼠标移动的距离
                    Point currentPosition = e.GetPosition(TreeNodes);
                    //判断鼠标是否移动
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
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

        Point _lastMouseDown;

        private void TreeNodes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastMouseDown = e.GetPosition(this);
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            Node selectedNode = TreeNodes.SelectedItem as Node;
            if (selectedNode != null && selectedNode.RootNode != null)
            {
                selectedNode.RealRemoveItSelf();
                DataOut.MoveNodeToOtherTable(selectedNode, selectedNode.TabName, Gval.CurrentBook.BoxPublished.TabName);
                Gval.CurrentBook.BoxPublished.ChildNodes.Add(selectedNode);
            }
        }
    }
}
