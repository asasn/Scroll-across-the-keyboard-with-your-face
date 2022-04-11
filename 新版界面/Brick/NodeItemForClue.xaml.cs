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
    /// NodeItemForDoc.xaml 的交互逻辑
    /// </summary>
    public partial class NodeItemForClue : UserControl
    {
        public NodeItemForClue()
        {
            InitializeComponent();
        }

        private void TbReName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (this.DataContext as Node).ReNameing = false;
                e.Handled = true;
            }
        }

        /// <summary>
        /// 向下递归改变子节点标记
        /// </summary>
        /// <param name="selectedSection"></param>
        private void CheckAllChildNodes(Node thisNode)
        {
            foreach (Node node in thisNode.ChildNodes)
            {
                CheckAllChildNodes(node);
                node.IsChecked = thisNode.IsChecked;
            }
        }

        /// <summary>
        /// 向上改变父节点标记
        /// </summary>
        /// <param name="selectedSection"></param>
        private void CheckParentNodes(Node thisNode)
        {
            if (thisNode.IsChecked == false)
            {
                while (thisNode.ParentNode != null)
                {
                    thisNode = thisNode.ParentNode;
                    thisNode.IsChecked = false;
                }
            }
            else
            {
                bool tag = true;
                //兄弟节点当中有任意一个未选择，则改变标志
                foreach (Node node in thisNode.ParentNode.ChildNodes)
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
                    thisNode.ParentNode.IsChecked = true;
                }
                else
                {
                    thisNode.ParentNode.IsChecked = false;
                    thisNode.ParentNode.IsExpanded = false;
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Node node = this.DataContext as Node;
            if (node != null)
            {
                CheckAllChildNodes(node);
                CheckParentNodes(node);
            }
        }
    }
}
