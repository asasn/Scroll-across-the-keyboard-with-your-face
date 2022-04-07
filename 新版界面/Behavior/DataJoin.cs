using RootNS.Brick;
using RootNS.Model;
using System;
using System.Collections.Generic;
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
        public static void ReadyForBegin()
        {
            string sql = string.Format("SELECT * FROM 书库;");
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
                    DataJoin.InitRootNodes(book);
                }
                Gval.BooksBank.Add(book);
            }
            reader.Close();
        }

        //public static void LoadMaterialContent()
        //{
        //    CSqlitePlus.PoolOperate.Add(Gval.MaterialBook.Name);
        //    Gval.MaterialBook.LoadForMaterialPart();
        //    Gval.MaterialBook.LoadForCards();
        //}

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
            DataJoin.InitRootNodes(book);
            book.LoadBookChapters();
            book.LoadBookNotes();
            book.LoadForCards();
        }

        public static void InitRootNodes(Book book)
        {
            Node[] rootNodes = { book.BoxDraft, book.BoxTemp, book.BoxPublished, book.NoteMemorabilia, book.NoteStory, book.NoteScenes, book.NoteClues, book.NoteTemplate, book.CardRole, book.CardOther, book.CardWorld, book.MapPoints };
            foreach (Node node in rootNodes)
            {
                node.OwnerName = book.Name;
            }
        }


        public static void FillInPart(string pid, Node rootNode)
        {
            if (Gval.CurrentBook.Name == null && rootNode.OwnerName != "index")
            {
                return;
            }
            string sql = string.Format("SELECT * FROM {0} WHERE TabName='{1}' AND Pid='{2}';", rootNode.TabName, rootNode.TabName, pid);
            SQLiteDataReader reader = CSqlitePlus.PoolDict[rootNode.OwnerName].ExecuteQuery(sql);
            while (reader.Read())
            {
                Node node = new Node
                {
                    Uid = reader["Uid"].ToString(),
                    Index = Convert.ToInt32(reader["Index"]),
                    Pid = reader["Pid"].ToString(),
                    Title = reader["Title"] == DBNull.Value ? null : reader["Title"].ToString(),
                    IsDir = (bool)reader["IsDir"],
                    Text = reader["Text"] == DBNull.Value ? null : reader["Text"].ToString(),
                    Summary = reader["Summary"] == DBNull.Value ? null : reader["Summary"].ToString(),
                    TabName = reader["TabName"].ToString(),
                    PointX = reader["PointX"] == DBNull.Value ? double.NaN : Convert.ToDouble(reader["PointX"]),
                    PointY = reader["PointY"] == DBNull.Value ? double.NaN : Convert.ToDouble(reader["PointY"]),
                    WordsCount = reader["WordsCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["WordsCount"]),
                    IsExpanded = (bool)reader["IsExpanded"],
                    IsChecked = (bool)reader["IsChecked"],
                    IsDel = (bool)reader["IsDel"]
                };
                rootNode.ChildNodes.Add(node);
                FillInPart(node.Uid, node);
            }
            reader.Close();
        }



    }
}
