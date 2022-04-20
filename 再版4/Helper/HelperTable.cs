using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version4.Model;

namespace Version4.Helper
{
    public class HelperTable
    {
        public static void TryToBuildIndexDatabase(string dbName = "index")
        {
            string sql = string.Empty;
            sql += GetSqlStringForCreateBooksBankTable();
            foreach (Material.MaterialTabName item in Enum.GetValues(typeof(Material.MaterialTabName)))
            {
                sql += GetSqlStringForCreateNodeTable(item.ToString());
            }
            sql += GetSqlStringForCreateCardTags(dbName);
            foreach (Book.CardTabName item in Enum.GetValues(typeof(Book.CardTabName)))
            {
                sql += GetSqlStringForCreateCardTable(item.ToString());
            }
            sql += GetSqlStringForCreateSiteTable("地图");
            sql += GetSqlStringForCreateSettingsTable();
            SqlitePlus.PoolOperate.Add(dbName);
            SqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
        }


        public static void TryToBuildBookTables(string dbName)
        {
            string sql = string.Empty;

            foreach (Book.ChapterTabName item in Enum.GetValues(typeof(Book.ChapterTabName)))
            {
                sql += GetSqlStringForCreateNodeTable(item.ToString());
            }
            foreach (Book.NoteTabName item in Enum.GetValues(typeof(Book.NoteTabName)))
            {
                sql += GetSqlStringForCreateNodeTable(item.ToString());
            }
            sql += GetSqlStringForCreateCardTags(dbName);
            foreach (Book.CardTabName item in Enum.GetValues(typeof(Book.CardTabName)))
            {
                sql += GetSqlStringForCreateCardTable(item.ToString());
            }
            sql += GetSqlStringForCreateSiteTable("地图");
            sql += GetSqlStringForCreateSettingsTable();
            SqlitePlus.PoolOperate.Add(dbName);
            SqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
        }




        private static string GetSqlStringForCreateBooksBankTable()
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS 书库 ([Index] INTEGER DEFAULT (0), Uid CHAR PRIMARY KEY, Name CHAR NOT NULL, Summary CHAR, Price DOUBLE DEFAULT (0), CurrentYear INTEGER DEFAULT (0), IsDel BOOLEAN DEFAULT(False));");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 书库Uid ON 书库(Uid);");
            return sql;
        }

        private static string GetSqlStringForCreateNodeTable(string tableName)
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS {0} ([Index] INTEGER DEFAULT (0), Uid CHAR PRIMARY KEY, Pid CHAR DEFAULT \"\", Title CHAR, Text TEXT, Summary CHAR, WordsCount INTEGER (0), IsDir BOOLEAN DEFAULT(False), IsExpanded BOOLEAN DEFAULT(False), IsChecked BOOLEAN DEFAULT(False), IsDel BOOLEAN DEFAULT(False));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Uid ON {0}(Uid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Pid ON {0}(Pid);", tableName);
            return sql;
        }

        private static bool IsExistsTable(string dbName, string tableName)
        {
            string sql = string.Format("SELECT name FROM sqlite_master WHERE name='{0}';", tableName);
            SqlitePlus.PoolOperate.Add(dbName);
            SQLiteDataReader reader = SqlitePlus.PoolDict[dbName].ExecuteQuery(sql);
            if (reader.Read())
            {
                reader.Close();
                return true;
            }
            else
            {
                reader.Close();
                return false;
            }

        }

        private static void CardDesignDefault(string dbName)
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS 卡设计 ([Index] INTEGER DEFAULT (0), Uid CHAR PRIMARY KEY, Title CHAR NOT NULL, TabName CHAR NOT NULL);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Uid ON {0}(Uid);", "卡设计");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Title ON {0}(Title);", "卡设计");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}TabName ON {0}(TabName);", "卡设计");
            string[] forRole = { "别称", "身份", "外观", "阶级", "所属", "物品", "能力", "经历" };
            string[] forOther = { "别称", "身份", "外观", "阶级", "所属", "物品", "能力", "经历" };
            string[] forWorld = { "别称", "描述", "阶级" };
            for (int i = 0; i < forRole.Length; i++)
            {
                sql += string.Format("REPLACE INTO 卡设计 ([Index], Uid, Title, TabName) values ('{0}', '{1}', '{2}', '{3}');", i, Guid.NewGuid().ToString(), forRole[i], "角色");
                SqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
            }
            for (int i = 0; i < forOther.Length; i++)
            {
                sql += string.Format("REPLACE INTO 卡设计 ([Index], Uid, Title, TabName) values ('{0}', '{1}', '{2}', '{3}');", i, Guid.NewGuid().ToString(), forOther[i], "其他");
                SqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
            }
            for (int i = 0; i < forWorld.Length; i++)
            {
                sql += string.Format("REPLACE INTO 卡设计 ([Index], Uid, Title, TabName) values ('{0}', '{1}', '{2}', '{3}');", i, Guid.NewGuid().ToString(), forWorld[i], "世界");
                SqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
            }
            SqlitePlus.PoolOperate.Add(dbName);
            SqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
        }

        private static string GetSqlStringForCreateCardTags(string dbName)
        {
            string sql = string.Empty;
            if (IsExistsTable(dbName, "卡设计") == false)
            {
                CardDesignDefault(dbName);
            }
            sql += string.Format("CREATE TABLE IF NOT EXISTS 卡片 ([Index] INTEGER DEFAULT (0), Uid CHAR PRIMARY KEY, Pid CHAR DEFAULT \"\", Tid CHAR DEFAULT \"\" REFERENCES 卡设计(Uid) ON DELETE CASCADE ON UPDATE CASCADE, Title CHAR NOT NULL UNIQUE, TabName CHAR NOT NULL);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Uid ON {0}(Uid);", "卡片");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Pid ON {0}(Pid);", "卡片");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Tid ON {0}(Tid);", "卡片");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}TabName ON {0}(TabName);", "卡片");
            return sql;
        }
        private static string GetSqlStringForCreateCardTable(string tableName)
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS {0} ([Index] INTEGER DEFAULT (0), Uid CHAR PRIMARY KEY, Title CHAR, Summary CHAR, Weight INTEGER DEFAULT (0), BornYear INTEGER DEFAULT (0), IsChecked BOOLEAN DEFAULT(False), IsDel BOOLEAN DEFAULT(False));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Uid ON {0}(Uid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Title ON {0}(Title);", tableName);
            return sql;
        }


        private static string GetSqlStringForCreateSiteTable(string tableName)
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS {0} ([Index] INTEGER DEFAULT (0), Uid CHAR PRIMARY KEY, Md5 CHAR DEFAULT \"\", Title CHAR, Summary CHAR, PointX DOUBLE, PointY DOUBLE, IsChecked BOOLEAN DEFAULT(False), IsDel BOOLEAN DEFAULT(False));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Uid ON {0}(Uid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Md5 ON {0}(Md5);", tableName);
            return sql;
        }


        private static string GetSqlStringForCreateSettingsTable()
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS 设置 (Key CHAR PRIMARY KEY, Value CHAR);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 设置Key ON 设置(Key);");
            return sql;
        }


    }
}
