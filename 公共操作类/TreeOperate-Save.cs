using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace 脸滚键盘
{
    static partial class TreeOperate
    {
        public static partial class Save
        {
            static string Gsql = string.Empty;
            public static void BySql(TreeView tv, string tableName)
            {
                if (tableName == "material")
                {
                    SqliteOperate.NewConnection(Gval.Base.AppPath + "/" + "material", "material.db");
                }

                tableName = "Tree_" + tableName;
                string sql = string.Format("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = '{0}';", tableName);
                SQLiteDataReader reader = SqliteOperate.ExecuteQuery(sql);
                sql = string.Empty;
                if (reader.Read())
                {
                    if (reader.GetInt32(0) > 0)
                    {
                        sql += string.Format("delete from '{0}';", tableName); //清空数据
                        //sql += string.Format("update sqlite_sequence SET seq = 0 where name = '{0}';", tableName);//自增长ID为0
                    }
                }
                reader.Close();
                sql += string.Format("CREATE TABLE IF NOT EXISTS {0}(id CHAR PRIMARY KEY, pid CHAR, Header CHAR, Name CHAR, Uid CHAR, Tag CHAR, DataContext CHAR, IsExpanded BOOLEAN);", tableName);
                SqliteOperate.ExecuteNonQuery(sql);
                Gsql = string.Empty;
                ReTraversal(tv, tableName);
                SqliteOperate.ExecuteNonQuery(Gsql);

                //恢复默认连接
                SqliteOperate.NewConnection();
            }

            static void ReTraversal(TreeView tv, string tableName)
            {
                foreach (TreeViewItem item in tv.Items)
                {
                    //借用uid过桥，以便获取pid，所以在借用之前先转移数据
                    string itemUid = item.Uid;
                    item.Uid = TreeOperate.GetItemIndex(item);
                    item.Tag = item.Tag ?? "";
                    item.DataContext = item.DataContext ?? "";
                    item.IsExpanded = item.IsExpanded == true;
                    Gsql += string.Format("insert or ignore into {0} (id, pid, Header, Name, Uid, Tag, DataContext, IsExpanded ) values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8});", tableName, item.Uid, null, item.Header.ToString(), item.Name, itemUid, item.Tag, item.DataContext, item.IsExpanded);
                    //SqliteOperate.ExecuteNonQuery(sql);             
                    if (item.HasItems)
                    {
                        ReTraversal(item, tableName);
                    }
                }

            }

            static void ReTraversal(TreeViewItem parentItem, string tableName)
            {
                foreach (TreeViewItem item in parentItem.Items)
                {
                    //借用uid过桥，以便获取pid，所以在借用之前先转移数据
                    string itemUid = item.Uid;
                    item.Uid = TreeOperate.GetItemIndex(item);
                    item.Tag = item.Tag ?? "";
                    item.DataContext = item.DataContext ?? "";
                    item.IsExpanded = item.IsExpanded == true;
                    Gsql += string.Format("insert or ignore into {0} (id, pid, Header, Name, Uid, Tag, DataContext, IsExpanded) values ('{1}', '{2}', '{3}','{4}', '{5}', '{6}', '{7}', {8});", tableName, item.Uid, parentItem.Uid, item.Header.ToString(), item.Name, itemUid, item.Tag, item.DataContext, item.IsExpanded);
                    //SqliteOperate.ExecuteNonQuery(sql);                                     
                    if (item.HasItems)
                    {
                        ReTraversal(item, tableName);
                    }
                }

            }

            public static class BookTree
            {
                /// <summary>
                /// 保存书目结构和关联的所有书籍结构
                /// </summary>
                /// <param name="tv"></param>
                public static void SaveAll(TreeView tv)
                {
                    //保存书目
                    //SaveRoot(tv);
                    //保存书籍
                    foreach (TreeViewItem bookItem in tv.Items)
                    {
                        //SaveCurBook(bookItem);
                    }
                }

                public static void BuildBookXml(string fullXmlName, string rootEleName)
                {
                    XmlDocument doc = new XmlDocument();
                    XmlElement ele = doc.CreateElement(rootEleName);
                    doc.AppendChild(ele);
                    doc.Save(fullXmlName);
                }

                /// <summary>
                /// 流程1：保存书目结构
                /// </summary>
                /// <param name="tv"></param>
                public static void AddToBooksXml(string itemTitle)
                {
                    string fullXmlName_main = Gval.Base.AppPath + "/books/index.xml";
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fullXmlName_main);
                    XmlElement eleBook = doc.CreateElement("dir");
                    eleBook.SetAttribute("title", itemTitle);
                    doc.DocumentElement.AppendChild(eleBook);
                    doc.Save(fullXmlName_main);
                }

                /// <summary>
                /// 流程2：保存当前书籍结构
                /// </summary>
                /// <param name="tv">目录树控件</param>
                /// <param name="bookItem">指向的书籍节点</param>
                public static void SaveCurBook(TreeView tv)
                {
                    if (tv != null)
                    {
                        //获取当前书籍对应的完整xml文件名
                        string fullXmlName_book = Gval.Current.curBookPath + "/index.xml";

                        XmlDocument doc = new XmlDocument();
                        XmlElement eleRoot = ItemToRootElement(doc, "root");
                        DoSaveToXml(tv, eleRoot, doc);
                        doc.Save(fullXmlName_book);
                        Console.WriteLine("保存至：" + fullXmlName_book);
                    }
                    else
                    {
                        MessageBox.Show("参数可能存在错误！", "提醒");
                    }
                }
            }

            public static void ToSingleXml(TreeView tv, string ucTag)
            {
                //获取当前笔记对应的完整xml文件名
                string fullXmlName;
                if (ucTag == "material")
                {
                    fullXmlName = Gval.Base.AppPath + "/" + ucTag + "/index.xml";
                }
                else
                {
                    fullXmlName = Gval.Current.curBookPath + "/" + ucTag + ".xml";
                }

                XmlDocument doc = new XmlDocument();
                XmlElement eleRoot = ItemToRootElement(doc, "root");
                DoSaveToXml(tv, eleRoot, doc);
                //foreach (TreeViewItem dirItem in tv.Items)
                //{
                //    XmlElement eleDir = ItemToElement(doc, eleRoot, dirItem, "dir");
                //    DoSaveToXml(dirItem, eleDir, doc);
                //}
                doc.Save(fullXmlName);
                Gval.ucEditor.SetRules();
            }

            /// <summary>
            /// 建立xml文档的非根节点
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="eleParent"></param>
            /// <param name="item"></param>
            /// <param name="itemName"></param>
            static XmlElement ItemToElement(XmlDocument doc, XmlElement eleParent, TreeViewItem item, string itemName)
            {
                XmlElement ele = doc.CreateElement(itemName);
                ele.SetAttribute("title", item.Header.ToString());
                if (false == string.IsNullOrEmpty(item.Uid))
                {
                    ele.SetAttribute("id", item.Uid.ToString());
                }
                eleParent.AppendChild(ele);
                return ele;
            }

            /// <summary>
            /// 建立xml文档的根节点
            /// </summary>
            /// <param name="doc"></param>
            /// <param name="eleName"></param>
            static XmlElement ItemToRootElement(XmlDocument doc, string eleName)
            {
                XmlElement ele = doc.CreateElement(eleName);
                doc.AppendChild(ele);
                return ele;
            }

            /// <summary>
            /// 递归保存
            /// </summary>
            /// <param name="curItem"></param>
            /// <param name="parentXmlElement"></param>
            /// <param name="doc"></param>
            static void DoSaveToXml(TreeView tv, XmlElement parentXmlElement, XmlDocument doc)
            {
                foreach (TreeViewItem childitem in tv.Items)
                {
                    XmlElement ele = ItemToElement(doc, parentXmlElement, childitem, childitem.Name);
                    DoSaveToXml(childitem, ele, doc);
                }
            }

            /// <summary>
            /// 递归保存
            /// </summary>
            /// <param name="curItem"></param>
            /// <param name="parentXmlElement"></param>
            /// <param name="doc"></param>
            static void DoSaveToXml(TreeViewItem curItem, XmlElement parentXmlElement, XmlDocument doc)
            {
                foreach (TreeViewItem childitem in curItem.Items)
                {
                    XmlElement ele = ItemToElement(doc, parentXmlElement, childitem, childitem.Name);
                    DoSaveToXml(childitem, ele, doc);
                }
            }


        }
    }
}
