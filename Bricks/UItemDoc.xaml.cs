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
using static NSMain.TreeViewPlus.CNodeModule;
using NSMain.TreeViewPlus;

namespace NSMain.Bricks
{
    /// <summary>
    /// UItemForDoc.xaml 的交互逻辑
    /// </summary>
    public partial class UItemDoc : UserControl
    {
        public UItemDoc()
        {
            InitializeComponent();
        }



        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(UItemDoc), new PropertyMetadata(null));




        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(UItemDoc), new PropertyMetadata(null));




        /// <summary>
        /// 点击图标伸展/缩回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewNode selectedNode = (sender as Image).DataContext as TreeViewNode;
            if (selectedNode != null && selectedNode.IsDir == true)
            {
                selectedNode.IsExpanded = !selectedNode.IsExpanded;
            }
        }




        private void TbReName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Parent != null)
            {
                CurBookName = (this.Parent as UserControl).Uid;
                TypeOfTree = (this.Parent as UserControl).Tag.ToString();
            }
            TreeViewNode selectedNode = TbReName.DataContext as TreeViewNode;
            selectedNode.NodeName = TbReName.Text;
            TbReName.Visibility = Visibility.Hidden;
            if (selectedNode != null && (bool)TbReName.Tag == true)
            {
                string tableName = TypeOfTree;
                CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, selectedNode.NodeName.Replace("'", "''"), selectedNode.Uid);
                cSqlite.ExecuteNonQuery(sql);
            }
            TbReName.Tag = false;
        }



        private void TbReName_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox TbReName = sender as TextBox;
            if (e.Key == Key.F2 ||
                 e.Key == Key.Enter
                 )
            {
                TbReName.Visibility = Visibility.Hidden;

                e.Handled = true;
            }
        }

        private void TbReName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TbReName.Tag = true;
        }
    }
}
