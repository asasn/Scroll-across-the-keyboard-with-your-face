﻿using System;
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
        WrapPanel[] wrapPanels;
        WrapPanel[] wrapPanels2;
        public struct thisCard
        {
            public static string id;
            public static string weight;
            public static string realAge;
            public static string 相对年龄;
        }

        public RoleCard(TreeView tv, TreeViewItem selectedItem)
        {
            InitializeComponent();

            //添加拖曳面板事件
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };

            //根据外来调用传入的参数填充变量，以备给类成员方法使用
            thisItem = selectedItem;
            Tv = tv;
            thisCard.id = selectedItem.Uid;
            WrapPanel[] temp = { wp别称, wp身份, wp外观, wp所属, wp阶级, wp物品, wp能力, wp经历 };
            wrapPanels = temp;
            WrapPanel[] temp2 = { wp状态, wp性别 };
            wrapPanels2 = temp2;

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
                CardOperate.FillMainInfo(wrapPanels, "角色", thisCard.id);

                CardOperate.FillOtherInfo(wrapPanels2, "角色", thisCard.id);
            }
        }

        void FillBaseInfo()
        {
            string sql = string.Format("select * from 角色 where 角色id = {0};", thisCard.id);
            reader = SqliteOperate.ExecuteQuery(sql);

            while (reader.Read())
            {
                if (false == reader.IsDBNull(2))
                {
                    tb备注.Text = reader.GetString(2);
                }
                if (false == reader.IsDBNull(3))
                {
                    thisCard.weight = reader.GetInt32(3).ToString();
                }
                if (false == reader.IsDBNull(4))
                {
                    tbOffsetAge.Text = reader.GetString(4).ToString();
                }

            }            

            tbName.Text = thisItem.Header.ToString();
            card.Header = string.Format("　　id：{0}　　权重：{1}　　年龄：{2}", thisCard.id, thisCard.weight, tbOffsetAge.Text);
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
            string num = b.Name.Substring(3);
            string wpName = "wp" + num;
            WrapPanel wp = gCard.FindName(wpName) as WrapPanel;
            TextBox tb = CardOperate.AddTextBox();
            wp.Children.Add(tb);

        }




        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            reader = SqliteOperate.ExecuteQuery(string.Format("select * from 角色 where 名称='{0}'", tbName.Text));
            if (reader.Read() && thisCard.id != reader.GetInt32(0).ToString())
            {
                MessageBox.Show("数据库中已经存在同名不同id条目，请修改成为其他名称！");
                return;
            }
            if (thisCard.id != null && false == string.IsNullOrEmpty(thisCard.id))
            {
                if (string.IsNullOrEmpty(thisCard.weight))
                {
                    thisCard.weight = 0.ToString();
                }
                if (string.IsNullOrEmpty(tbOffsetAge.Text))
                {
                    tbOffsetAge.Text = 0.ToString();
                }

                string sql = string.Format("update 角色 set 名称='{0}', 备注='{1}', 权重={3}, 相对年龄={4} where 角色id = {2};", tbName.Text, tb备注.Text, thisCard.id, thisCard.weight, tbOffsetAge.Text);
                SqliteOperate.ExecuteNonQuery(sql);

                thisItem.Header = tbName.Text;

                CardOperate.SaveMainInfo(wrapPanels, "角色", thisCard.id);

                CardOperate.SaveOtherInfo(wrapPanels2, "角色", thisCard.id);
            }

            TreeOperate.Save.ToSingleXml(Tv, Gval.Current.curBookItem, "role");
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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