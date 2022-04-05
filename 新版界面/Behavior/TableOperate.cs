﻿using RootNS.Brick;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Behavior
{
    public class TableOperate
    {
        public static void TryToBuildIndexTables()
        {
            string dbName = "index";
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE 书库 (Uid CHAR PRIMARY KEY, [Index] INTEGER DEFAULT (0), Name CHAR NOT NULL, Summary CHAR, Price DOUBLE DEFAULT (0), CurrentYear INTEGER DEFAULT (0), IsDel BOOLEAN DEFAULT(False));");
            sql += string.Format("CREATE INDEX 书库Uid ON 书库(Uid);");
            CSqlitePlus.PoolOperate.Add(dbName);
            CSqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
            TryToBuildBookTables(dbName);
        }


        public static void TryToBuildBookTables(string dbName)
        {
            string sql = string.Empty;
            sql += GetSqlStringForBookTables("目录");
            sql += GetSqlStringForBookTables("记事");
            sql += GetSqlStringForBookTables("卡片");
            sql += GetSqlStringForBookTables("地图");
            sql += string.Format("CREATE TABLE IF NOT EXISTS 设置 (Key CHAR PRIMARY KEY, Value CHAR);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 设置Key ON 设置(Key);");
            CSqlitePlus.PoolOperate.Add(dbName);
            CSqlitePlus.PoolDict[dbName].ExecuteNonQuery(sql);
        }

        private static string GetSqlStringForBookTables(string dbName)
        {
            string sql = string.Empty;
            sql += string.Format("CREATE TABLE IF NOT EXISTS {0} (Uid CHAR PRIMARY KEY, [Index] INTEGER DEFAULT (0), Pid CHAR, Title CHAR, IsDir BOOLEAN DEFAULT(False), Text TEXT, Summary CHAR, TabName CHAR, PointX DOUBLE, PointY DOUBLE, WordsCount INTEGER, IsExpanded BOOLEAN DEFAULT(False), IsChecked BOOLEAN DEFAULT(False), IsDel BOOLEAN DEFAULT(False));", dbName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Uid ON {0}(Uid);", dbName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}Pid ON {0}(Pid);", dbName);
            sql += string.Format("CREATE INDEX IF NOT EXISTS {0}TabName ON {0}(TabName);", dbName);
            return sql;
        }
    }
}
