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
            (GMian.DataContext as Secen).Node = null;
            string json = JsonHelper.ObjToJson(GMian.DataContext as Secen);
            (GMian.DataContext as Secen).Node = this.DataContext as Node;

            DataOut.UpdateNodeProperty((GMian.DataContext as Secen).Node, nameof(Node.Text), (GMian.DataContext as Secen).Node.Text);
            DataOut.UpdateNodeProperty((GMian.DataContext as Secen).Node, nameof(Node.Summary), json);
            (GMian.DataContext as Secen).CanSave = false;
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
                secen = JsonHelper.JsonToObj<Secen>(secen.Node.Summary);
                secen.Node = this.DataContext as Node;
            }            
            secen.Roles.ChildItems.CollectionChanged += ChildItems_CollectionChanged;
            secen.Origin.ChildItems.CollectionChanged += ChildItems_CollectionChanged;
            secen.Result.ChildItems.CollectionChanged += ChildItems_CollectionChanged;
            GMian.DataContext = secen;
        }

        private void ChildItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            (GMian.DataContext as Secen).CanSave = true;
        }
    }
}
