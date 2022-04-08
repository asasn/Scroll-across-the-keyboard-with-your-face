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
    public partial class NodeItemForDoc : UserControl
    {
        public NodeItemForDoc()
        {
            InitializeComponent();
        }

        private void ThisControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {            
            if ((this.DataContext as Node).IsDir != true)
            {
                if (Gval.OpenedDocList.Contains((this.DataContext as Node)) != true)
                {
                    Gval.OpenedDocList.Add(this.DataContext as Node);
                }
                Gval.EditorTabControl.SelectedItem = this.DataContext as Node;
            }
            (this.DataContext as Node).Title = "测试";
        }

        private void ThisControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as Node).IsSelected = true;
        }


    }
}
