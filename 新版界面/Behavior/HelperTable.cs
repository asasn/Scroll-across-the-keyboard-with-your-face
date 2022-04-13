using RootNS.Brick;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Behavior
{
    public class HelperTable
    {
        public static void TryToBuildIndexDatabase()
        {
            string dbName = "index";
            string sql = string.Empty;
            sql += GetSqlStringForCreateBooksBankTable();
            foreach (Material.MaterialTabName item in Enum.GetValues(typeof(Material.MaterialTabName)))
            {
                sql += GetSqlStringForCreateNodeTable(item.ToString());
            }
            sql += GetSqlStringForCreateCardTags();
            foreach (Book.CardTabName item in Enum.GetValues(typeof(Book.CardTabName)))
            {
                sql += GetSqlStringForCreateCardTable(item.ToString());
            }
            sql += GetSqlStringForCreateSiteTable("地图");
            sql += GetSqlStringForCreateSettingsTable();
            CSqlitePlus.PoolOperate.Add(dbName);
            CSqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
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
            sql += GetSqlStringForCreateCardTags();
            foreach (Book.CardTabName item in Enum.GetValues(typeof(Book.CardTabName)))
            {
                sql += GetSqlStringForCreateCardTable(item.ToString());
            }
            sql += GetSqlStringForCreateSiteTable("地图");
            sql += GetSqlStringForCreateSettingsTable();
            CSqlitePlus.PoolOperate.Add(dbName);
            CSqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
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

        private static string GetSqlStringForCreateCardTags()
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS 卡片 ([Index] INTEGER DEFAULT (0), Uid CHAR PRIMARY KEY, Title CHAR NOT NULL UNIQUE, Tag CHAR NOT NULL);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Uid ON {0}(Uid);", "卡片");
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Tag ON {0}(Tag);", "卡片");
            return sql;
        }
        private static string GetSqlStringForCreateCardTable(string tableName)
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS {0} ([Index] INTEGER DEFAULT (0), Uid CHAR PRIMARY KEY,Pid CHAR DEFAULT \"\", Tid CHAR DEFAULT \"\" REFERENCES 卡片(Uid) ON DELETE CASCADE ON UPDATE CASCADE, Title CHAR, Summary CHAR, Weight INTEGER DEFAULT (0), BornYear INTEGER DEFAULT (0), IsChecked BOOLEAN DEFAULT(False), IsDel BOOLEAN DEFAULT(False));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Uid ON {0}(Uid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Pid ON {0}(Pid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Tid ON {0}(Tid);", tableName);
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
