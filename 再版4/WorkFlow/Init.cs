using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Version4.Helper;
using Version4.Model;

namespace Version4.WorkFlow
{
    public class Init
    {
        /// <summary>
        /// 窗口打开后，开始用户操作前的载入数据流程
        /// </summary>
        public static void ReadyForBegin(Book currentBook)
        {
            IOTool.CreateFolder(AppMain.PathBooks);
            HelperTable.TryToBuildIndexDatabase();
            currentBook.Uid = Settings.Get("index", "CurBookUid");
        }

        public static void LoadBooksBank(Book currentBook, ObservableCollection<Book> booksBank)
        {
            string sql = string.Format("SELECT * FROM 书库 ORDER BY [Index];");
            SqlitePlus.PoolOperate.Add("index");
            SQLiteDataReader reader = SqlitePlus.PoolDict["index"].ExecuteQuery(sql);
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
                if (currentBook.Uid != null && currentBook.Uid == book.Uid)
                {
                    currentBook = book;
                }
                booksBank.Add(book);
            }
            reader.Close();
        }
    }
}
