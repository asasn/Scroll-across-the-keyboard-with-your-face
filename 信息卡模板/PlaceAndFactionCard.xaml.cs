﻿using System;
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

namespace 脸滚键盘.信息卡模板
{
    /// <summary>
    /// PlaceCard.xaml 的交互逻辑
    /// </summary>
    public partial class PlaceAndFactionCard : Window
    {
        TreeView Tv;
        SQLiteDataReader reader;
        TreeViewItem thisItem;
        public PlaceAndFactionCard(TreeView tv, TreeViewItem selectedItem)
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
                WrapPanel[] wrapPanels = { w2, w3, w4, w5, w6, w7, w8, w9, w10 };
                CardOperate.FillMainInfo(wrapPanels, "势力", thisItem.Uid);

            }
        }

        void FillBaseInfo()
        {
            string sql = string.Format("select * from 势力 where 势力id = {0};", thisItem.Uid);
            reader = SqliteOperate.ExecuteQuery(sql);

            string 备注 = string.Empty;
            string 权重 = string.Empty;
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

            }
            tbId.Text = thisItem.Uid;
            tbId.Uid = thisItem.Uid;
            tbName.Text = thisItem.Header.ToString();
            t12.Text = 备注;
            tbQz.Text = 权重;

        }

        //void FillMainInfo(string 势力id, WrapPanel[] wrapPanels)
        //{

        //    foreach (WrapPanel wp in wrapPanels)
        //    {
        //        string sql = string.Format("select * from 势力{0}表 where 势力id = {1};", wp.Uid, 势力id);
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



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            reader = SqliteOperate.ExecuteQuery(string.Format("select * from 势力 where 名称='{0}'", tbName.Text));
            if (reader.Read() && tbId.Uid != reader.GetInt32(0).ToString())
            {
                MessageBox.Show("数据库中已经存在同名条目，请修改成为其他名称！");
                return;
            }
            if (tbId != null && false == string.IsNullOrEmpty(tbId.Uid))
            {
                string 势力id = tbId.Uid;
                string 权重 = "0";
                if (string.IsNullOrEmpty(tbQz.Text))
                {
                    权重 = 0.ToString();
                }
                else
                {
                    权重 = tbQz.Text;
                }

                string sql = string.Format("update 势力 set 名称='{0}', 备注='{1}', 权重={3}+1 where 势力id = {2};", tbName.Text, t12.Text, 势力id, 权重);
                SqliteOperate.ExecuteNonQuery(sql);

                thisItem.Header = tbName.Text;

                WrapPanel[] wrapPanels = { w2, w3, w4, w5, w6, w7, w8, w9, w10 };
                CardOperate.SaveMainInfo(wrapPanels, "势力", 势力id);
            }

            TreeOperate.Save.ToSingleXml(Tv, Gval.Current.curBookItem, "faction");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string num = b.Name.Substring(1);
            string wpName = "w" + num;
            WrapPanel wp = gCard.FindName(wpName) as WrapPanel;

            TextBox tb = AddTextBox();
            wp.Children.Add(tb);
        }

        TextBox AddTextBox()
        {
            TextBox tb = new TextBox();
            tb.MinWidth = 30;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Text = "";
            tb.BorderThickness = new Thickness(0, 0, 0, 1);
            tb.Margin = new Thickness(10, 2, 0, 0);
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            return tb;
        }
    }
}
