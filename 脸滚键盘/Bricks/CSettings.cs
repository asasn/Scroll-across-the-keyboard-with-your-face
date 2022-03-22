using System.Configuration;
using System.Data.SQLite;

namespace NSMain.Bricks
{
    class CSettings
    {
        public static string Get(string key)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string value = cfa.AppSettings.Settings[key].Value;
            return value;
        }

        public static void Set(string key, string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();
        }
    }

    public class MySettings
    {
        /// <summary>
        /// 初始化填入的参数
        /// </summary>
        /// <param name="curBookName"></param>
        private static void InitParameters(string curBookName)
        {
            Parameters.CSqlite = GlobalVal.SQLClass.Pools[curBookName];
            if (curBookName == "index")
            {
                Parameters.TableName = "公共";
            }
            else
            {
                Parameters.TableName = "本书";
            }
        }

        public static class Parameters
        {
            public static string TableName;
            public static CSqlitePlus CSqlite;
        }

        /// <summary>
        /// 获取设置值
        /// </summary>
        /// <param name="curBookName">当前工作的书籍名称</param>
        /// <param name="key">设置键</param>
        /// <returns></returns>
        public static string Get(string curBookName, string key)
        {
            InitParameters(curBookName);
            string value = null;
            string sql = string.Format("SELECT * FROM {0}设置表 where Key='{1}';", Parameters.TableName, key.Replace("'", "''"));
            SQLiteDataReader reader = Parameters.CSqlite.ExecuteQuery(sql);
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
        /// <param name="curBookName">当前工作的书籍名称</param>
        /// <param name="key">设置键</param>
        /// <param name="value">设置值</param>
        public static void Set(string curBookName, string key, string value)
        {
            InitParameters(curBookName);
            string sql = string.Format("REPLACE INTO {0}设置表 (Key, Value) VALUES ('{1}', '{2}');", Parameters.TableName, key.Replace("'", "''"), value.Replace("'", "''"));
            Parameters.CSqlite.ExecuteNonQuery(sql);
        }


    }


}
