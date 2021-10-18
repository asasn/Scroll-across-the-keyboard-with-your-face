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
    public partial class CommonCard : Window
    {
        TreeView Tv;
        SQLiteDataReader reader;
        TreeViewItem thisItem;
        public CommonCard(TreeView tv, TreeViewItem selectedItem)
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
                WrapPanel[] wrapPanels = { w2, };
                CardOperate.FillMainInfo(wrapPanels, "通用", thisItem.Uid);

            }
        }

        void FillBaseInfo()
        {
            string sql = string.Format("select * from 通用 where 通用id = {0};", thisItem.Uid);
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

        //void FillMainInfo(string 通用id, WrapPanel[] wrapPanels)
        //{

        //    foreach (WrapPanel wp in wrapPanels)
        //    {
        //        string sql = string.Format("select * from 通用{0}表 where 通用id = {1};", wp.Uid, 通用id);
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
            reader = SqliteOperate.ExecuteQuery(string.Format("select * from 通用 where 名称='{0}'", tbName.Text));
            if (reader.Read() && tbId.Uid != reader.GetInt32(0).ToString())
            {
                MessageBox.Show("数据库中已经存在同名条目，请修改成为其他名称！");
                return;
            }
            if (tbId != null && false == string.IsNullOrEmpty(tbId.Uid))
            {
                string 通用id = tbId.Uid;
                string 权重 = "0";
                if (string.IsNullOrEmpty(tbQz.Text))
                {
                    权重 = 0.ToString();
                }
                else
                {
                    权重 = tbQz.Text;
                }

                string sql = string.Format("update 通用 set 名称='{0}', 备注='{1}', 权重={3} where 通用id = {2};", tbName.Text, t12.Text, 通用id, 权重);
                SqliteOperate.ExecuteNonQuery(sql);

                thisItem.Header = tbName.Text;

                WrapPanel[] wrapPanels = { w2, };
                CardOperate.SaveMainInfo(wrapPanels, "通用", 通用id);
            }

            TreeOperate.Save.ToSingleXml(Tv, Gval.Current.curBookItem, "common");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string num = b.Name.Substring(1);
            string wpName = "w" + num;
            WrapPanel wp = gCard.FindName(wpName) as WrapPanel;

            TextBox tb = CardOperate.AddTextBox();
            wp.Children.Add(tb);
        }

        void AddNewTable()
        {
            RowDefinition row = new RowDefinition();
            gCard.RowDefinitions.Add(row);

            WrapPanel wp = new WrapPanel();            
            Label lb = new Label();
            lb.Content = "测试测试测试";
            TextBox tb = CardOperate.AddTextBox();
            wp.Children.Add(tb);
            gCard.Children.Add(wp);

            int n = 2;
            foreach (var lineWp in gCard.Children)
            {
                if (lineWp.GetType() == typeof(WrapPanel))
                {

                    Grid.SetRow(lineWp as WrapPanel, n);
                    n++;
                }

            }

        }

        private void 点击事件〇控件〇添加新行(object sender, RoutedEventArgs e)
        {
            //AddNewTable();
        }

        private void 点击事件〇控件〇删除新行(object sender, RoutedEventArgs e)
        {

        }
    }
}
