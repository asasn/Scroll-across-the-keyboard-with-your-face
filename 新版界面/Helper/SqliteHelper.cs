﻿using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace RootNS.Helper
{
    internal class SqliteHelper
    {

        public SqliteHelper(string dbPath, string dbName = null)
        {
            Close();
            Connection = CreateDatabaseConnection(dbPath, dbName);
            DbName = dbName;
            this.IsSqlconnOpening = true;
        }

        ~SqliteHelper()
        {
            this.IsSqlconnOpening = false;
            SQLiteConnection.ClearAllPools();
        }


        // 使用全局静态变量保存连接
        public readonly SQLiteConnection Connection;
        public string DbName;
        public bool IsSqlconnOpening;
        public static Dictionary<string, SqliteHelper> PoolDict { get; set; } = new Dictionary<string, SqliteHelper>();

        public struct PoolOperate
        {
            /// <summary>
            /// 检校字典中是否存在数据库对象，如果不存在则添加
            /// </summary>
            /// <param name="dbPath"></param>
            /// <param name="dbName"></param>
            public static void Add(string dbName)
            {
                if (dbName == null || PoolDict.ContainsKey(dbName) == true)
                {
                    return;
                }
                else
                {
                    PoolDict.Add(dbName, new SqliteHelper(Gval.Path.Books, dbName + ".db"));
                }
            }

            /// <summary>
            /// 关闭数据库连接并且从字典中删除
            /// </summary>
            /// <param name="keyName"></param>
            public static void Remove(string keyName)
            {
                //关闭数据库连接并从字典中删除
                if (PoolDict.ContainsKey(keyName) == true)
                {
                    PoolDict[keyName].Close();
                    PoolDict.Remove(keyName);
                }
                else
                {
                    return;
                }
            }
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

        // 打开连接
        private void Open()
        {
            if (Connection != null && Connection.State != System.Data.ConnectionState.Open)
            {
                Connection.Open();
                //System.Console.WriteLine(string.Format("打开数据库 {0} 的连接", this.DbName));
            }
        }

        // 关闭连接
        public void Close()
        {
            if (Connection != null)
            {
                try
                {
                    Connection.Close();
                    Console.WriteLine(string.Format("关闭数据库 {0} 的连接", this.DbName));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("关闭数据库失败！\n{0}", ex));
                }
            }
        }

        /// <summary>
        /// 执行非查询语句
        /// </summary>
        /// <param name="sql"></param>
        public void ExecuteNonQuery(string sql)
        {
            // 确保连接打开
            Open();

            using (SQLiteTransaction tr = Connection.BeginTransaction())
            {
                using (SQLiteCommand command = Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                try
                {
                    tr.Commit();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

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
            using (SQLiteTransaction tr = Connection.BeginTransaction())
            {
                using (SQLiteCommand command = Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    try
                    {
                        // 执行查询，返回一个SQLiteDataReader对象
                        return command.ExecuteReader();
                    }
                    catch (System.Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                        return null;
                    }
                }
            }
        }


        /// <summary>
        /// 压缩数据库
        /// </summary>
        /// <param name="cSqlite"></param>
        public void Vacuum()
        {
            SQLiteCommand cmd = new SQLiteCommand("VACUUM", Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 判断某列是否存在
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool ReaderExists(SQLiteDataReader reader, string columnName)
        {
            return reader.GetSchemaTable().Select("ColumnName='" + columnName + "'").Length > 0;
        }
    }
}
