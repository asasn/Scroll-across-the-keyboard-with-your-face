using NSMain.TreeViewPlus;
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

namespace NSMain.Bricks
{
    /// <summary>
    /// UItemTask.xaml 的交互逻辑
    /// </summary>
    public partial class UItemTask : UserControl
    {
        public UItemTask()
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
            DependencyProperty.Register("CurBookName", typeof(string), typeof(UItemTask), new PropertyMetadata(null));




        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(UItemTask), new PropertyMetadata(null));





        private void Ck_Checked(object sender, RoutedEventArgs e)
        {
            if (this.Parent != null)
            {
                CurBookName = (this.Parent as UserControl).Uid;
                TypeOfTree = (this.Parent as UserControl).Tag.ToString();
            }
            CheckBox Ck = sender as CheckBox;
            //Grid grid = GetParentObjectEx<Grid>(Ck as DependencyObject) as Grid;
            //TextBlock TbkName = FindChild<TextBlock>(grid as DependencyObject, "TbkName");
            TreeViewNode selectedNode = Ck.DataContext as TreeViewNode;
            CTreeView.CheckedBySql(CurBookName, TypeOfTree, selectedNode);
            
            TbkName.Foreground = Brushes.Silver;
        }

        private void Ck_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.Parent != null)
            {
                CurBookName = (this.Parent as UserControl).Uid;
                TypeOfTree = (this.Parent as UserControl).Tag.ToString();
            }
            CheckBox Ck = sender as CheckBox;
            //Grid grid = GetParentObjectEx<Grid>(Ck as DependencyObject) as Grid;
            //TextBlock TbkName = FindChild<TextBlock>(grid as DependencyObject, "TbkName");
            TreeViewNode selectedNode = Ck.DataContext as TreeViewNode;
            CTreeView.CheckedBySql(CurBookName, TypeOfTree, selectedNode);
            
            TbkName.Foreground = Brushes.Black;
        }

        private void Ck_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent != null)
            {
                CurBookName = (this.Parent as UserControl).Uid;
                TypeOfTree = (this.Parent as UserControl).Tag.ToString();
            }
            CheckBox Ck = sender as CheckBox;
            TreeViewItem item = CTreeView.GetParentObjectEx<TreeViewItem>(Ck as DependencyObject) as TreeViewItem;
            //Grid grid = GetParentObjectEx<Grid>(Ck as DependencyObject) as Grid;
            //TextBlock TbkName = FindChild<TextBlock>(grid as DependencyObject, "TbkName");
            TreeViewNode selectedNode = Ck.DataContext as TreeViewNode;
            selectedNode.TheItem = item;
            if (Ck.IsChecked == true)
            {
                //当自身选中时，子节点全部进行相应的改变
                foreach (TreeViewNode node in selectedNode.ChildNodes)
                {
                    node.IsChecked = true;
                }

                bool tag = true;
                //兄弟节点当中有任意一个未选择，则改变标志
                foreach (TreeViewNode node in selectedNode.ParentNode.ChildNodes)
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
                    selectedNode.ParentNode.IsChecked = true;
                }
                else
                {
                    selectedNode.ParentNode.IsChecked = false;
                }
            }
            else
            {
                //当自身取消选中时，父节点也取消选中
                selectedNode.ParentNode.IsChecked = false;

                //当自身选中时，子节点全部进行相应的改变
                foreach (TreeViewNode node in selectedNode.ChildNodes)
                {
                    node.IsChecked = false;
                }
            }
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
            if (selectedNode != null && (bool)TbReName.Tag  == true)
            {
                string tableName = TypeOfTree;
                CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, selectedNode.NodeName.Replace("'", "''"), selectedNode.Uid);
                cSqlite.ExecuteNonQuery(sql);
            }
            TbReName.Tag = false;
        }

        private void TbReName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TbReName.Tag = true;
        }
    }
}
