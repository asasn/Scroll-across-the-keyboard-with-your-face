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
                sql += GetSqlStringForCreateTable(item.ToString());
            }
            foreach (Book.CardTabName item in Enum.GetValues(typeof(Book.CardTabName)))
            {
                sql += GetSqlStringForCreateTable(item.ToString());
            }
            sql += GetSqlStringForCreateTable("地图");
            sql += GetSqlStringForCreateSettingsTable();
            CSqlitePlus.PoolOperate.Add(dbName);
            CSqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
        }


        public static void TryToBuildBookTables(string dbName)
        {
            string sql = string.Empty;

            foreach (Book.ChapterTabName item in Enum.GetValues(typeof(Book.ChapterTabName)))
            {
                sql += GetSqlStringForCreateTable(item.ToString());
            }
            foreach (Book.NoteTabName item in Enum.GetValues(typeof(Book.NoteTabName)))
            {
                sql += GetSqlStringForCreateTable(item.ToString());
            }
            foreach (Book.CardTabName item in Enum.GetValues(typeof(Book.CardTabName)))
            {
                sql += GetSqlStringForCreateTable(item.ToString());
            }
            sql += GetSqlStringForCreateTable("地图");
            sql += GetSqlStringForCreateSettingsTable();
            CSqlitePlus.PoolOperate.Add(dbName);
            CSqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
        }


        private static string GetSqlStringForCreateSettingsTable()
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS 设置 (Key CHAR PRIMARY KEY, Value CHAR);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 设置Key ON 设置(Key);");
            return sql;
        }


        private static string GetSqlStringForCreateBooksBankTable()
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS 书库 (Uid CHAR PRIMARY KEY, [Index] INTEGER DEFAULT (0), Name CHAR NOT NULL, Summary CHAR, Price DOUBLE DEFAULT (0), CurrentYear INTEGER DEFAULT (0), IsDel BOOLEAN DEFAULT(False));");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 书库Uid ON 书库(Uid);");
            return sql;
        }

        private static string GetSqlStringForCreateTable(string tableName)
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS {0} (Uid CHAR PRIMARY KEY, [Index] INTEGER DEFAULT (0), Pid CHAR DEFAULT \"\", Title CHAR, IsDir BOOLEAN DEFAULT(False), Text TEXT, Summary CHAR, TabName CHAR, PointX DOUBLE, PointY DOUBLE, WordsCount INTEGER, IsExpanded BOOLEAN DEFAULT(False), IsChecked BOOLEAN DEFAULT(False), IsDel BOOLEAN DEFAULT(False));", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Uid ON {0}(Uid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Pid ON {0}(Pid);", tableName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}TabName ON {0}(TabName);", tableName);
            return sql;
        }
    }
}
