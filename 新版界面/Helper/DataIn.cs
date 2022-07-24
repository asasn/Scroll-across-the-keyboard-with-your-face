﻿using RootNS.View;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Helper
{
    class DataIn
    {
        /// <summary>
        /// 窗口打开后，开始用户操作前的载入数据流程
        /// </summary>
        public static void ReadyForBaseInfo()
        {
            IOHelper.CreateFolder(Gval.Path.Books);
            TableHelper.TryToBuildIndexDatabase();
            Gval.CurrentBook.Uid = SettingsHelper.Get(Gval.MaterialBook.Name, "CurBookUid")?.ToString();
            LoadBooksBank();
        }


        private static void LoadBooksBank()
        {
            string sql = string.Format("SELECT * FROM 书库 ORDER BY [Index];");
            SqliteHelper.PoolOperate.Add("index");
            SQLiteDataReader reader = SqliteHelper.PoolDict["index"].ExecuteQuery(sql);
            while (reader.Read())
            {
                Book book = new Book
                {
                    Uid = reader["Uid"].ToString(),
                    Index = Convert.ToInt32(reader["Index"]),
                    Name = reader["Name"].ToString(),
                    Summary = reader["Summary"].ToString(),
                    Price = Convert.ToDouble(reader["Price"]),
                    CurrentYear = Convert.ToInt64(reader["CurrentYear"]),
                    IsDel = (bool)reader["IsDel"],
                };
                if (Gval.CurrentBook.Uid != null && Gval.CurrentBook.Uid == book.Uid)
                {
                    Gval.CurrentBook = book;
                    SqliteHelper.PoolOperate.Add(Gval.CurrentBook.Name);
                }
                Gval.BooksBank.Add(book);
            }
            reader.Close();
        }


        public static Book LoadCurrentBookContent(Book book)
        {
            SqliteHelper.PoolOperate.Add(Gval.CurrentBook.Name);
            string sql = string.Format("SELECT * FROM 书库 WHERE Uid='{0}';", book.Uid);
            SQLiteDataReader reader = SqliteHelper.PoolDict["index"].ExecuteQuery(sql);
            while (reader.Read())
            {
                book.Uid = reader["Uid"].ToString();
                book.Index = Convert.ToInt32(reader["Index"]);
                book.Name = reader["Name"].ToString();
                book.Summary = reader["Summary"].ToString();
                book.Price = Convert.ToDouble(reader["Price"]);
                book.CurrentYear = Convert.ToInt64(reader["CurrentYear"]);
                book.IsDel = (bool)reader["IsDel"];
            }
            reader.Close();
            Gval.FlagLoadingCompleted = false;
            book.LoadForChapterTab();
            book.LoadForNoteTab();
            book.LoadForCardTab();
            Gval.FlagLoadingCompleted = true;
            return book;
        }


        public static Card CardDesginLoad(Card card)
        {
            if (Gval.CurrentBook.Name == null && card.OwnerName != "index")
            {
                return card;
            }
            Card.Line line = card.Lines[0];
            line.Tips.Clear();
            string sql = string.Format("SELECT * FROM 卡设计 WHERE TabName='{0}' ORDER BY [Index];", card.TabName);
            SQLiteDataReader reader = SqliteHelper.PoolDict[card.OwnerName].ExecuteQuery(sql);
            while (reader.Read())
            {
                Card.Tip tip = new Card.Tip()
                {
                    Index = Convert.ToInt32(reader["Index"]),
                    Uid = reader["Uid"].ToString(),
                    Title = reader["Title"] == DBNull.Value ? null : reader["Title"].ToString()
                };
                if (tip.Title == "别称" ||
                    tip.Title == "所属" ||
                    tip.Title == "物品")
                {
                    tip.IsEnabled = false;
                }
                line.Tips.Add(tip);
            }
            reader.Close();
            return card;
        }

        private static void FillInNodes(string pid, Node rootNode)
        {
            if (Gval.CurrentBook.Name == null && rootNode.OwnerName != "index")
            {
                return;
            }
            string sql = string.Format("SELECT * FROM {0} WHERE Pid='{1}' ORDER BY [Index];", rootNode.TabName, pid);
            SQLiteDataReader reader = SqliteHelper.PoolDict[rootNode.OwnerName].ExecuteQuery(sql);
            while (reader.Read())
            {
                Node node = new Node
                {
                    Index = Convert.ToInt32(reader["Index"]),
                    Uid = reader["Uid"].ToString(),
                    Pid = reader["Pid"].ToString(),
                    Title = reader["Title"] == DBNull.Value ? null : reader["Title"].ToString(),
                    IsDir = (bool)reader["IsDir"],
                    Text = reader["Text"] == DBNull.Value ? null : reader["Text"].ToString(),
                    Summary = reader["Summary"] == DBNull.Value ? null : reader["Summary"].ToString(),
                    WordsCount = reader["WordsCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["WordsCount"]),
                    IsExpanded = (bool)reader["IsExpanded"],
                    IsChecked = (bool)reader["IsChecked"],
                    IsDel = (bool)reader["IsDel"],
                    TabName = rootNode.TabName,
                };
                node = FillExtra(node);
                rootNode.ChildNodes.Add(node);
                FillInNodes(node.Uid, node);
            }

            reader.Close();
        }

        private static Node FillExtra(Node node)
        {
            node.Extra = new Summary()
            {
                Node = node,
            };
            if (node.TabName == Material.MaterialTabName.题材.ToString())
            {
                Topic topic = new Topic();
                if (JsonHelper.JsonToObject<Topic>(node.Summary) != null)
                {
                    topic = JsonHelper.JsonToObject<Topic>(node.Summary);
                }
                node.Extra = topic;
            }
            else
            {
                if (JsonHelper.JsonToObject<Summary>(node.Summary) != null)
                {
                    (node.Extra as Summary).Json = JsonHelper.JsonToObject<Summary.JsonData>(node.Summary);
                }
                (node.Extra as Summary).Time = (node.Extra as Summary).Json.Time;
                (node.Extra as Summary).Place = (node.Extra as Summary).Json.Place;
                foreach (string uid in (node.Extra as Summary).Json.Roles.ToList())
                {
                    foreach (Card card in Gval.CurrentBook.CardRole.ChildNodes)
                    {
                        if (uid == card.Uid)
                        {
                            (node.Extra as Summary).Roles.Add(card);
                        }
                    }
                }
                foreach (string uid in (node.Extra as Summary).Json.Origin.ToList())
                {
                    foreach (Node cNode in Gval.CurrentBook.GetSecenNodes())
                    {
                        if (uid == cNode.Uid)
                        {
                            (node.Extra as Summary).Origin.Add(cNode);
                        }
                    }
                }
                foreach (string uid in (node.Extra as Summary).Json.Result.ToList())
                {
                    foreach (Node cNode in Gval.CurrentBook.GetSecenNodes())
                    {
                        if (uid == cNode.Uid)
                        {
                            (node.Extra as Summary).Result.Add(cNode);
                        }
                    }
                }
            }
            return node;
        }

        private static Node FillInNodeContent(Node node)
        {
            if (Gval.CurrentBook.Name == null && node.OwnerName != "index")
            {
                return node;
            }
            string sql = string.Format("SELECT * FROM {0} WHERE Uid='{1}';", node.TabName, node.Uid);
            SQLiteDataReader reader = SqliteHelper.PoolDict[node.OwnerName].ExecuteQuery(sql);
            while (reader.Read())
            {
                node.Index = Convert.ToInt32(reader["Index"]);
                node.Uid = reader["Uid"].ToString();
                node.Pid = reader["Pid"].ToString();
                node.Title = reader["Title"] == DBNull.Value ? null : reader["Title"].ToString();
                node.IsDir = (bool)reader["IsDir"];
                node.Text = reader["Text"] == DBNull.Value ? null : reader["Text"].ToString();
                node.Summary = reader["Summary"] == DBNull.Value ? null : reader["Summary"].ToString();
                node.WordsCount = reader["WordsCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["WordsCount"]);
                node.IsExpanded = (bool)reader["IsExpanded"];
                node.IsChecked = (bool)reader["IsChecked"];
                node.IsDel = (bool)reader["IsDel"];
                node = FillExtra(node);
            }
            reader.Close();
            return node;
        }


        public static Node LoadNodeContent(Node node)
        {
            Gval.FlagLoadingCompleted = false;
            SqliteHelper.PoolOperate.Add(node.OwnerName);
            node = DataIn.FillInNodeContent(node);
            Gval.FlagLoadingCompleted = true;
            return node;
        }


        public static void FillInNodes(Node rootNode)
        {
            Gval.FlagLoadingCompleted = false;
            SqliteHelper.PoolOperate.Add(rootNode.OwnerName);
            DataIn.FillInNodes(null, rootNode);
            Gval.FlagLoadingCompleted = true;
        }

        private static void FillInCards(string pid, Card rootCard)
        {
            if (Gval.CurrentBook.Name == null && rootCard.OwnerName != "index")
            {
                return;
            }
            string sql = string.Format("SELECT * FROM {0} ORDER BY [Index];", rootCard.TabName, pid);
            SQLiteDataReader reader = SqliteHelper.PoolDict[rootCard.OwnerName].ExecuteQuery(sql);
            while (reader.Read())
            {
                Card card = new Card
                {
                    Index = Convert.ToInt32(reader["Index"]),
                    Uid = reader["Uid"].ToString(),
                    Title = reader["Title"] == DBNull.Value ? null : reader["Title"].ToString(),
                    Summary = reader["Summary"] == DBNull.Value ? null : reader["Summary"].ToString(),
                    Weight = reader["Weight"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Weight"]),
                    BornYear = string.IsNullOrWhiteSpace(reader["BornYear"].ToString()) == true ? null : reader["BornYear"].ToString(),
                    IsChecked = (bool)reader["IsChecked"],
                    IsDel = (bool)reader["IsDel"]
                };
                rootCard.ChildNodes.Add(card);
                if (card.Lines.Contains(card.NickNames) == false)
                {
                    card.Lines.Add(card.NickNames);
                }
                sql = string.Format("SELECT * FROM 卡片 WHERE Pid='{0}' AND Tid=(select Uid from 卡设计 where Title='{1}' AND TabName='{2}') AND TabName='{2}' ORDER BY [Index];", card.Uid, "别称", card.TabName);
                SQLiteDataReader readerTip = SqliteHelper.PoolDict[card.OwnerName].ExecuteQuery(sql);
                while (readerTip.Read())
                {
                    Card.Tip tip = new Card.Tip
                    {
                        Index = Convert.ToInt32(readerTip["Index"]),
                        Tid = readerTip["Tid"] == DBNull.Value ? null : readerTip["Tid"].ToString(),
                        Title = readerTip["Title"] == DBNull.Value ? null : readerTip["Title"].ToString(),
                        TabName = card.TabName
                    };
                    card.NickNames.Tips.Add(tip);
                }
                readerTip.Close();
            }
            reader.Close();
        }


        public static void FillInCards(Card rootCard)
        {
            Gval.FlagLoadingCompleted = false;
            SqliteHelper.PoolOperate.Add(rootCard.OwnerName);
            DataIn.FillInCards(null, rootCard);
            Gval.FlagLoadingCompleted = true;
        }

        private static Card FillInCardContent(Card card)
        {
            card.Lines.Clear();
            if (Gval.CurrentBook.Name == null && card.OwnerName != "index")
            {
                return card;
            }
            string sqlMain = string.Format("SELECT * FROM {0} WHERE Uid='{1}';", card.TabName, card.Uid);
            SQLiteDataReader readerMain = SqliteHelper.PoolDict[card.OwnerName].ExecuteQuery(sqlMain);
            while (readerMain.Read())
            {
                card.Index = Convert.ToInt32(readerMain["Index"]);
                card.Uid = readerMain["Uid"].ToString();
                card.Title = readerMain["Title"] == DBNull.Value ? null : readerMain["Title"].ToString();
                card.Summary = readerMain["Summary"] == DBNull.Value ? null : readerMain["Summary"].ToString();
                card.Weight = readerMain["Weight"] == DBNull.Value ? 0 : Convert.ToInt32(readerMain["Weight"]);
                card.BornYear = string.IsNullOrWhiteSpace(readerMain["BornYear"].ToString()) == true ? null : readerMain["BornYear"].ToString();
                card.IsChecked = (bool)readerMain["IsChecked"];
                card.IsDel = (bool)readerMain["IsDel"];
            }
            readerMain.Close();
            string sql = string.Format("SELECT * FROM 卡设计 WHERE TabName='{0}' ORDER BY [Index];", card.TabName);
            SQLiteDataReader readerSet = SqliteHelper.PoolDict[card.OwnerName].ExecuteQuery(sql);
            while (readerSet.Read())
            {
                Card.Line line = new Card.Line
                {
                    LineTitle = readerSet["Title"] == DBNull.Value ? null : readerSet["Title"].ToString(),
                };
                if (card.Lines.Contains(line) == false)
                {
                    card.Lines.Add(line);
                }
                sql = string.Format("SELECT * FROM 卡片 WHERE Pid='{0}' AND Tid=(select Uid from 卡设计 where Title='{1}' AND TabName='{2}') AND TabName='{2}' ORDER BY [Index];", card.Uid, line.LineTitle, card.TabName);
                SQLiteDataReader readerTip = SqliteHelper.PoolDict[card.OwnerName].ExecuteQuery(sql);
                while (readerTip.Read())
                {
                    Card.Tip tip = new Card.Tip
                    {
                        Index = Convert.ToInt32(readerTip["Index"]),
                        Uid = readerTip["Uid"] == DBNull.Value ? null : readerTip["Uid"].ToString(),
                        Pid = readerTip["Pid"] == DBNull.Value ? null : readerTip["Pid"].ToString(),
                        Tid = readerTip["Tid"] == DBNull.Value ? null : readerTip["Tid"].ToString(),
                        Title = readerTip["Title"] == DBNull.Value ? null : readerTip["Title"].ToString(),
                        TabName = card.TabName
                    };
                    line.Tips.Add(tip);
                }
                readerTip.Close();
            }
            readerSet.Close();
            return card;
        }


        public static Card LoadCardContent(Card card)
        {
            Gval.FlagLoadingCompleted = false;
            SqliteHelper.PoolOperate.Add(card.OwnerName);
            card = DataIn.FillInCardContent(card);
            Gval.FlagLoadingCompleted = true;
            return card;
        }

        /// <summary>
        /// 返回搜索结果列表（Uid列表）
        /// </summary>
        /// <param name="rootCard"></param>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static List<string> GetSearchResults(Card rootCard, string inputStr)
        {
            List<string> rutList = new List<string>();
            string[] sArray = inputStr.Split(new char[3] { '：', ':', ' ' });
            if (sArray.Length == 1)
            {
                string[] sArrayYear = inputStr.Split(new char[3] { '-', '~', '|' });
                string sql = string.Empty;
                string sql2 = string.Empty;
                if (sArrayYear.Length == 1)
                {
                    if (int.TryParse(inputStr, out int n))
                    {
                        sql = string.Format("SELECT * FROM {0} WHERE BornYear BETWEEN {1} AND {2};", rootCard.TabName, Convert.ToInt32((n * 0.8)).ToString().Replace("'", "''"), Convert.ToInt32((n * 1.2)).ToString().Replace("'", "''"));
                    }
                    else
                    {
                        sql = string.Format("SELECT * FROM {0} WHERE Title LIKE '%{1}%' OR Summary LIKE '%{1}%' OR BornYear LIKE '%{1}%';", rootCard.TabName, inputStr);
                        sql2 = string.Format("SELECT* FROM 卡片 WHERE Title LIKE '%{0}%';", inputStr); //卡片表的内容查询语句
                    }
                }
                if (sArrayYear.Length == 2)
                {
                    if (string.IsNullOrWhiteSpace(sArrayYear[0]) || string.IsNullOrWhiteSpace(sArrayYear[1]))
                    {
                        return rutList;
                    }
                    sql = string.Format("SELECT * FROM {0} WHERE BornYear BETWEEN {1} AND {2};", rootCard.TabName, sArrayYear[0].Replace("'", "''"), sArrayYear[1].Replace("'", "''"));
                }
                SQLiteDataReader reader = SqliteHelper.PoolDict[rootCard.OwnerName].ExecuteQuery(sql);
                while (reader.Read())
                {
                    Card card = new Card
                    {
                        Index = Convert.ToInt32(reader["Index"]),
                        Uid = reader["Uid"].ToString(),
                        Title = reader["Title"] == DBNull.Value ? null : reader["Title"].ToString(),
                        Summary = reader["Summary"] == DBNull.Value ? null : reader["Summary"].ToString(),
                        Weight = reader["Weight"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Weight"]),
                        BornYear = string.IsNullOrWhiteSpace(reader["BornYear"].ToString()) == true ? null : reader["BornYear"].ToString(),
                        IsChecked = (bool)reader["IsChecked"],
                        IsDel = (bool)reader["IsDel"],
                        IsShowCard = true,
                    };
                    if (rutList.Contains(card.Uid) == false)
                    {
                        rutList.Add(card.Uid);
                    }
                }
                reader.Close();
                SQLiteDataReader reader2 = SqliteHelper.PoolDict[rootCard.OwnerName].ExecuteQuery(sql2);
                while (reader2.Read())
                {
                    string Uid = reader2["Pid"].ToString(); //这里的Pid才是对应信息卡的Uid
                    foreach (Card card in rootCard.ChildNodes)
                    {
                        if (Uid == card.Uid)
                        {
                            card.IsShowCard = true;
                            if (rutList.Contains(card.Uid) == false)
                            {
                                rutList.Add(card.Uid);
                            }
                        }
                    };
                }
                reader2.Close();
            }
            if (sArray.Length == 2)
            {
                string sql = string.Format("SELECT * FROM 卡片 WHERE Title LIKE '%{0}%' AND Tid=(select Uid from 卡设计 where Title='{1}' AND TabName='{2}') AND TabName='{2}' ORDER BY [Index];", sArray[1].Replace("'", "''"), sArray[0].Replace("'", "''"), rootCard.TabName);
                SQLiteDataReader readerTip = SqliteHelper.PoolDict[rootCard.OwnerName].ExecuteQuery(sql);
                while (readerTip.Read())
                {
                    string cardUid = readerTip["Pid"].ToString();
                    string sql2 = string.Format("SELECT * FROM {0} WHERE Uid='{1}';", rootCard.TabName, cardUid);
                    SQLiteDataReader reader = SqliteHelper.PoolDict[rootCard.OwnerName].ExecuteQuery(sql2);
                    while (reader.Read())
                    {
                        Card card = new Card
                        {
                            Index = Convert.ToInt32(reader["Index"]),
                            Uid = reader["Uid"].ToString(),
                            Title = reader["Title"] == DBNull.Value ? null : reader["Title"].ToString(),
                            Summary = reader["Summary"] == DBNull.Value ? null : reader["Summary"].ToString(),
                            Weight = reader["Weight"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Weight"]),
                            BornYear = string.IsNullOrWhiteSpace(reader["BornYear"].ToString()) == true ? null : reader["BornYear"].ToString(),
                            IsChecked = (bool)reader["IsChecked"],
                            IsDel = (bool)reader["IsDel"],
                            IsShowCard = true,
                        };
                        if (rutList.Contains(card.Uid) == false)
                        {
                            rutList.Add(card.Uid);
                        }
                    }
                    reader.Close();
                }
                readerTip.Close();
            }
            return rutList;
        }
    }
}
