﻿using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NSMain.Bricks
{
    class CCards
    {
        public static void SetWindowsMiddle(MouseButtonEventArgs e, Window card)
        {
            Point p = Mouse.GetPosition(e.Source as FrameworkElement);
            _ = (e.Source as FrameworkElement).PointToScreen(p);//转化为屏幕中的坐标
            card.Left = 305;
            card.Top = 115;
            //card.Left = pointToWindow.X - card.Width / 2;
            //card.Top = pointToWindow.Y - card.Height / 2;
            //card.WindowStartupLocation = WindowStartupLocation.Manual;
        }


        public static void TryToBuildBaseTable(string curBookName, string typeOfTree)
        {
            string tableName = typeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[curBookName];
            string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}主表 ({0}id PRIMARY KEY, 名称 CHAR UNIQUE, 备注 TEXT, 权重 INTEGER DEFAULT (0), 诞生年份 CHAR, IsDel BOOLEAN DEFAULT (false));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}主表{0}id ON {0}主表({0}id);", tableName);
            sql += string.Format("CREATE TABLE IF NOT EXISTS {0}{1}表 ({0}id CHAR REFERENCES {0}主表 ({0}id) ON DELETE CASCADE ON UPDATE CASCADE,{1} CHAR,{1}id CHAR PRIMARY KEY);", tableName, "别称");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}{1}表{0}id ON {0}{1}表({0}id);", tableName, "别称");
            sqlConn.ExecuteNonQuery(sql);
        }

        ///// <summary>
        ///// 在数据库中添加一个信息卡
        ///// </summary>
        //public static string AddCard(string curBookName, string typeOfTree, TreeViewNode newNode)
        //{
        //    string tableName = typeOfTree;
        //    SqliteOperate sqlConn = GlobalVal.SQLClass.Pools[curBookName];
        //    //实际上是以名字为标识符
        //    if (false == string.IsNullOrEmpty(newNode.NodeName) && false == string.IsNullOrEmpty(tableName))
        //    {
        //        string sql = string.Format("insert or ignore into {0}主表 ({0}id, 名称) values ('{1}', '{2}');", tableName, newNode.Uid, newNode.NodeName.Replace("'", "''"));
        //        sqlConn.ExecuteNonQuery(sql);
        //    }
        //    
        //    return newNode.NodeName;

        //}


        public static void SaveMainInfo(string curBookName, string typeOfTree, UIElementCollection wrapPanels, string idValue)
        {
            string tableName = typeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[curBookName];
            int w = 0;
            foreach (URecord box in wrapPanels)
            {
                WrapPanel wp = box.WpMain;
                string sql = string.Empty;
                foreach (UTip tipBox in wp.Children)
                {
                    if (string.IsNullOrEmpty(tipBox.Uid))
                    {
                        //不存在记录
                        if (false == string.IsNullOrEmpty(tipBox.Text))
                        {
                            //将外面带入的sql语句提交，并且清空
                            sqlConn.ExecuteNonQuery(sql);
                            sql = string.Empty;

                            //编辑框不为空，插入，这里的sql语句使用单条语句，以便获取最后填入的id
                            string guid = Guid.NewGuid().ToString();
                            sql = string.Format("insert or ignore into {0}{1}表 ({0}id, {1}, {1}id) values ('{2}', '{3}', '{4}');", tableName, wp.Tag.ToString(), idValue, tipBox.Text.Replace("'", "''"), guid);
                            sqlConn.ExecuteNonQuery(sql);
                            tipBox.Uid = guid;
                            sql = string.Empty; //注意清空，以免影响后续语句运行                            
                        }
                    }
                    else
                    {
                        //存在记录，为空时删除，不为空时更新
                        if (string.IsNullOrEmpty(tipBox.Text))
                        {
                            sql += string.Format("delete from {0}{1}表 where {1}id = '{2}';", tableName, wp.Tag.ToString(), tipBox.Uid);
                            w--;
                        }
                        else
                        {
                            if ((bool)tipBox.Tag == true)
                            {
                                sql += string.Format("update {0}{1}表 set {1}='{2}' where {1}id = '{3}';", tableName, wp.Tag.ToString(), tipBox.Text.Replace("'", "''"), tipBox.Uid);
                            }
                        }
                    }
                    w++;
                    tipBox.Tag = false;
                }
                sqlConn.ExecuteNonQuery(sql);
            }
            string sql2 = string.Format("update {0}主表 set 权重={1} where {0}id = '{2}';", tableName, w, idValue);
            sqlConn.ExecuteNonQuery(sql2);

        }



        public static void FillMainInfo(string curBookName, string typeOfTree, UIElementCollection wrapPanels, string pid)
        {
            string tableName = typeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[curBookName];
            foreach (URecord box in wrapPanels)
            {
                WrapPanel wp = box.WpMain;
                //尝试建立新表（IF NOT EXISTS）
                string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}{1}表 ({0}id CHAR REFERENCES {0}主表 ({0}id) ON DELETE CASCADE ON UPDATE CASCADE,{1} CHAR,{1}id CHAR PRIMARY KEY);", tableName, wp.Tag.ToString());
                sql += string.Format("CREATE INDEX IF NOT EXISTS {0}{1}表{0}id ON {0}{1}表({0}id);", tableName, wp.Tag.ToString());
                sqlConn.ExecuteNonQuery(sql);

                sql = string.Format("select * from {0}{1}表 where {0}id = '{2}';", tableName, wp.Tag.ToString(), pid);
                SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
                wp.Children.Clear();
                while (reader.Read())
                {
                    UTip tipBox = new UTip(box, reader.GetString(1));
                    tipBox.Uid = reader.GetString(2);
                }
                reader.Close();
            }

        }

        private static void Tb_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        //public static TextBox AddTextBox()
        //{
        //    TextBox tb = new TextBox
        //    {
        //        MinWidth = 30,
        //        MinHeight = 0,
        //        TextWrapping = TextWrapping.Wrap,
        //        Text = "",
        //        BorderThickness = new Thickness(0, 0, 0, 1),
        //        Margin = new Thickness(10, 0, 0, 0),
        //        HorizontalAlignment = HorizontalAlignment.Left,
        //        VerticalAlignment = VerticalAlignment.Center,
        //        Padding = new Thickness(2)
        //    };
        //    HandyControl.Controls.BorderElement.SetCornerRadius(tb, new CornerRadius(0));
        //    return tb;
        //}


    }
}