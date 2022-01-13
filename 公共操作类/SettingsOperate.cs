using System.Configuration;
using System.Data.SQLite;

namespace 脸滚键盘.公共操作类
{
    class SettingsOperate
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
        private static void InitMySettings(string bookName)
        {
            SqlConn = Gval.SQLClass.Pools[bookName];
            if (bookName == "index")
            {
                TableName = "公共";
            }
            else
            {
                TableName = "本书";
            }

            string sql = string.Format("CREATE TABLE IF NOT EXISTS {0}设置表 (Key CHAR PRIMARY KEY, Value CHAR);", TableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}设置表Key ON {0}设置表(Key);", TableName);
            SqlConn.ExecuteNonQuery(sql);
        }

        static string TableName;
        static SqliteOperate SqlConn;

        public static string Get(string bookName, string key)
        {
            InitMySettings(bookName);
            string value = null;
            string sql = string.Format("SELECT * FROM {0}设置表 where Key='{1}';", TableName, key.Replace("'", "''"));
            SQLiteDataReader reader = SqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                value = reader["Value"].ToString();
            }
            reader.Close();
            return value;
        }

        public static void Set(string bookName, string key, string value)
        {
            InitMySettings(bookName);
            string sql = string.Format("REPLACE INTO {0}设置表 (Key, Value) VALUES ('{1}', '{2}');", TableName, key.Replace("'", "''"), value.Replace("'", "''"));
            SqlConn.ExecuteNonQuery(sql);
        }


    }


}
