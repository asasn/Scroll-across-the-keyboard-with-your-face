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

        private void Command_AddBrotherNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Command_AddChildNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Command_Import_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddBrother_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem != null)
            {
                (TreeNodes.SelectedItem as Node).RemoveItSelf();
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem != null)
            {
                (TreeNodes.SelectedItem as Node).ChildNodes.Clear();
            }
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDown_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {

            if (((sender as Button).DataContext as Node).OwnerName != "index" && string.IsNullOrEmpty(Gval.CurrentBook.Uid) == true)
            {
                return;
            }
            Node node = new Node();
            ((sender as Button).DataContext as Node).AddNode(node);
        }

        private void BtnAddDoc_Click(object sender, RoutedEventArgs e)
        {
            if (TreeNodes.SelectedItem != null && (TreeNodes.SelectedItem as Node).RootNode != null)
            {
                Node node = new Node();
                (TreeNodes.SelectedItem as Node).ParentNode.AddNode(node);
            }
        }
    }
}
