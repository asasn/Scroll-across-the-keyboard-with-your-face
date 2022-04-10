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
            HelperTable.TryToBuildBookTables(bookName);
            string sql = string.Format("INSERT INTO 书库 (Uid, [Index], Name) VALUES ('{0}', '{1}', '{2}');", newBook.Uid, newBook.Index, newBook.Name.Replace("'", "''"));
            CSqlitePlus.PoolDict["index"].ExecuteNonQuery(sql);
            Gval.BooksBank.Add(newBook);
            return newBook;
        }


        public static void MoveNodeToOtherTable(Node node, string oldTabName, string newTabName)
        {
            node.Pid = string.Empty;
            string sql = string.Empty;
            sql += string.Format("INSERT INTO {0} SELECT * FROM {1} WHERE Uid='{2}';", newTabName, oldTabName, node.Uid);
            sql += string.Format("DELETE FROM {0} WHERE Uid='{1}';", oldTabName, node.Uid);
            CSqlitePlus.PoolDict[node.OwnerName].ExecuteNonQuery(sql);

            //移动至新表之后再进行操作
            node.TabName = newTabName;            
        }

        public static void CreateNewNode(Node node)
        {
            string sql = string.Format("INSERT INTO {0} (Uid, [Index], Pid, Title, IsDir, Text, Summary, TabName, PointX, PointY, WordsCount, IsExpanded, IsChecked, IsDel) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}');", node.TabName, node.Uid, node.Index, node.Pid, node.Title.Replace("'", "''"), node.IsDir, node.Text.Replace("'", "''"), node.Summary.Replace("'", "''"), node.TabName, node.PointX, node.PointY, node.WordsCount, node.IsExpanded, node.IsChecked, node.IsDel);
            CSqlitePlus.PoolDict[node.OwnerName].ExecuteNonQuery(sql);
        }

        public static void UpdateNodeProperty(Node node, string fieldName, string value)
        {
            string sql = string.Format("UPDATE {0} set [{1}]='{2}' where Uid='{3}';", node.TabName, fieldName, value.Replace("'", "''"), node.Uid);
            CSqlitePlus.PoolDict[node.OwnerName].ExecuteNonQuery(sql);
        }
    }
}
