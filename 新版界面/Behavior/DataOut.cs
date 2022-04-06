using RootNS.Brick;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Behavior
{
    class DataOut
    {
        public static Book CreateNewBook(string bookName)
        {
            Book newBook = new Book();
            newBook.Uid = Gval.NewGuid();
            newBook.Name = bookName;
            TableOperate.TryToBuildBookTables(bookName);
            string sql = string.Format("INSERT INTO 书库 (Uid, [Index], Name) VALUES ('{0}', '{1}', '{2}');", newBook.Uid, newBook.Index, newBook.Name.Replace("'", "''"));
            CSqlitePlus.PoolDict["index"].ExecuteNonQuery(sql);
            Gval.BooksBank.Add(newBook);
            return newBook;
        }


        public static void CreateNewNode(Node node)
        {
            string sql = string.Format("INSERT INTO {0} (Uid, [Index], Pid, Title, TabName) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}');", Gval.TableName, node.Uid, node.Index, node.Pid, node.Title.Replace("'", "''"), node.TabName);
            CSqlitePlus.PoolDict[Gval.WorkSpace].ExecuteNonQuery(sql);
        }
    }
}
