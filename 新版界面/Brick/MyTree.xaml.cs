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
                node.IsDel = true;
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
    }
}
