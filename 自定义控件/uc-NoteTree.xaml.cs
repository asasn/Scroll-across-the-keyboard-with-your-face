using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 脸滚键盘
{
    /// <summary>
    /// uc_NoteTree.xaml 的交互逻辑
    /// </summary>
    public partial class uc_NoteTree : UserControl
    {
        public uc_NoteTree()
        {
            InitializeComponent();
        }



        public string UcTitle
        {
            get { return (string)GetValue(UcTitleProperty); }
            set { SetValue(UcTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTitleProperty =
            DependencyProperty.Register("UcTitle", typeof(string), typeof(uc_NoteTree), new PropertyMetadata(null));

        public string UcTag
        {
            get { return (string)GetValue(UcTagProperty); }
            set { SetValue(UcTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTagProperty =
            DependencyProperty.Register("UcTag", typeof(string), typeof(uc_NoteTree), new PropertyMetadata(null));



        /// <summary>
        /// DataContext绑定了当前指向的curItem，因此将其更改事件作为curItem的更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Gval.Current.curBookItem != null)
            {
                tv.Items.Clear();

                //获取当前notes对应的完整xml文件名
                string fullXmlName_notes = Gval.Base.AppPath + "/books/" + Gval.Current.curBookItem.Header.ToString() + "/" + UcTag + ".xml";
                if (true == FileOperate.IsFileExists(fullXmlName_notes))
                {
                    TreeOperate.Show.FromSingleXml(tv, Gval.Current.curBookItem, UcTag);
                }
                uc.IsEnabled = true;
            }
            else
            {
                tv.Items.Clear();
                uc.IsEnabled = false;
            }
        }


        private void tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnNewFolder_Click(object sender, RoutedEventArgs e)
        {
            string itemTitle = "新根节点";
            if (Gval.Current.curBookItem != null)
            {
                TreeViewItem newItem = TreeOperate.AddItem.RootItem(tv, itemTitle, TreeOperate.ItemType.目录);
                TreeOperate.Save.ToSingleXml(tv, Gval.Current.curBookItem, UcTag);
            }
        }

        private void btnNewDoc_Click(object sender, RoutedEventArgs e)
        {
            string itemTitle = "新子节点";
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null && Gval.Current.curBookItem != null)
            {
                int level = TreeOperate.GetLevel(selectedItem);
                if (level == 2)
                {
                    TreeOperate.AddItem.BrotherItem(selectedItem, itemTitle, TreeOperate.ItemType.文档);
                    TreeOperate.Save.ToSingleXml(tv, Gval.Current.curBookItem, UcTag);

                }
                if (level == 1)
                {
                    TreeOperate.AddItem.ChildItem(selectedItem, itemTitle, TreeOperate.ItemType.文档);
                    TreeOperate.Save.ToSingleXml(tv, Gval.Current.curBookItem, UcTag);
                }
            }
        }

        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null && Gval.Current.curBookItem != null)
            {
                TreeOperate.DelItem.Do(selectedItem);
                TreeOperate.Save.ToSingleXml(tv, Gval.Current.curBookItem, UcTag);
            }
        }

        private void tv_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;

            //实例化菜单
            ContextMenu cm = this.FindResource("tvContextMenu") as ContextMenu;

            //在鼠标所在的位置显现
            cm.Placement = PlacementMode.MousePoint;

            //显示菜单
            cm.IsOpen = true;

            if (selectedItem != null)
            {
                (cm.Items.GetItemAt(2) as MenuItem).IsEnabled = true;
                (cm.Items.GetItemAt(4) as MenuItem).IsEnabled = true;
            }
            else
            {
                (cm.Items.GetItemAt(2) as MenuItem).IsEnabled = false;
                (cm.Items.GetItemAt(4) as MenuItem).IsEnabled = false;
            }
        }

        /// <summary>
        /// TreeView鼠标左键点击事件：点击在TreeView类型的控件tv上，对应Item来说，相当于点击在空白
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                selectedItem.IsSelected = false;
            }
            if (renameBox.Visibility == Visibility.Visible)
            {
                TreeOperate.ReName.Do(tv, curItem, renameBox, UcTag);
                selectedItem.Focus();
            }

        }

        TreeViewItem curItem;
        /// <summary>
        /// TreeView快捷键（包含按F2重命名等）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
                if (selectedItem != null)
                {
                    if (renameBox.Visibility == Visibility.Hidden)
                    {
                        curItem = selectedItem;//记录下当前节点的各种信息
                        TreeOperate.ReName.Ready(tv, selectedItem, renameBox);
                    }
                }
            }
        }

        /// <summary>
        /// 重命名文本框快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameBox_KeyDown(object sender, KeyEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (e.Key == Key.Enter)
            {
                if (renameBox.Visibility == Visibility.Visible)
                {
                    TreeOperate.ReName.Do(tv, curItem, renameBox, UcTag);
                    selectedItem.Focus();
                }
            }
        }
    }
}
