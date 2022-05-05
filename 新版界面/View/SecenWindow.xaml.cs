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
using System.Windows.Shapes;

namespace RootNS.View
{
    /// <summary>
    /// SecenWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SecenWindow : Window
    {
        public SecenWindow()
        {
            InitializeComponent();
        }
        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //添加拖曳面板事件
            DragMove();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (GMian.DataContext as Secen).CanSave = false;
        }
        private void BoxContent_Loaded(object sender, RoutedEventArgs e)
        {
            (GMian.DataContext as Secen).Node.IsExpanded = !string.IsNullOrWhiteSpace(TbContent.Text);
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //Node里面存在着循环引用，所以不能直接转换，需要先设置为空，然后再重新赋值回去。
            SaveSecen(GMian.DataContext as Secen, this.DataContext as Node);

            Node thisNode = this.DataContext as Node;

            //注意理顺这里的逻辑
            foreach (Node node in Gval.CurrentBook.GetSecenNodes())
            {
                foreach (Node sNode in (node.Extra as Secen).Origin.ToList())
                {
                    if ((GMian.DataContext as Secen).Result.Contains(node) == false)
                    {
                        (node.Extra as Secen).Origin.Remove(thisNode);
                        SaveSecen(node.Extra as Secen, (node.Extra as Secen).Node);
                        (node.Extra as Secen).Node.IsExpanded = true;
                    }
                }
            }
            foreach (Node node in Gval.CurrentBook.GetSecenNodes())
            {
                foreach (Node sNode in (node.Extra as Secen).Result.ToList())
                {
                    if ((GMian.DataContext as Secen).Origin.Contains(node) == false)
                    {
                        (node.Extra as Secen).Result.Remove(thisNode);
                        SaveSecen(node.Extra as Secen, (node.Extra as Secen).Node);
                        (node.Extra as Secen).Node.IsExpanded = true;
                    }
                }
            }

            foreach (Node sNode in (GMian.DataContext as Secen).Origin)
            {
                if ((sNode.Extra as Secen).Result.Contains(thisNode) == false)
                {
                    (sNode.Extra as Secen).Result.Add(thisNode);
                    SaveSecen(sNode.Extra as Secen, (sNode.Extra as Secen).Node);
                    (sNode.Extra as Secen).Node.IsExpanded = true;
                }
            }
            foreach (Node sNode in (GMian.DataContext as Secen).Result)
            {
                if ((sNode.Extra as Secen).Origin.Contains(thisNode) == false)
                {
                    (sNode.Extra as Secen).Origin.Add(thisNode);
                    SaveSecen(sNode.Extra as Secen, (sNode.Extra as Secen).Node);
                    (sNode.Extra as Secen).Node.IsExpanded = true;
                }
            }

        }

        private void SaveSecen(Secen secen, Node node)
        {
            secen.Node = null;
            string json = JsonHelper.ObjToJson(secen.Json);
            secen.Node = node;

            DataOut.UpdateNodeProperty(secen.Node, nameof(Node.Text), secen.Node.Text);
            DataOut.UpdateNodeProperty(secen.Node, nameof(Node.Summary), json);
            secen.CanSave = false;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (GMian.DataContext as Secen).CanSave = true;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
