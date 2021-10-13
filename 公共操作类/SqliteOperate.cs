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
        private static SQLiteConnection CreateDatabaseConnection(string dbName = null)
        {
            // 数据库文件夹
            string DbPath = Gval.Current.curBookPath;

            dbName = dbName == null ? "card.db" : dbName;
            var dbFilePath = Path.Combine(DbPath, dbName);
            return new SQLiteConnection("DataSource = " + dbFilePath + ";foreign keys=true;");
        }

        // 使用全局静态变量保存连接
        private static SQLiteConnection connection;

        // 打开连接
        private static void Open()
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        // 关闭连接
        public static void Close()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }



        // 刷新数据库连接

        public static void Refresh()
        {
            Close();
            connection = CreateDatabaseConnection();
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="sql"></param>
        public static void ExecuteNonQuery(string sql)
        {
            // 确保连接打开
            Open();

            using (var tr = connection.BeginTransaction())
            {
                using (var command = connection.CreateCommand())
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

            using (var tr = connection.BeginTransaction())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;

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
            return lastuid;
        }

    }
}
