using System.Data.SQLite;
using System.IO;

namespace 脸滚键盘.公共操作类
{
    class SqliteOperate
    {

        public SqliteOperate(string DbPath, string DbName = null)
        {
            NewConnection(DbPath, DbName);
            Gval.Flag.IsSqlconnOpening = true;
        }

        ~SqliteOperate()
        {
            Gval.Flag.IsSqlconnOpening = false;
            //Close();
        }

        /// <summary>
        /// 数据库建立连接
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        private SQLiteConnection CreateDatabaseConnection(string dbPath, string dbName = null)
        {
            // 数据库文件夹
            dbName = string.IsNullOrEmpty(dbName) ? "bookData.db" : dbName;
            string dbFilePath = Path.Combine(dbPath, dbName);
            return new SQLiteConnection("DataSource = " + dbFilePath + ";foreign keys=true;");
        }

        // 使用全局静态变量保存连接
        private SQLiteConnection connection;

        // 打开连接
        private void Open()
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                SQLiteCommand command = connection.CreateCommand();
            }
        }

        // 关闭连接
        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
                SQLiteConnection.ClearAllPools();
            }
            
        }


        /// <summary>
        /// 刷新数据库连接
        /// </summary>
        public void NewConnection(string DbPath, string DbName = null)
        {
            Close();
            connection = CreateDatabaseConnection(DbPath, DbName);
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="sql"></param>
        public void ExecuteNonQuery(string sql)
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
        public SQLiteDataReader ExecuteQuery(string sql)
        {
            // 确保连接打开
            Open();

            using (SQLiteTransaction tr = connection.BeginTransaction())
            {
                using (SQLiteCommand command = connection.CreateCommand())
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
        public int GetLastUid(string tableName)
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
