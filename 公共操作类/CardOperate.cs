using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace 脸滚键盘
{
    public static class CardOperate
    {
        public static void SetWindowsMiddle(MouseButtonEventArgs e, Window card)
        {
            Point p = Mouse.GetPosition(e.Source as FrameworkElement);
            Point pointToWindow = (e.Source as FrameworkElement).PointToScreen(p);//转化为屏幕中的坐标
            card.Left = pointToWindow.X - card.Width / 2;
            card.Top = pointToWindow.Y - card.Height / 2;
            card.WindowStartupLocation = WindowStartupLocation.Manual;
        }


        public static void TryToBuildBaseTable(string tagName)
        {
            string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}({0}id INTEGER PRIMARY KEY,名称 CHAR UNIQUE,备注 TEXT,权重 INTEGER);", tagName);
            SqliteOperate.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 在数据库中添加一个信息卡
        /// </summary>
        public static int AddCard(string tableName, string mainField)
        {
            //实际上是以名字为标识符
            if (false == string.IsNullOrEmpty(mainField) && false == string.IsNullOrEmpty(tableName))
            {
                string sql = string.Format("insert or ignore into {0} (名称) values ('{1}');", tableName, mainField);
                SqliteOperate.ExecuteNonQuery(sql);
                int lastuid = SqliteOperate.GetLastUid(tableName);
                return lastuid;
            }
            return -1;
        }


        public static void SaveMainInfo(WrapPanel[] wrapPanels, string tagName, string idValue)
        {
            foreach (WrapPanel wp in wrapPanels)
            {
                string sql = string.Empty;
                foreach (TextBox tb in wp.Children)
                {
                    if (string.IsNullOrEmpty(tb.Uid))
                    {
                        //不存在记录
                        if (false == string.IsNullOrEmpty(tb.Text))
                        {
                            //编辑框不为空，插入
                            sql += string.Format("insert or ignore into {0}{1}表 ({0}id, {1}) values ({2}, '{3}');", tagName, wp.Uid, idValue, tb.Text);
                        }
                    }
                    else
                    {
                        //存在记录，为空时删除，不为空时更新
                        if (string.IsNullOrEmpty(tb.Text))
                        {
                            sql += string.Format("delete from {0}{1}表 where {1}id = {2};", tagName, wp.Uid, tb.Uid);

                        }
                        else
                        {
                            sql += string.Format("update {0}{1}表 set {1}='{3}' where {1}id = {2};", tagName, wp.Uid, tb.Uid, tb.Text);
                        }


                    }
                }
                SqliteOperate.ExecuteNonQuery(sql);
            }
        }

        public static void SaveOtherInfo(WrapPanel[] wrapPanels, string tagName, string idValue)
        {
            foreach (WrapPanel wp in wrapPanels)
            {
                string sql = string.Empty;
                int n = 0;
                foreach (RadioButton rb in wp.Children)
                {
                    if (rb.IsChecked == true)
                    {
                        break;
                    }
                    n++;
                }
                sql += string.Format("replace into {0}{1}表 ({0}id, {1}) VALUES ({2}, '{3}');", tagName, wp.Uid, idValue, n);
                SqliteOperate.ExecuteNonQuery(sql);
            }
        }

        public static void FillMainInfo(WrapPanel[] wrapPanels, string tagName, string idValue)
        {

            foreach (WrapPanel wp in wrapPanels)
            {
                //尝试建立新表（IF NOT EXISTS）
                string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}{1}表({0}id INTEGER REFERENCES {0} ({0}id) ON DELETE CASCADE ON UPDATE CASCADE,{1} CHAR,{1}id INTEGER PRIMARY KEY);", tagName, wp.Uid);
                SqliteOperate.ExecuteNonQuery(sql);

                sql = string.Format("select * from {0}{1}表 where {0}id = {2};", tagName, wp.Uid, idValue);
                SQLiteDataReader reader = SqliteOperate.ExecuteQuery(sql);
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
        }

        static TextBox AddTextBox()
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


        public static void FillOtherInfo(WrapPanel[] wrapPanels, string tagName, string idValue)
        {
            foreach (WrapPanel wp in wrapPanels)
            {
                //尝试建立新表（IF NOT EXISTS）
                string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}{1}表({0}id INTEGER REFERENCES {0} ({0}id) ON DELETE CASCADE ON UPDATE CASCADE UNIQUE,{1} INT);", tagName, wp.Uid);
                SqliteOperate.ExecuteNonQuery(sql);

                sql = string.Format("select * from {0}{1}表 where {0}id = {2};", tagName, wp.Uid, idValue);
                SQLiteDataReader reader = SqliteOperate.ExecuteQuery(sql);
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
}
