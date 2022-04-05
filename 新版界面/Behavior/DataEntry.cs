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
            TableOperate.TryToBuildIndexTables();
            Gval.CurrentBook.Uid = CSettingsOperate.Get("index", "CurBookUid");
            string sql = string.Format("SELECT * FROM 书库;", Gval.CurrentBook.Uid);
            SQLiteDataReader reader = CSqlitePlus.PoolDict["index"].ExecuteQuery(sql);
            while (reader.Read())
            {
                Book book = new Book();
                book.Index = Convert.ToInt32(reader["Index"]);
                book.Name = reader["Name"].ToString();
                book.Summary = reader["Summary"].ToString();
                book.Price = Convert.ToDouble(reader["Price"]);
                book.CurrentYear = Convert.ToInt64(reader["CurrentYear"]);
                if (Gval.CurrentBook.Uid == book.Uid)
                {
                    Gval.CurrentBook = book;
                }
                Gval.BooksBank.Add(book);
            }
            reader.Close();
        }




    }
}
