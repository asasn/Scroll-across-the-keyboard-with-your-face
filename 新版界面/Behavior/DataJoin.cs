using RootNS.Brick;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Behavior
{
    class DataJoin
    {
        /// <summary>
        /// 窗口打开后，开始用户操作前的载入数据流程
        /// </summary>
        public static void ReadyForBaseInfo()
        {
            CFileOperate.CreateFolder(Gval.Path.Books);
            HelperTable.TryToBuildIndexDatabase();
            Gval.CurrentBook.Uid = CSettingsOperate.Get(Gval.MaterialBook.Name, "CurBookUid");
            LoadBooksBank();            
        }


        private static void LoadBooksBank()
        {
            string sql = string.Format("SELECT * FROM 书库 ORDER BY [Index];");
            CSqlitePlus.PoolOperate.Add("index");
            SQLiteDataReader reader = CSqlitePlus.PoolDict["index"].ExecuteQuery(sql);
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
                }
                Gval.BooksBank.Add(book);
            }
            reader.Close();
        }


        public static void LoadCurrentBookContent(Book book)
        {
            CSqlitePlus.PoolOperate.Add(Gval.CurrentBook.Name);
            string sql = string.Format("SELECT * FROM 书库 WHERE Uid='{0}';", book.Uid);
            SQLiteDataReader reader = CSqlitePlus.PoolDict["index"].ExecuteQuery(sql);
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
            book.LoadBookChapters();
            book.LoadBookNotes();
            book.LoadForCards(Gval.SelectedCardTab);
            Gval.FlagLoadingCompleted = true;

        }


        public static ObservableCollection<Card> CardDesginLoad(Card rootCard)
        {
            ObservableCollection<Card> lineTitles = new ObservableCollection<Card>();
            if (Gval.CurrentBook.Name == null && rootCard.OwnerName != "index")
            {
                return null;
            }
            string sql = string.Format("SELECT * FROM 卡设计 WHERE TabName='{0}' ORDER BY [Index];", rootCard.TabName);
            SQLiteDataReader reader = CSqlitePlus.PoolDict[rootCard.OwnerName].ExecuteQuery(sql);
            while (reader.Read())
            {
                Card card = new Card
                {
                    Index = Convert.ToInt32(reader["Index"]),
                    Uid = reader["Uid"].ToString(),
                    Title = reader["Title"] == DBNull.Value ? null : reader["Title"].ToString(),
                    TabName = rootCard.TabName,
                    OwnerName = rootCard.OwnerName,
                };
                lineTitles.Add(card);
            }
            reader.Close();
            return lineTitles;
        }

        private static void FillInNodes(string pid, Node rootNode)
        {
            if (Gval.CurrentBook.Name == null && rootNode.OwnerName != "index")
            {
                return;
            }
            string sql = string.Format("SELECT * FROM {0} WHERE Pid='{1}' ORDER BY [Index];", rootNode.TabName, pid);
            SQLiteDataReader reader = CSqlitePlus.PoolDict[rootNode.OwnerName].ExecuteQuery(sql);
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
                    IsDel = (bool)reader["IsDel"]
                };
                rootNode.ChildNodes.Add(node);
                FillInNodes(node.Uid, node);
            }
            reader.Close();
        }


        public static void FillInNodes(Node rootNode)
        {
            Gval.FlagLoadingCompleted = false;
            CSqlitePlus.PoolOperate.Add(rootNode.OwnerName);
            DataJoin.FillInNodes(null, rootNode);
            Gval.FlagLoadingCompleted = true;
        }

        private static void FillInCards(string pid, Card rootCard)
        {
            if (Gval.CurrentBook.Name == null && rootCard.OwnerName != "index")
            {
                return;
            }
            string sql = string.Format("SELECT * FROM {0} ORDER BY [Index];", rootCard.TabName, pid);
            SQLiteDataReader reader = CSqlitePlus.PoolDict[rootCard.OwnerName].ExecuteQuery(sql);
            while (reader.Read())
            {
                Card card = new Card
                {
                    Index = Convert.ToInt32(reader["Index"]),
                    Uid = reader["Uid"].ToString(),
                    Title = reader["Title"] == DBNull.Value ? null : reader["Title"].ToString(),
                    Summary = reader["Summary"] == DBNull.Value ? null : reader["Summary"].ToString(),
                    Weight = reader["Weight"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Weight"]),
                    BornYear = reader["BornYear"] == DBNull.Value ? 0 : Convert.ToInt64(reader["BornYear"]),
                    IsChecked = (bool)reader["IsChecked"],
                    IsDel = (bool)reader["IsDel"]
                };
                rootCard.ChildNodes.Add(card);
                if (card.Lines.Contains(card.NickNames) == false)
                {
                    card.Lines.Add(card.NickNames);
                }
                sql = string.Format("SELECT * FROM 卡片 WHERE Pid='{0}' AND Tid=(select Uid from 卡设计 where Title='{1}' AND TabName='{2}') AND TabName='{2}' ORDER BY [Index];", card.Uid, "别称", card.TabName);
                SQLiteDataReader readerTip = CSqlitePlus.PoolDict[card.OwnerName].ExecuteQuery(sql);
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
            CSqlitePlus.PoolOperate.Add(rootCard.OwnerName);
            DataJoin.FillInCards(null, rootCard);
            Gval.FlagLoadingCompleted = true;
        }

        private static void FillInCardContent(string pid, Card card)
        {
            card.Lines.Clear();
            if (Gval.CurrentBook.Name == null && card.OwnerName != "index")
            {
                return;
            }
            string sql = string.Format("SELECT * FROM 卡设计 WHERE TabName='{0}' ORDER BY [Index];", card.TabName);
            SQLiteDataReader readerSet = CSqlitePlus.PoolDict[card.OwnerName].ExecuteQuery(sql);
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
                SQLiteDataReader readerTip = CSqlitePlus.PoolDict[card.OwnerName].ExecuteQuery(sql);
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
        }


        public static void FillInCardContent(Card card)
        {
            Gval.FlagLoadingCompleted = false;
            CSqlitePlus.PoolOperate.Add(card.OwnerName);
            DataJoin.FillInCardContent(null, card);
            Gval.FlagLoadingCompleted = true;
        }
    }
}
