using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace 脸滚键盘
{
    class SqliteOperate
    {



        //与指定的数据库(实际上就是一个文件)建立连接
        private static SQLiteConnection CreateDatabaseConnection(string dbPath = null, string dbName = null)
        {
            // 数据库文件夹

            dbPath = dbPath ?? Gval.Current.curBookPath;
            dbName = dbName ?? "bookData.db";
            var dbFilePath = Path.Combine(dbPath, dbName);
            return new SQLiteConnection("DataSource = " + dbFilePath + ";foreign keys=true;");
        }

        // 使用全局静态变量保存连接
        private static SQLiteConnection connection;

        //使用全局静态变量方便关闭
        private static SQLiteDataReader Reader;

        // 打开连接
        private static void Open()
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
            }
        }

        // 关闭连接
        public static void Close()
        {
            if (Reader != null && Reader.IsClosed == false)
            {
                Reader.Close();
            }
            if (connection != null)
            {
                connection.Close();                
            }
            System.Data.SQLite.SQLiteConnection.ClearAllPools();
        }


        /// <summary>
        /// 刷新数据库连接
        /// </summary>
        public static void NewConnection(string DbPath = null, string DbName = null)
        {            
            Close();            
            connection = CreateDatabaseConnection(DbPath, DbName);

        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="sql"></param>
        public static void ExecuteNonQuery(string sql)
        {
            // 确保连接打开
            Open();

            using (SQLiteTransaction tr = connection.BeginTransaction())
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
                tr.Commit();
            }
        }

        /// <summary>
        /// 执行查询语句
        /// </summary>
        /// <param name="sql"></param>
        public static SQLiteDataReader ExecuteQuery(string sql)
        {
            // 确保连接打开
            Open();

            using (SQLiteTransaction tr = connection.BeginTransaction())
            {
                using (SQLiteCommand command = connection.CreateCommand())
                {

                    command.CommandText = sql;

                    if (Reader != null && Reader.IsClosed == false)
                    {
                        Reader.Close();
                    }

                    // 执行查询会返回一个SQLiteDataReader对象
                    SQLiteDataReader reader = command.ExecuteReader();

                    return reader;
                    //reader.Read()方法会从读出一行匹配的数据到reader中。注意：是一行数据。

                }
                //tr.Commit();
            }
        }



        /// <summary>
        /// 获取最后添加记录的uid
        /// </summary>
        /// <returns></returns>
        public static int GetLastUid(string tableName)
        {
            string sql = string.Format("select last_insert_rowid() from {0};", tableName);
            SQLiteDataReader reader = ExecuteQuery(sql);
            int lastuid = -1;
            while (reader.Read())
            {
                lastuid = reader.GetInt32(0);
            }
            reader.Close();
            return lastuid;
        }

    }
}
