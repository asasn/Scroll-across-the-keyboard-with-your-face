using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using 脸滚键盘.信息卡模板;

namespace 脸滚键盘
{
    public delegate void SaveCardDelegate();

    /// <summary>
    /// uc_NoteTree.xaml 的交互逻辑
    /// </summary>
    public partial class uc_InfoCard : UserControl
    {
        public uc_InfoCard()
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
            DependencyProperty.Register("UcTitle", typeof(string), typeof(uc_InfoCard), new PropertyMetadata(null));



        public string UcTag
        {
            get { return (string)GetValue(UcTagProperty); }
            set { SetValue(UcTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTagProperty =
            DependencyProperty.Register("UcTag", typeof(string), typeof(uc_InfoCard), new PropertyMetadata(null));


        private string getMd5(string password)
        {
            byte[] pasArray = System.Text.Encoding.Default.GetBytes(password);
            pasArray = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(pasArray);
            string rMd5Str = "";
            foreach (byte ibyte in pasArray)
                rMd5Str += ibyte.ToString("x").PadLeft(2, '0');
            return rMd5Str;
        }

        /// <summary>
        /// DataContext绑定了当前指向的curItem，因此将其更改事件作为curItem的更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (true == FileOperate.IsFolderExists(Gval.Current.curBookPath))
            {
                tv.Items.Clear();

                //获取当前notes对应的完整xml文件名
                string fullXmlName_notes = Gval.Current.curBookPath + "/" + UcTag + ".xml";
                if (true == FileOperate.IsFileExists(fullXmlName_notes))
                {
                    TreeOperate.Show.FromSingleXml(tv, UcTag);
                }
                uc.IsEnabled = true;
                Gval.ucEditor.SetRules();
            }
            else
            {
                tv.Items.Clear();
                uc.IsEnabled = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 双击事件：根据不同的标志打开不同的信息卡窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null && UcTag == "角色")
            {
                RoleCard roleCard = new RoleCard(tv, selectedItem);
                roleCard.Show();
                CardOperate.SetWindowsMiddle(e, roleCard);
            }
            if (selectedItem != null && UcTag == "势力")
            {
                PlaceAndFactionCard placeAndFactionCard = new PlaceAndFactionCard(tv, selectedItem);
                placeAndFactionCard.Show();
                CardOperate.SetWindowsMiddle(e, placeAndFactionCard);
            }
            if (selectedItem != null && UcTag == "物品")
            {
                GoodsCard goodsCard = new GoodsCard(tv, selectedItem);
                goodsCard.Show();
                CardOperate.SetWindowsMiddle(e, goodsCard);
            }
            if (selectedItem != null && UcTag == "通用")
            {
                CommonCard commonCard = new CommonCard(tv, selectedItem);
                commonCard.Show();
                CardOperate.SetWindowsMiddle(e, commonCard);
            }
        }


        private void uc_Loaded(object sender, RoutedEventArgs e)
        {
            //赋值给编辑器着色功能使用
            tv.Tag = UcTag;
        }

        /// <summary>
        /// 右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;

            //实例化菜单
            ContextMenu cm = this.FindResource("tvContextMenu") as ContextMenu;

            //在鼠标所在的位置显现
            cm.Placement = PlacementMode.MousePoint;

            //显示菜单
            cm.IsOpen = true;
            (cm.Items.GetItemAt(2) as MenuItem).IsEnabled = false;
            if (selectedItem != null)
            {
                (cm.Items.GetItemAt(4) as MenuItem).IsEnabled = true;
            }
            else
            {
                (cm.Items.GetItemAt(4) as MenuItem).IsEnabled = false;
            }
        }



        private void btnNewFolder_Click(object sender, RoutedEventArgs e)
        {
            string itemTitle = "新信息卡";
            int id = -1;
            foreach (TreeViewItem item in tv.Items)
            {
                if (itemTitle == item.Header.ToString())
                {
                    return;
                }
            }

            if (true == FileOperate.IsFolderExists(Gval.Current.curBookPath))
            {
                CardOperate.TryToBuildBaseTable(UcTag);
                id = CardOperate.AddCard(UcTag, itemTitle);

                //string timestr = DateTime.Now.ToString();
                //string uid = getMd5(timestr);
                TreeViewItem newItem = TreeOperate.AddItem.RootItem(tv, itemTitle, TreeOperate.ItemType.目录);
                newItem.Uid = id.ToString();
                TreeOperate.Save.ToSingleXml(tv, UcTag);
            }

        }

        private void btnNewDoc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null && true == FileOperate.IsFolderExists(Gval.Current.curBookPath))
            {
                TreeOperate.DelItem.Do(selectedItem);
                TreeOperate.Save.ToSingleXml(tv, UcTag);

                string sql = string.Format("DELETE FROM {0} where {0}id = {1};", UcTag, selectedItem.Uid);
                SqliteOperate.ExecuteNonQuery(sql);
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
