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
            string sql = string.Format("select * from 角色 where 角色id = '{0}';", thisCard.id);
            SQLiteDataReader reader = SqliteOperate.ExecuteQuery(sql);

            while (reader.Read())
            {
                if (reader["备注"].ToString() != "")
                {
                    tb备注.Text = reader["备注"].ToString();
                }
                if (reader["权重"].ToString() != "")
                {
                    thisCard.weight = reader["权重"].ToString();
                }
                if (reader["相对年龄"].ToString() == "")
                {
                    tbOffsetAge.Text = "0";
                }
                else
                {
                    tbOffsetAge.Text = reader["相对年龄"].ToString();
                    
                }

            }
            reader.Close();

            tbName.Text = thisItem.Header.ToString();
            int realAge = int.Parse(Gval.Current.tbCurYear.Text) - int.Parse(Gval.Current.tbBornYear.Text) + int.Parse(tbOffsetAge.Text);
            card.Header = string.Format("　　权重：{0}　　年龄：{1}", thisCard.weight, realAge.ToString());
        }


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
            SQLiteDataReader reader = SqliteOperate.ExecuteQuery(string.Format("select * from 角色 where 名称='{0}'", tbName.Text));
            if (reader.Read() && thisCard.id != reader.GetString(0).ToString())
            {
                MessageBox.Show("数据库中已经存在同名不同id条目，请修改成为其他名称！");
                reader.Close();
                return;
            }
            reader.Close();
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

                string sql = string.Format("update 角色 set 名称='{0}', 备注='{1}', 权重={3}, 相对年龄={4} where 角色id = '{2}';", tbName.Text, tb备注.Text, thisCard.id, thisCard.weight, tbOffsetAge.Text);
                SqliteOperate.ExecuteNonQuery(sql);

                thisItem.Header = tbName.Text;

                CardOperate.SaveMainInfo(wrapPanels, "角色", thisCard.id);

                CardOperate.SaveOtherInfo(wrapPanels2, "角色", thisCard.id);
            }

            TreeOperate.Save.ToSingleXml(Tv, "角色");
            //TreeOperate.Save.BySql(Tv, "角色");
            TreeOperate.ReName.toTable(thisItem, tbName.Text, "角色");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}