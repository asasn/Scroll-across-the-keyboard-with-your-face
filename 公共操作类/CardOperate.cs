using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace 脸滚键盘
{
    public static class CardOperate
    {
        public static void SetWindowsMiddle(MouseButtonEventArgs e, Window card)
        {
            Point p = Mouse.GetPosition(e.Source as FrameworkElement);
            Point pointToWindow = (e.Source as FrameworkElement).PointToScreen(p);//转化为屏幕中的坐标
            card.Left = 305;
            card.Top = 115;
            //card.Left = pointToWindow.X - card.Width / 2;
            //card.Top = pointToWindow.Y - card.Height / 2;
            //card.WindowStartupLocation = WindowStartupLocation.Manual;
        }


        public static void TryToBuildBaseTable(string tagName)
        {
            string sql = string.Empty;
            if (tagName == "角色")
            {
                sql = string.Format("CREATE TABLE IF NOT EXISTS {0}({0}id INTEGER PRIMARY KEY AUTOINCREMENT, 名称 CHAR UNIQUE,备注 TEXT,权重 INTEGER,相对年龄 CHAR);", tagName);             
            }
            else
            {
                sql = string.Format("CREATE TABLE IF NOT EXISTS {0}({0}id INTEGER PRIMARY KEY,名称 CHAR UNIQUE,备注 TEXT,权重 INTEGER);", tagName);
            }
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
            int w = 0;
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
                            //将外面带入的sql语句提交，并且清空
                            SqliteOperate.ExecuteNonQuery(sql);
                            sql = string.Empty;

                            //编辑框不为空，插入，这里的sql语句使用单条语句，以便获取最后填入的id
                            sql = string.Format("insert or ignore into {0}{1}表 ({0}id, {1}) values ({2}, '{3}');", tagName, wp.Uid, idValue, tb.Text);
                            SqliteOperate.ExecuteNonQuery(sql);
                            int lastuid = SqliteOperate.GetLastUid(tagName + wp.Uid + "表");
                            tb.Uid = lastuid.ToString();
                            sql = string.Empty; //注意清空，以免影响后续语句运行
                            w++;
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
                            w++;
                        }


                    }
                }
                SqliteOperate.ExecuteNonQuery(sql);
            }
            string sql2 = string.Format("update {0} set 权重={1} where {0}id = {2};", tagName, w, idValue);
            SqliteOperate.ExecuteNonQuery(sql2);
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
                    TextBox tb = CardOperate.AddTextBox();
                    tb.Text = t;
                    tb.Uid = n.ToString();
                    wp.Children.Add(tb);
                }
            }
        }

        public static TextBox AddTextBox()
        {
            TextBox tb = new TextBox();
            tb.MinWidth = 30;
            tb.MinHeight = 0;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Text = "";
            //tb.BorderBrush = Brushes.Blue;
            tb.BorderThickness = new Thickness(0, 0, 0, 1);
            tb.Margin = new Thickness(10, 0, 0, 0);
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.Padding = new Thickness(2);
            HandyControl.Controls.BorderElement.SetCornerRadius(tb, new CornerRadius(0));
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
