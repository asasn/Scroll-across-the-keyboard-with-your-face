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
using 脸滚键盘.公共操作类;
using 脸滚键盘.自定义控件;
using static 脸滚键盘.公共操作类.TreeOperate;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// RoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class RoleCard : Window
    {
        TreeView Tv;
        Button CurButton;
        WrapPanel[] wrapPanels;
        string CurBookName;
        string TypeOfTree;
        public struct thisCard
        {
            public static string id;
            public static string weight;
            public static string realAge;
            public static string 相对年龄;
        }

        public RoleCard(string curBookName, string typeOfTree, Button curButton)
        {
            InitializeComponent();

            CurBookName = curBookName;
            TypeOfTree = typeOfTree;

            //添加拖曳面板事件
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };

            //根据外来调用传入的参数填充变量，以备给类成员方法使用
            CurButton = curButton;
            tbName.Text = curButton.Content.ToString();

            WrapPanel[] temp = { wp别称, wp身份, wp外观, wp所属, wp阶级, wp物品, wp能力, wp经历 };
            wrapPanels = temp;

            //填充窗口信息
            GetDataAndFillCard();
        }



        /// <summary>
        /// 显示信息卡的主流程：从数据库中获取信息以填充卡片
        /// </summary>
        void GetDataAndFillCard()
        {
            if (CurButton != null)
            {
                FillBaseInfo();
                CardOperate.FillMainInfo(CurBookName, TypeOfTree, wrapPanels, CurButton.Uid);

            }
        }

        void FillBaseInfo()
        {
            string tableName = TypeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            string sql = string.Format("select * from {0}主表 where {0}id = '{1}';", tableName, CurButton.Uid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
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
            sqlConn.Close();

            int realAge = Gval.CurrentBook.CurrentYear - Gval.CurrentBook.BornYear + int.Parse(tbOffsetAge.Text);
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
            string tableName = TypeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            SQLiteDataReader reader = sqlConn.ExecuteQuery(string.Format("select * from {0}主表 where 名称='{1}'", tableName, tbName.Text));
            while (reader.Read())
            {
                if (CurButton.Uid != reader.GetString(0).ToString())
                {
                    MessageBox.Show("数据库中已经存在同名不同id条目，请修改成为其他名称！");
                    reader.Close();
                    sqlConn.Close();
                    return;
                }
            }
            reader.Close();

            if (false == string.IsNullOrEmpty(CurButton.Content.ToString()))
            {
                if (string.IsNullOrEmpty(thisCard.weight))
                {
                    thisCard.weight = 0.ToString();
                }
                if (string.IsNullOrEmpty(tbOffsetAge.Text))
                {
                    tbOffsetAge.Text = 0.ToString();
                }

                string sql = string.Format("update {0}主表 set 名称='{1}', 备注='{2}', 权重={3}, 相对年龄={4} where {0}id = '{5}';", tableName, tbName.Text, tb备注.Text, thisCard.weight, tbOffsetAge.Text, CurButton.Uid);
                sqlConn.ExecuteNonQuery(sql);

                CurButton.Content = tbName.Text;

                CardOperate.SaveMainInfo(CurBookName, TypeOfTree, wrapPanels, CurButton.Uid);
            }
            sqlConn.Close();


            foreach (HandyControl.Controls.TabItem tabItem in Gval.Uc.TabControl.Items)
            {
                UcEditor ucEditor = tabItem.Content as UcEditor;
                ucEditor.SetRules();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}