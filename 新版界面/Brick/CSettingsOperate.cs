using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Brick
{
    public class CSettingsOperate
    {

        /// <summary>
        /// 获取设置值
        /// </summary>
        /// <param name="dbName">当前工作的书籍名称/数据库名称</param>
        /// <param name="key">设置键</param>
        /// <returns></returns>
        public static string Get(string dbName, string key)
        {
            string value = null;
            string sql = string.Format("SELECT * FROM 设置 where Key='{0}';", key.Replace("'", "''"));
            SQLiteDataReader reader = CSqlitePlus.PoolDict[dbName].ExecuteQuery(sql);
            while (reader.Read())
            {
                value = reader["Value"].ToString();
            }
            reader.Close();
            return value;
        }

        /// <summary>
        /// 保存设置值
        /// </summary>
        /// <param name="dbName">当前工作的书籍名称/数据库名称</param>
        /// <param name="key">设置键</param>
        /// <param name="value">设置值</param>
        public static void Set(string dbName, string key, string value)
        {
            string sql = string.Format("REPLACE INTO 设置 (Key, Value) VALUES ('{0}', '{1}');", key.Replace("'", "''"), value.Replace("'", "''"));
            CSqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
        }


    }
}
