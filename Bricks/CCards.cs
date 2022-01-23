using System;
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
        public static void SaveNickName(string curBookName, string typeOfTree, URecord uRecord, string pid)
        {
            string tableName = typeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[curBookName];
            int w = 0;

            string sql = string.Empty;
            foreach (UTip tipBox in uRecord.WpMain.Children)
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
                        string guid = tipBox.Uid = Guid.NewGuid().ToString();
                        sql = string.Format("insert or ignore into {0}从表 (Uid, Pid, Tid, Text) values ('{1}', '{2}', '{3}', '{4}');", tableName, guid, pid, uRecord.Uid, tipBox.Text.Replace("'", "''"));
                        sqlConn.ExecuteNonQuery(sql);
                        sql = string.Empty; //注意清空，以免影响后续语句运行                            
                    }
                }
                else
                {
                    //存在记录，为空时删除，不为空时更新
                    if (string.IsNullOrEmpty(tipBox.Text))
                    {
                        sql += string.Format("delete from {0}从表 where Uid='{1}';", tableName, tipBox.Uid);
                        w--;
                    }
                    else
                    {
                        if ((bool)tipBox.Tag == true)
                        {
                            sql += string.Format("update {0}从表 set Text='{1}' where Uid='{2}' AND Pid='{3}' AND Tid='{4}';", tableName, tipBox.Text.Replace("'", "''"), tipBox.Uid, pid, uRecord.Uid);
                        }
                    }
                }
                w++;
                tipBox.Tag = false;
            }
            sqlConn.ExecuteNonQuery(sql);

        }

        public static void SaveMainInfo(string curBookName, string typeOfTree, UIElementCollection wrapPanels, string pid)
        {
            string tableName = typeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[curBookName];
            int w = 0; 
            string sql = string.Empty;
            foreach (URecord uRecord in wrapPanels)
            {
                WrapPanel wp = uRecord.WpMain;
                sql = string.Empty;
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
                            string guid = tipBox.Uid = Guid.NewGuid().ToString();
                            sql = string.Format("insert or ignore into {0}从表 (Uid, Pid, Tid, Text) values ('{1}', '{2}', '{3}', '{4}');", tableName, guid, pid, uRecord.Uid, tipBox.Text.Replace("'", "''"));
                            sqlConn.ExecuteNonQuery(sql);
                            sql = string.Empty; //注意清空，以免影响后续语句运行                            
                        }
                    }
                    else
                    {
                        //存在记录，为空时删除，不为空时更新
                        if (string.IsNullOrEmpty(tipBox.Text))
                        {
                            sql += string.Format("delete from {0}从表 where Uid='{1}';", tableName, tipBox.Uid);
                            w--;
                        }
                        else
                        {
                            if ((bool)tipBox.Tag == true)
                            {
                                sql += string.Format("update {0}从表 set Text='{1}' where Uid='{2}' AND Pid='{3}' AND Tid='{4}';", tableName, tipBox.Text.Replace("'", "''"), tipBox.Uid, pid, uRecord.Uid);
                            }
                        }
                    }
                    w++;
                    tipBox.Tag = false;
                }
                sqlConn.ExecuteNonQuery(sql);
            }

            sql = string.Format("select * from {0}从表 where Pid='{1}' AND Tid=(select Uid from {0}属性表 where Text='别称');", tableName, pid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                w += 1;
            }
            reader.Close();
            sql = string.Format("update {0}主表 set 权重={1} where Uid='{2}';", tableName, w, pid);
            sqlConn.ExecuteNonQuery(sql);

        }



        public static void FillMainInfo(string curBookName, string typeOfTree, UIElementCollection wrapPanels, string pid)
        {
            string tableName = typeOfTree;
            CSqlitePlus sqlConn = GlobalVal.SQLClass.Pools[curBookName];
            foreach (URecord box in wrapPanels)
            {
                WrapPanel wp = box.WpMain;
                string sql = string.Format("select * from {0}从表 where Tid='{1}' AND Pid='{2}';", tableName, box.Uid, pid);
                SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
                wp.Children.Clear();
                while (reader.Read())
                {
                    UTip tipBox = new UTip(box, reader["Text"].ToString());
                    tipBox.Uid = reader["Uid"].ToString();
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
