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
    public partial class uc_InfoCardTree : UserControl
    {
        public uc_InfoCardTree()
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
            DependencyProperty.Register("UcTitle", typeof(string), typeof(uc_InfoCardTree), new PropertyMetadata(null));



        public string UcTag
        {
            get { return (string)GetValue(UcTagProperty); }
            set { SetValue(UcTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTagProperty =
            DependencyProperty.Register("UcTag", typeof(string), typeof(uc_InfoCardTree), new PropertyMetadata(null));


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
            if (selectedItem != null && UcTag == "role")
            {
                RoleCard roleCard = new RoleCard(tv, selectedItem);
                CardOperate.SetWindowsMiddle(e, roleCard);
                roleCard.Show();
            }
            if (selectedItem != null && UcTag == "faction")
            {
                PlaceAndFactionCard placeAndFactionCard = new PlaceAndFactionCard(tv, selectedItem);
                CardOperate.SetWindowsMiddle(e, placeAndFactionCard);
                placeAndFactionCard.Show();
            }
            if (selectedItem != null && UcTag == "goods")
            {
                GoodsCard goodsCard = new GoodsCard(tv, selectedItem);
                CardOperate.SetWindowsMiddle(e, goodsCard);
                goodsCard.Show();
            }
            if (selectedItem != null && UcTag == "common")
            {
                CommonCard commonCard = new CommonCard(tv, selectedItem);
                CardOperate.SetWindowsMiddle(e, commonCard);
                commonCard.Show();
            }
        }


        private void uc_Loaded(object sender, RoutedEventArgs e)
        {
            //赋值给不同的公共变量以便调用
            if (UcTag == "role")
            {
                Gval.InfoCard.RoleTv = tv;
            }
            if (UcTag == "goods")
            {
                Gval.InfoCard.GoodsTv = tv;
            }
            if (UcTag == "faction")
            {
                Gval.InfoCard.FactionTv = tv;
            }
            if (UcTag == "common")
            {
                Gval.InfoCard.CommonTv = tv;
            }
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

            if (Gval.Current.curBookItem != null)
            {
                string tagName;
                if (UcTag == "role")
                {
                    tagName = "角色";
                    string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}({0}id INTEGER PRIMARY KEY AUTOINCREMENT, 名称 CHARUNIQUE,备注 TEXT,权重 INTEGER,相对年龄 CHAR);", tagName);
                    SqliteOperate.ExecuteNonQuery(sql);

                    id = CardOperate.AddCard(tagName, itemTitle);
                }
                if (UcTag == "faction")
                {
                    tagName = "势力";
                    CardOperate.TryToBuildBaseTable(tagName);

                    id = CardOperate.AddCard(tagName, itemTitle);
                }
                if (UcTag == "goods")
                {
                    tagName = "物品";
                    CardOperate.TryToBuildBaseTable(tagName);

                    id = CardOperate.AddCard(tagName, itemTitle);
                }
                if (UcTag == "common")
                {
                    tagName = "通用";
                    CardOperate.TryToBuildBaseTable(tagName);

                    id = CardOperate.AddCard(tagName, itemTitle);
                }
                //string timestr = DateTime.Now.ToString();
                //string uid = getMd5(timestr);
                TreeViewItem newItem = TreeOperate.AddItem.RootItem(tv, itemTitle, TreeOperate.ItemType.目录);
                newItem.Uid = id.ToString();
                TreeOperate.Save.ToSingleXml(tv, Gval.Current.curBookItem, UcTag);
            }

        }

        private void btnNewDoc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelItem_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null && Gval.Current.curBookItem != null)
            {
                TreeOperate.DelItem.Do(selectedItem);
                TreeOperate.Save.ToSingleXml(tv, Gval.Current.curBookItem, UcTag);
                if (UcTag == "role")
                {
                    string sql = string.Format("DELETE FROM 角色 where 角色id = {0};", selectedItem.Uid);
                    SqliteOperate.ExecuteNonQuery(sql);
                }
                if (UcTag == "faction")
                {
                    string sql = string.Format("DELETE FROM 势力 where 势力id = {0};", selectedItem.Uid);
                    SqliteOperate.ExecuteNonQuery(sql);
                }
                if (UcTag == "goods")
                {
                    string sql = string.Format("DELETE FROM 物品 where 物品id = {0};", selectedItem.Uid);
                    SqliteOperate.ExecuteNonQuery(sql);
                }
                if (UcTag == "common")
                {
                    string sql = string.Format("DELETE FROM 通用 where 通用id = {0};", selectedItem.Uid);
                    SqliteOperate.ExecuteNonQuery(sql);
                }
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
