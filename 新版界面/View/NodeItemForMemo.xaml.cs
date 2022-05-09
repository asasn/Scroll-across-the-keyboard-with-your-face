using RootNS.Helper;
using RootNS.Model;
using RootNS.Workfolw;
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
    public partial class NodeItemForMemo : UserControl
    {
        public NodeItemForMemo()
        {
            InitializeComponent();
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

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Node node = this.DataContext as Node;
            if (node != null)
            {
                node.CheckChildNodes();
                node.CheckParentNodes();
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Summary secen = new Summary
            {
                Node = this.DataContext as Node
            };
            if (JsonHelper.JsonToObj<Summary>(secen.Node.Summary) != null)
            {
                secen.Json = JsonHelper.JsonToObj<Summary.JsonData>(secen.Node.Summary);
            }

            secen.Time = secen.Json.Time;
            secen.Place = secen.Json.Place;
            (this.DataContext as Node).Extra = secen;
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((this.DataContext as Node).IsChecked == true)
            {
                return;
            }
            UserControl_Loaded(null, null);
            ClueWindow clueWindow = new ClueWindow();
            clueWindow.DataContext = this.DataContext as Node;
            clueWindow.GMian.DataContext = (this.DataContext as Node).Extra;
            Workfolw.ViewSet.ForViewPointX(clueWindow, Gval.View.TabNote, -6);
            Workfolw.ViewSet.ForViewPointY(clueWindow, this, 50);
            clueWindow.ShowDialog();
        }
    }
}
