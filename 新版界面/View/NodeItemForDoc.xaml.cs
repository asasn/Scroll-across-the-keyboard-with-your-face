using RootNS.Helper;
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

namespace RootNS.View
{
    /// <summary>
    /// NodeItemForDoc.xaml 的交互逻辑
    /// </summary>
    public partial class NodeItemForDoc : UserControl
    {
        public NodeItemForDoc()
        {
            InitializeComponent();
        }

        private void ThisControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Node node = this.DataContext as Node;
            if (node.IsDir == false && node.IsDel == false)
            {
                if (Gval.OpeningDocList.Contains(node) == false)
                {
                    Gval.OpeningDocList.Add(node);
                }
                EditorHelper.SelectItem(Gval.EditorTabControl, node);
            }
        }

        private void ThisControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as Node).IsSelected = true;
        }

        private void TbReName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (this.DataContext as Node).FinishRename();
                e.Handled = true;//防止触发对应的快捷键
            }
        }

        private void TbReName_LostFocus(object sender, RoutedEventArgs e)
        {
            (this.DataContext as Node).FinishRename();
        }
    }
}
