﻿using System;
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
        // 数据库文件夹
        static string DbPath = Gval.Current.curBookPath;

        //与指定的数据库(实际上就是一个文件)建立连接
        private static SQLiteConnection CreateDatabaseConnection(string dbName = null)
        {
            dbName = dbName == null ? "card.db" : dbName;
            var dbFilePath = Path.Combine(DbPath, dbName);
            return new SQLiteConnection("DataSource = " + dbFilePath + ";foreign keys=true;");
        }

        // 使用全局静态变量保存连接
        private static SQLiteConnection connection = CreateDatabaseConnection();

        // 判断连接是否处于打开状态
        private static void Open(SQLiteConnection connection)
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="sql"></param>
        public static void ExecuteNonQuery(string sql)
        {
            // 确保连接打开
            Open(connection);

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
            Open(connection);

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
        /// 在数据库中添加一个新角色
        /// </summary>
        public static int AddRole(string roleName)
        {
            //实际上是以名字为标识符
            if (false == string.IsNullOrEmpty(roleName))
            {
                string sql = string.Format("insert or ignore into 角色 (名称) values ('{0}');", roleName);
                SqliteOperate.ExecuteNonQuery(sql);
                sql = "select last_insert_rowid() from 角色;";
                SQLiteDataReader reader = SqliteOperate.ExecuteQuery(sql);
                int lastuid = 0;
                while (reader.Read())
                {
                    lastuid = reader.GetInt32(0);
                }
                return lastuid;
            }
            return -1;
        }


    }
}
