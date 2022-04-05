using RootNS.Brick;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Behavior
{
    class DataExport
    {
        public static Book CreateNewBook(string bookName)
        {
            Book newBook = new Book();
            newBook.Uid = Gval.NewGuid();
            newBook.Name = bookName;
            TableOperate.TryToBuildBookTables(bookName);
            string sql = string.Format("INSERT INTO 书库 (Uid, Index, Name) VALUES ('{0}', '{1}', '{2}');", newBook.Uid, newBook.Index, newBook.Name.Replace("'", "''"));
            CSqlitePlus.PoolDict[bookName].ExecuteNonQuery(sql);
            Gval.BooksBank.Add(newBook);
            return newBook;
        }
    }
}
