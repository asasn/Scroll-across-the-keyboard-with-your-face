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
using static 脸滚键盘.公共操作类.TreeOperate;

namespace 脸滚键盘.公共操作类
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


        public static void TryToBuildBaseTable(string curBookName, string typeOfTree)
        {
            string tableName = typeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, curBookName + ".db");
            string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}主表 ({0}id PRIMARY KEY REFERENCES Tree_{0}(Uid) ON DELETE CASCADE ON UPDATE CASCADE, 名称 CHAR UNIQUE,备注 TEXT,权重 INTEGER,相对年龄 CHAR);", tableName);
            sqlConn.ExecuteNonQuery(sql);
            sqlConn.Close();
        }

        /// <summary>
        /// 在数据库中添加一个信息卡
        /// </summary>
        public static string AddCard(string curBookName, string typeOfTree, TreeViewNode newNode)
        {
            string tableName = typeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, curBookName + ".db");
            //实际上是以名字为标识符
            if (false == string.IsNullOrEmpty(newNode.NodeName) && false == string.IsNullOrEmpty(tableName))
            {
                string sql = string.Format("insert or ignore into {0}主表 ({0}id, 名称) values ('{1}', '{2}');", tableName, newNode.Uid, newNode.NodeName);
                sqlConn.ExecuteNonQuery(sql);
            }
            sqlConn.Close();
            return newNode.NodeName;

        }


        public static void SaveMainInfo(string curBookName, string typeOfTree, WrapPanel[] wrapPanels, string idValue)
        {
            string tableName = typeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, curBookName + ".db");
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
                            sqlConn.ExecuteNonQuery(sql);
                            sql = string.Empty;

                            //编辑框不为空，插入，这里的sql语句使用单条语句，以便获取最后填入的id
                            string guid = Guid.NewGuid().ToString();
                            sql = string.Format("insert or ignore into {0}{1}表 ({0}id, {1}, {1}id) values ('{2}', '{3}', '{4}');", tableName, wp.Uid, idValue, tb.Text, guid);
                            sqlConn.ExecuteNonQuery(sql);
                            tb.Uid = guid;
                            sql = string.Empty; //注意清空，以免影响后续语句运行
                            w++;
                        }
                    }
                    else
                    {
                        //存在记录，为空时删除，不为空时更新
                        if (string.IsNullOrEmpty(tb.Text))
                        {
                            sql += string.Format("delete from {0}{1}表 where {1}id = '{2}';", tableName, wp.Uid, tb.Uid);

                        }
                        else
                        {
                            sql += string.Format("update {0}{1}表 set {1}='{3}' where {1}id = '{2}';", tableName, wp.Uid, tb.Uid, tb.Text);
                            w++;
                        }


                    }
                }
                sqlConn.ExecuteNonQuery(sql);
            }
            string sql2 = string.Format("update {0}主表 set 权重={1} where {0}id = '{2}';", tableName, w, idValue);
            sqlConn.ExecuteNonQuery(sql2);
            sqlConn.Close();
        }



        public static void FillMainInfo(string curBookName, string typeOfTree, WrapPanel[] wrapPanels, string idValue)
        {
            string tableName = typeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, curBookName + ".db");
            foreach (WrapPanel wp in wrapPanels)
            {
                //尝试建立新表（IF NOT EXISTS）
                string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}{1}表 ({0}id CHAR REFERENCES {0}主表 ({0}id) ON DELETE CASCADE ON UPDATE CASCADE,{1} CHAR,{1}id CHAR PRIMARY KEY);", tableName, wp.Uid);
                sqlConn.ExecuteNonQuery(sql);

                sql = string.Format("select * from {0}{1}表 where {0}id = '{2}';", tableName, wp.Uid, idValue);
                SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
                wp.Children.Clear();
                while (reader.Read())
                {
                    string t = reader.GetString(1);
                    string n = reader.GetString(2);
                    TextBox tb = CardOperate.AddTextBox();
                    tb.Text = t;
                    tb.Uid = n.ToString();
                    wp.Children.Add(tb);
                }
                reader.Close();
            }
            sqlConn.Close();
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


    }
}
