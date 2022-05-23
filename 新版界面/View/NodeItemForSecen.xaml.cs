using RootNS.Helper;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// NodeItemForSecens.xaml 的交互逻辑
    /// </summary>
    public partial class NodeItemForSecen : UserControl
    {
        public NodeItemForSecen()
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
            Node node = ((this.DataContext as Node).Extra as Summary).Node;
            if (node != null)
            {
                node.CheckChildNodes();
                node.CheckParentNodes();
            }
        }




        private void TbReName_Loaded(object sender, RoutedEventArgs e)
        {
            ((this.DataContext as Node).Extra as Summary).CanSave = false;
        }


        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            Summary secen = new Summary
            {
                Node = this.DataContext as Node
            };
            if (JsonHelper.JsonToObject<Summary>(secen.Node.Summary) != null)
            {
                secen.Json = JsonHelper.JsonToObject<Summary.JsonData>(secen.Node.Summary);
            }

            secen.Time = secen.Json.Time;
            secen.Place = secen.Json.Place;
            (this.DataContext as Node).Extra = secen;


            foreach (string uid in secen.Json.Roles.ToList())
            {
                foreach (Card card in Gval.CurrentBook.CardRole.ChildNodes)
                {
                    if (uid == card.Uid)
                    {
                        secen.Roles.Add(card);
                    }
                }
            }
            foreach (string uid in secen.Json.Origin.ToList())
            {
                foreach (Node node in Gval.CurrentBook.GetSecenNodes())
                {
                    if (uid == node.Uid)
                    {
                        secen.Origin.Add(node);
                    }
                }
            }
            foreach (string uid in secen.Json.Result.ToList())
            {
                foreach (Node node in Gval.CurrentBook.GetSecenNodes())
                {
                    if (uid == node.Uid)
                    {
                        secen.Result.Add(node);
                    }
                }
            }

        }



        private void ThisControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((this.DataContext as Node).IsChecked == true)
            {
                return;
            }
            SecenWindow secenWindow = new SecenWindow();
            secenWindow.DataContext = this.DataContext as Node;
            secenWindow.GMian.DataContext = (this.DataContext as Node).Extra;
            Workfolw.ViewSet.ForViewPointX(secenWindow, Gval.View.TabNote, -6);
            Workfolw.ViewSet.ForViewPointY(secenWindow, this, 50);
            secenWindow.ShowDialog();
        }
    }
}
