using NSMain.Bricks;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NSMain.Cards
{
    class CCards
    {
        public class 属性条目
        {
            public string Uid { get; set; }
            public string Text { get; set; }
        }

        public static void SaveNickName(string curBookName, string typeOfTree, URecord uRecord, string pid)
        {
            string tableName = typeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
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
                        cSqlite.ExecuteNonQuery(sql);
                        sql = string.Empty;

                        //编辑框不为空，插入，这里的sql语句使用单条语句，以便获取最后填入的id
                        string guid = tipBox.Uid = Guid.NewGuid().ToString();
                        sql = string.Format("insert or ignore into {0}从表 (Uid, Pid, Tid, Text) values ('{1}', '{2}', '{3}', '{4}');", tableName, guid, pid, uRecord.Uid, tipBox.Text.Replace("'", "''"));
                        cSqlite.ExecuteNonQuery(sql);
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
            cSqlite.ExecuteNonQuery(sql);

        }

        public static void SaveMainInfo(string curBookName, string typeOfTree, UIElementCollection wrapPanels, string pid)
        {
            string tableName = typeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            int w = 0;
            string sql = string.Empty;
            foreach (URecord uRecord in wrapPanels)
            {
                WrapPanel wp = uRecord.WpMain;
                sql = string.Empty;
                foreach (UTip tipBox in wp.Children)
                {
                    if (string.IsNullOrEmpty(tipBox.Text))
                    {
                        tipBox.Visibility = Visibility.Collapsed;
                    }
                    if (string.IsNullOrEmpty(tipBox.Uid))
                    {
                        if (false == string.IsNullOrEmpty(tipBox.Text))
                        {
                            //将外面带入的sql语句提交，并且清空
                            cSqlite.ExecuteNonQuery(sql);
                            sql = string.Empty;

                            //编辑框不为空，插入，这里的sql语句使用单条语句，以便获取最后填入的id
                            string guid = tipBox.Uid = Guid.NewGuid().ToString();
                            sql = string.Format("insert or ignore into {0}从表 (Uid, Pid, Tid, Text) values ('{1}', '{2}', '{3}', '{4}');", tableName, guid, pid, uRecord.Uid, tipBox.Text.Replace("'", "''"));
                            cSqlite.ExecuteNonQuery(sql);
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
                cSqlite.ExecuteNonQuery(sql);
            }

            sql = string.Format("select * from {0}从表 where Pid='{1}' AND Tid=(select Uid from {0}属性表 where Text='别称');", tableName, pid);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            while (reader.Read())
            {
                w += 1;
            }
            reader.Close();
            sql = string.Format("update {0}主表 set 权重={1} where Uid='{2}';", tableName, w, pid);
            cSqlite.ExecuteNonQuery(sql);

        }



        public static void FillMainInfo(string curBookName, string typeOfTree, UIElementCollection wrapPanels, string pid)
        {
            string tableName = typeOfTree;
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[curBookName];
            foreach (URecord box in wrapPanels)
            {
                WrapPanel wp = box.WpMain;
                string sql = string.Format("select * from {0}从表 where Tid='{1}' AND Pid='{2}';", tableName, box.Uid, pid);
                SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
                wp.Children.Clear();
                while (reader.Read())
                {
                    UTip tipBox = new UTip(box, reader["Text"].ToString());
                    tipBox.Uid = reader["Uid"].ToString();
                }
                reader.Close();
            }

        }



    }
}
