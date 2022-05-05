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
            GMian.DataContext = new Secen();
        }


        private void TbReName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (GMian.DataContext as Secen).Node.ReNameing = false;
                e.Handled = true;
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
            Node node = (GMian.DataContext as Secen).Node;
            if (node != null)
            {
                node.CheckChildNodes();
                node.CheckParentNodes();
            }
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

        private void TbReName_Loaded(object sender, RoutedEventArgs e)
        {
            (GMian.DataContext as Secen).CanSave = false;
        }

        private void BoxContent_Loaded(object sender, RoutedEventArgs e)
        {
            (GMian.DataContext as Secen).Node.IsExpanded = !string.IsNullOrWhiteSpace(TbContent.Text);
        }

        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            Secen secen = new Secen
            {
                Node = this.DataContext as Node
            };
            if (JsonHelper.JsonToObj<Secen>(secen.Node.Summary) != null)
            {
                secen.Json = JsonHelper.JsonToObj<Secen.JsonData>(secen.Node.Summary);
                secen.Node = this.DataContext as Node;
            }
            secen.Roles.CollectionChanged += Roles_CollectionChanged;
            secen.Origin.CollectionChanged += Origin_CollectionChanged;
            secen.Result.CollectionChanged += Result_CollectionChanged;

            foreach (string uid in secen.Json.Roles)
            {
                foreach (Card card in Gval.CurrentBook.CardRole.ChildNodes)
                {
                    if (uid == card.Uid)
                    {
                        secen.Roles.Add(card);
                    }
                }
            }
            foreach (string uid in secen.Json.Origin)
            {
                foreach (Node node in Gval.CurrentBook.GetSecenNodes())
                {
                    if (uid == node.Uid)
                    {
                        secen.Origin.Add(node);
                    }
                }
            }
            foreach (string uid in secen.Json.Result)
            {
                foreach (Node node in Gval.CurrentBook.GetSecenNodes())
                {
                    if (uid == node.Uid)
                    {
                        secen.Result.Add(node);
                    }
                }
            }

            GMian.DataContext = secen;
            (this.DataContext as Node).Extra = secen;
        }

        private void Result_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            (GMian.DataContext as Secen).Json.Result.Clear();
            foreach (Node item in (sender as ObservableCollection<object>))
            {
                (GMian.DataContext as Secen).Json.Result.Add(item.Uid);
            }
            (GMian.DataContext as Secen).CanSave = true;
        }

        private void Origin_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            (GMian.DataContext as Secen).Json.Origin.Clear();
            foreach (Node item in (sender as ObservableCollection<object>))
            {
                (GMian.DataContext as Secen).Json.Origin.Add(item.Uid);
            }
            (GMian.DataContext as Secen).CanSave = true;
        }

        private void Roles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            (GMian.DataContext as Secen).Json.Roles.Clear();
            foreach (Card item in (sender as ObservableCollection<object>))
            {
                (GMian.DataContext as Secen).Json.Roles.Add(item.Uid);
            }
            (GMian.DataContext as Secen).CanSave = true;
        }

    }
}
