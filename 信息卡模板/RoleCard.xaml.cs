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

        TreeViewItem thisItem;
        SaveCardDelegate saveCard;
        public RoleCard(TreeViewItem selectedItem, MouseButtonEventArgs e, SaveCardDelegate funSave)
        {
            InitializeComponent();

            thisItem = selectedItem;
            saveCard = funSave;
            Point p = Mouse.GetPosition(e.Source as FrameworkElement);
            Point pointToWindow = (e.Source as FrameworkElement).PointToScreen(p);//转化为屏幕中的坐标
            this.Left = pointToWindow.X - this.Width / 2;
            this.Top = pointToWindow.Y - this.Height / 2;
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            FillInNewCard();
        }

        void FillInNewCard()
        {
            if (thisItem != null)
            {
                string sql = string.Format("select * from 角色 where 角色id = {0};", thisItem.Uid);
                SQLiteDataReader reader = SqliteOperate.ExecuteQuery(sql);
                int 角色id = 0;
                string 备注 = string.Empty;
                string 权重 = string.Empty;
                string 相对年龄 = string.Empty;
                while (reader.Read())
                {
                    角色id = reader.GetInt32(0);
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
                        相对年龄 = reader.GetInt32(4).ToString();
                    }

                }
                if (string.IsNullOrEmpty(tbId.Text))
                {
                    tbId.Text = 角色id.ToString();
                    tbId.Uid = 角色id.ToString();
                    tbRoleName.Text = thisItem.Header.ToString();
                    t12.Text = 备注;
                    tbQz.Text = 权重;
                    tage.Text = 相对年龄;
                }

                WrapPanel[] wrapPanels = { w2, w3, w4, w7, w8, w9, w10, w11 };

                foreach (WrapPanel wp in wrapPanels)
                {
                    sql = string.Format("select * from {0} where 角色id = {1};", wp.Uid, 角色id);
                    reader = SqliteOperate.ExecuteQuery(sql);
                    wp.Children.Clear();
                    while (reader.Read())
                    {
                        string t = reader.GetString(1);
                        int n = reader.GetInt32(2);
                        TextBox tb = AddTextBox();
                        tb.Text = t;
                        tb.Uid = n.ToString();
                        wp.Children.Add(tb);
                    }
                }

                WrapPanel[] wrapPanels2 = { w5, w6 };
                foreach (WrapPanel wp in wrapPanels2)
                {
                    sql = string.Format("select * from {0} where 角色id = {1};", wp.Uid, 角色id);
                    reader = SqliteOperate.ExecuteQuery(sql);
                    int n = 0;
                    while (reader.Read())
                    {
                        n = reader.GetInt32(1);
                    }

                    foreach (RadioButton rb in wp.Children)
                    {
                        if (n == wp.Children.IndexOf(rb))
                        {
                            rb.IsChecked = true;
                            break;
                        }
                    }
                }
            }
        }

        TextBox AddTextBox()
        {
            TextBox tb = new TextBox();
            tb.MinWidth = 30;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Text = "";
            tb.BorderThickness = new Thickness(0, 0, 0, 1);
            tb.Margin = new Thickness(5, 2, 0, 0);
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            return tb;
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


        int GetLastUid()
        {
            string sql = "select last_insert_rowid() from role;";
            SQLiteDataReader reader = SqliteOperate.ExecuteQuery(sql);
            int lastuid = -1;
            while (reader.Read())
            {
                lastuid = reader.GetInt32(0);
            }
            return lastuid;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

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

                string sql = string.Format("update 角色 set 名称='{0}', 备注='{1}', 权重={3}+1, 相对年龄={4} where 角色id = {2};", tbRoleName.Text, t12.Text, 角色id, 权重 , 相对年龄);
                SqliteOperate.ExecuteNonQuery(sql);

                thisItem.Header = tbRoleName.Text;

                WrapPanel[] wrapPanels = { w2, w3, w4, w7, w8, w9, w10, w11 };

                foreach (WrapPanel wp in wrapPanels)
                {
                    sql = string.Empty;
                    foreach (TextBox tb in wp.Children)
                    {
                        if (string.IsNullOrEmpty(tb.Uid))
                        {
                            //不存在记录
                            if (false == string.IsNullOrEmpty(tb.Text))
                            {
                                //编辑框不为空，插入
                                sql += string.Format("insert or ignore into {0} (角色id, {0}) values ({1}, '{2}');", wp.Uid, 角色id, tb.Text);
                            }
                        }
                        else
                        {
                            //存在记录，为空时删除，不为空时更新
                            if (string.IsNullOrEmpty(tb.Text))
                            {
                                sql += string.Format("delete from {0} where {0}id = {1};", wp.Uid, tb.Uid);
                                
                            }
                            else
                            {
                                sql += string.Format("update {0} set {0}='{1}' where {0}id = {2};", wp.Uid, tb.Text, tb.Uid);
                            }
                            
                            
                        }
                    }
                    SqliteOperate.ExecuteNonQuery(sql);
                }

                WrapPanel[] wrapPanels2 = { w5, w6 };
                foreach (WrapPanel wp in wrapPanels2)
                {
                    sql = string.Empty;
                    int n = 0;
                    foreach (RadioButton rb in wp.Children)
                    {
                        if (rb.IsChecked == true)
                        {
                            break;
                        }
                        n++;
                    }
                    sql += string.Format("replace into {0} (角色id, {0}) VALUES ({1}, '{2}');", wp.Uid, 角色id, n);
                    SqliteOperate.ExecuteNonQuery(sql);
                }



            }

            saveCard();



        }

        /// <summary>
        /// 在数据库中添加一个角色行
        /// </summary>
        public void AddRole()
        {
            //实际上是以名字为标识符
            if (null == tbId.Text && false == string.IsNullOrEmpty(tbRoleName.Text))
            {
                string sql = string.Format("insert or ignore into 角色 (名称, 备注) values ('{0}', '{1}');", tbRoleName.Text, t12.Text);
                SqliteOperate.ExecuteNonQuery(sql);
                sql = "select last_insert_rowid() from 角色;";
                SQLiteDataReader reader = SqliteOperate.ExecuteQuery(sql);
                int lastuid = 0;
                while (reader.Read())
                {
                    lastuid = reader.GetInt32(0);
                }
                if (null == tbId.Text)
                {
                    tbId.Text = lastuid.ToString();
                }
            }
        }



    }
}