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
    class DataEntry
    {
        /// <summary>
        /// 窗口打开后，开始用户操作前的载入数据流程
        /// </summary>
        public static void ReadyForBegin()
        {
            Gval.BooksBank.Clear();
            CFileOperate.CreateFolder(Gval.Path.Books);
            CSqlitePlus.PoolOperate.Add("index");
            TableOperate.TryToBuildIndexTables();
            Gval.CurrentBook.Uid = CSettingsOperate.Get("index", "CurBookUid");
            string sql = string.Format("SELECT * FROM 书库;", Gval.CurrentBook.Uid);
            SQLiteDataReader reader = CSqlitePlus.PoolDict["index"].ExecuteQuery(sql);
            while (reader.Read())
            {
                Book book = new Book();
                book.Uid = reader["Uid"].ToString();
                book.Index = Convert.ToInt32(reader["Index"]);
                book.Name = reader["Name"].ToString();
                book.Summary = reader["Summary"].ToString();
                book.Price = Convert.ToDouble(reader["Price"]);
                book.CurrentYear = Convert.ToInt64(reader["CurrentYear"]);
                book.IsDel = (bool)reader["IsDel"];
                if (Gval.CurrentBook.Uid != null && Gval.CurrentBook.Uid == book.Uid)
                {
                    Gval.CurrentBook = book;
                }
                Gval.BooksBank.Add(book);
            }
            reader.Close();
            LoadMaterialContent(Gval.MaterialBook);
            LoadCurrentBookContent(Gval.CurrentBook);
        }

        private static void LoadMaterialContent(Material materialBook)
        {
            Gval.MaterialBook = materialBook;
            materialBook.Name = "index";
            CSqlitePlus.PoolOperate.Add(materialBook.Name);
            FillInPart(materialBook.Name, "目录", Material.MaterialTabName.范文.ToString(), null, materialBook.BoxExample);
            FillInPart(materialBook.Name, "目录", Material.MaterialTabName.资料.ToString(), null, materialBook.BoxMaterial);
            FillInPart(materialBook.Name, "记事", Material.MaterialTabName.主题.ToString(), null, materialBook.NoteTheme);
            FillInPart(materialBook.Name, "记事", Material.MaterialTabName.灵感.ToString(), null, materialBook.NoteInspiration);
            FillInPart(materialBook.Name, "卡片", Material.CardTabName.角色.ToString(), null, materialBook.CardRole);
            FillInPart(materialBook.Name, "卡片", Material.CardTabName.其他.ToString(), null, materialBook.CardOther);
            FillInPart(materialBook.Name, "卡片", Material.CardTabName.世界.ToString(), null, materialBook.CardWorld);
            FillInPart(materialBook.Name, "地图", null, null, materialBook.MapPoints);
        }

        public static void LoadCurrentBookContent(Book book)
        {
            Gval.CurrentBook = book;
            CSqlitePlus.PoolOperate.Add(book.Name);
            FillInPart(book.Name, "目录", Book.ChapterTabName.草稿箱.ToString(), null, book.BoxDraft);
            FillInPart(book.Name, "目录", Book.ChapterTabName.暂存箱.ToString(), null, book.BoxTemp);
            FillInPart(book.Name, "目录", Book.ChapterTabName.已发布.ToString(), null, book.BoxPublished);
            FillInPart(book.Name, "记事", Book.NoteTabName.大事记.ToString(), null, book.NoteMemorabilia);
            FillInPart(book.Name, "记事", Book.NoteTabName.故事.ToString(), null, book.NoteStory);
            FillInPart(book.Name, "记事", Book.NoteTabName.场景.ToString(), null, book.NoteScenes);
            FillInPart(book.Name, "记事", Book.NoteTabName.线索.ToString(), null, book.NoteClues);
            FillInPart(book.Name, "记事", Book.NoteTabName.文例.ToString(), null, book.NoteTemplate);
            FillInPart(book.Name, "卡片", Book.CardTabName.角色.ToString(), null, book.CardRole);
            FillInPart(book.Name, "卡片", Book.CardTabName.其他.ToString(), null, book.CardOther);
            FillInPart(book.Name, "卡片", Book.CardTabName.世界.ToString(), null, book.CardWorld);
            FillInPart(book.Name, "地图", null, null, book.MapPoints);
        }


        private static void FillInPart(string bookName, string tableName, string tabName, string pid, Node rootNode)
        {
            string sql = string.Format("SELECT * FROM {0} WHERE TabName='{1}' AND Pid='{2}';", tableName, tabName, pid);
            SQLiteDataReader reader = CSqlitePlus.PoolDict[bookName].ExecuteQuery(sql);
            while (reader.Read())
            {
                Node node = new Node();
                node.Uid = reader["Uid"].ToString();
                node.Index = Convert.ToInt32(reader["Index"]);
                node.Pid = reader["Pid"].ToString();
                node.Title = reader["Title"].ToString();
                node.IsDir = (bool)reader["IsDir"];
                node.Text = reader["Text"].ToString();
                node.Summary = reader["Summary"].ToString();
                node.TabName = reader["TabName"].ToString();
                node.PointX = Convert.ToDouble(reader["PointX"]);
                node.PointY = Convert.ToDouble(reader["PointY"]);
                node.WordsCount = Convert.ToInt32(reader["WordsCount"]);
                node.IsExpanded = (bool)reader["IsExpanded"];
                node.IsChecked = (bool)reader["IsChecked"];
                node.IsDel = (bool)reader["IsDel"];
                rootNode.ChildNodes.Add(node);
                FillInPart(bookName, tableName, tabName, node.Uid, node);
            }
            reader.Close();
        }



    }
}
