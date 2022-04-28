using RootNS.View;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Helper
{
    class DataOut
    {
        public static Book CreateNewBook(string bookName)
        {
            Book newBook = new Book();
            newBook.Name = bookName;
            TableHelper.TryToBuildBookTables(bookName);
            string sql = string.Format("INSERT OR IGNORE INTO 书库 (Uid, [Index], Name) VALUES ('{0}', '{1}', '{2}');", newBook.Uid, newBook.Index, newBook.Name.Replace("'", "''"));
            SqliteHelper.PoolDict["index"].ExecuteNonQuery(sql);
            Gval.BooksBank.Add(newBook);
            return newBook;
        }

        /// <summary>
        /// 在书库数据库中更新记录
        /// </summary>
        /// <param name="book"></param>
        public static void UpdateBookInfo(Book book)
        {
            string sql = string.Format("UPDATE 书库 SET [index]='{0}', Name='{1}', Summary='{2}', Price='{3}', CurrentYear='{4}', IsDel='{5}' WHERE Uid='{6}';", book.Index, book.Name.Replace("'", "''"), book.Summary.Replace("'", "''"), book.Price, book.CurrentYear, book.IsDel, book.Uid);
            SqliteHelper.PoolDict["index"].ExecuteNonQuery(sql);
        }

        public static void DeleteBook(Book book)
        {
            string sql = string.Format("DELETE FROM 书库 WHERE Uid='{0}';", book.Uid);
            SqliteHelper.PoolDict["index"].ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 从表中删除节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="oldTabName"></param>
        /// <param name="newTabName"></param>
        public static void RemoveNodeFromTable(Node node)
        {
            string sql = string.Empty;
            sql += string.Format("DELETE FROM {0} WHERE Uid='{1}';", node.TabName, node.Uid);
            SqliteHelper.PoolDict[node.OwnerName].ExecuteNonQuery(sql);
        }


        public static void CreateNewNode(Node node)
        {
            string sql = string.Format("INSERT OR IGNORE INTO {0} ([Index], Uid, Pid, Title, Text, Summary, WordsCount, IsDir, IsExpanded, IsChecked, IsDel) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');", node.TabName.Replace("'", "''"), node.Index, node.Uid, node.Pid, node.Title.Replace("'", "''"), node.Text.Replace("'", "''"), node.Summary.Replace("'", "''"), node.WordsCount, node.IsDir, node.IsExpanded, node.IsChecked, node.IsDel);
            SqliteHelper.PoolDict[node.OwnerName].ExecuteNonQuery(sql);
        }
        public static void CreateNewCard(Card card)
        {
            string sql = string.Format("INSERT OR IGNORE INTO {0} ([Index], Uid, Title, Summary, Weight, BornYear, IsChecked, IsDel) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');", card.TabName.Replace("'", "''"), card.Index, card.Uid, card.Title.Replace("'", "''"), card.Summary.Replace("'", "''"), card.Weight, card.BornYear, card.IsChecked, card.IsDel);
            SqliteHelper.PoolDict[card.OwnerName].ExecuteNonQuery(sql);
        }

        public static void UpdateNodeProperty(Node node, string fieldName, string value)
        {
            string sql = string.Format("UPDATE {0} SET [{1}]='{2}' WHERE Uid='{3}' AND EXISTS(select * from sqlite_master where name='{0}' and sql like '%{1}%');", node.TabName.Replace("'", "''"), fieldName, value.Replace("'", "''"), node.Uid);
            SqliteHelper.PoolDict[node.OwnerName].ExecuteNonQuery(sql);
        }


        public static void ReplaceIntoCardDesign(Card.Tip tip)
        {
            string sql = String.Empty;
            if (string.IsNullOrWhiteSpace(tip.Title) == true)
            {
                sql = string.Format("DELETE FROM 卡设计 WHERE Uid='{0}' AND TabName='{1}';", tip.Uid, tip.TabName); ;
            }
            else
            {
                sql = string.Format("REPLACE INTO 卡设计 ([Index], Uid, Title, TabName) values ('{0}', '{1}', '{2}', '{3}');", tip.Index, tip.Uid, tip.Title.Replace("'", "''"), tip.TabName);
            }
            SqliteHelper.PoolDict[tip.OwnerName].ExecuteNonQuery(sql);
        }


        public static void ReplaceIntoCard(Card card)
        {
            string sql = String.Empty;
            if (string.IsNullOrWhiteSpace(card.Title) == false)
            {
                sql += string.Format("REPLACE INTO {0} ([Index], Uid, Title, Summary, Weight, BornYear, IsChecked, IsDel) values ( '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');", card.TabName, card.Index, card.Uid, card.Title.Replace("'", "''"), card.Summary.Replace("'", "''"), card.Weight, card.BornYear, card.IsChecked, card.IsDel);
                foreach (Card.Line line in card.Lines)
                {
                    string tid = String.Empty;
                    string lineSql = string.Format("select Uid from 卡设计 where Title='{0}' AND TabName='{1}';", line.LineTitle, line.TabName);
                    SQLiteDataReader readerLine = SqliteHelper.PoolDict[card.OwnerName].ExecuteQuery(lineSql);
                    while (readerLine.Read())
                    {
                        tid = readerLine["Uid"] == DBNull.Value ? null : readerLine["Uid"].ToString();
                    }
                    readerLine.Close();
                    foreach (Card.Tip tip in line.Tips)
                    {
                        if (string.IsNullOrWhiteSpace(tip.Title) == true)
                        {
                            sql += string.Format("DELETE FROM 卡片 WHERE Uid='{0}';", tip.Uid);
                        }
                        else
                        {
                            sql += string.Format("REPLACE INTO 卡片 ([Index], Uid, Pid, Tid, Title, TabName) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');", tip.Index, tip.Uid, tip.Pid, tid, tip.Title.Replace("'", "''"), tip.TabName);
                        }
                    }
                }

            }
            SqliteHelper.PoolDict[card.OwnerName].ExecuteNonQuery(sql);
        }

        public static void RemoveCardFromTable(Card card)
        {
            string sql = String.Empty;
            sql += string.Format("DELETE FROM 卡片 WHERE Pid='{1}';", card.TabName, card.Uid);
            sql += string.Format("DELETE FROM {0} WHERE Uid='{1}';", card.TabName, card.Uid);
            SqliteHelper.PoolDict[card.OwnerName].ExecuteNonQuery(sql);
        }
    }
}
