using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Xml;

namespace 脸滚键盘.信息卡模板
{
    /// <summary>
    /// RoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class RoleCard : Window
    {
        TreeView Tv;
        SQLiteDataReader reader;
        TreeViewItem thisItem;
        public RoleCard(TreeView tv, TreeViewItem selectedItem)
        {
            InitializeComponent();

            //根据外来调用传入的参数填充变量，以备给类成员方法使用
            thisItem = selectedItem;
            Tv = tv;

            //填充窗口信息
            GetDataAndFillCard();
        }



        /// <summary>
        /// 显示信息卡的主流程：从数据库中获取信息以填充卡片
        /// </summary>
        void GetDataAndFillCard()
        {
            if (thisItem != null)
            {
                FillBaseInfo();
                WrapPanel[] wrapPanels = { w2, w3, w4, w7, w8, w9, w10, w11 };
                CardOperate.FillMainInfo(wrapPanels, "角色", thisItem.Uid);

                WrapPanel[] wrapPanels2 = { w5, w6 };
                CardOperate.FillOtherInfo(wrapPanels2, "角色", thisItem.Uid);
            }
        }

        void FillBaseInfo()
        {
            string sql = string.Format("select * from 角色 where 角色id = {0};", thisItem.Uid);
            reader = SqliteOperate.ExecuteQuery(sql);

            string 备注 = string.Empty;
            string 权重 = string.Empty;
            string 相对年龄 = string.Empty;
            while (reader.Read())
            {
                if (false == reader.IsDBNull(2))
                {
                    备注 = reader.GetString(2);
                }
                if (false == reader.IsDBNull(3))
                {
                    权重 = reader.GetInt32(3).ToString();
                }
                if (false == reader.IsDBNull(4))
                {
                    相对年龄 = reader.GetString(4).ToString();
                }

            }
            tbId.Text = thisItem.Uid;
            tbId.Uid = thisItem.Uid;
            tbRoleName.Text = thisItem.Header.ToString();
            t12.Text = 备注;
            tbQz.Text = 权重;
            tage.Text = 相对年龄;

        }

        //void FillMainInfo(string 角色id, WrapPanel[] wrapPanels)
        //{

        //    foreach (WrapPanel wp in wrapPanels)
        //    {
        //        string sql = string.Format("select * from 角色{0}表 where 角色id = {1};", wp.Uid, 角色id);
        //        reader = SqliteOperate.ExecuteQuery(sql);
        //        wp.Children.Clear();
        //        while (reader.Read())
        //        {
        //            string t = reader.GetString(1);
        //            int n = reader.GetInt32(2);
        //            TextBox tb = AddTextBox();
        //            tb.Text = t;
        //            tb.Uid = n.ToString();
        //            wp.Children.Add(tb);
        //        }
        //    }
        //}

        //void FillOtherInfo(string 角色id, WrapPanel[] wrapPanels)
        //{
        //    foreach (WrapPanel wp in wrapPanels)
        //    {
        //        string sql = string.Format("select * from 角色{0}表 where 角色id = {1};", wp.Uid, 角色id);
        //        reader = SqliteOperate.ExecuteQuery(sql);
        //        int n = 0;
        //        while (reader.Read())
        //        {
        //            n = reader.GetInt32(1);
        //        }

        //        foreach (RadioButton rb in wp.Children)
        //        {
        //            if (n == wp.Children.IndexOf(rb))
        //            {
        //                rb.IsChecked = true;
        //                break;
        //            }
        //        }
        //    }
        //}


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string num = b.Name.Substring(1);
            string wpName = "w" + num;
            WrapPanel wp = gCard.FindName(wpName) as WrapPanel;
            TextBox tb = CardOperate.AddTextBox();
            wp.Children.Add(tb);

        }




        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            reader = SqliteOperate.ExecuteQuery(string.Format("select * from 角色 where 名称='{0}'", tbRoleName.Text));
            if (reader.Read() && tbId.Uid != reader.GetInt32(0).ToString())
            {
                MessageBox.Show("数据库中已经存在同名不同id条目，请修改成为其他名称！");
                return;
            }
            if (tbId != null && false == string.IsNullOrEmpty(tbId.Uid))
            {
                string 角色id = tbId.Uid;
                string 权重 = "0";
                string 相对年龄 = "0";
                if (string.IsNullOrEmpty(tbQz.Text))
                {
                    权重 = 0.ToString();
                }
                else
                {
                    权重 = tbQz.Text;
                }
                if (string.IsNullOrEmpty(tage.Text))
                {
                    相对年龄 = 0.ToString();
                }
                else
                {
                    相对年龄 = tage.Text;
                }

                string sql = string.Format("update 角色 set 名称='{0}', 备注='{1}', 权重={3}+1, 相对年龄={4} where 角色id = {2};", tbRoleName.Text, t12.Text, 角色id, 权重, 相对年龄);
                SqliteOperate.ExecuteNonQuery(sql);

                thisItem.Header = tbRoleName.Text;

                WrapPanel[] wrapPanels = { w2, w3, w4, w7, w8, w9, w10, w11 };
                CardOperate.SaveMainInfo(wrapPanels, "角色", 角色id);

                WrapPanel[] wrapPanels2 = { w5, w6 };
                CardOperate.SaveOtherInfo(wrapPanels2, "角色", 角色id);
            }

            TreeOperate.Save.ToSingleXml(Tv, Gval.Current.curBookItem, "role");
        }

        ///// <summary>
        ///// 在数据库中添加一个角色行
        ///// </summary>
        //public void AddRole()
        //{
        //    //实际上是以名字为标识符
        //    if (null == tbId.Text && false == string.IsNullOrEmpty(tbRoleName.Text))
        //    {
        //        string sql = string.Format("insert or ignore into 角色 (名称, 备注) values ('{0}', '{1}');", tbRoleName.Text, t12.Text);
        //        SqliteOperate.ExecuteNonQuery(sql);
        //        int lastuid = SqliteOperate.GetLastUid("角色");
        //        if (null == tbId.Text)
        //        {
        //            tbId.Text = lastuid.ToString();
        //        }
        //    }
        //}



    }
}