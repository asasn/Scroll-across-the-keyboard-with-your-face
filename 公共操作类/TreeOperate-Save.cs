using System;
using System.Collections.Generic;
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
            public static void SaveTree(TreeView tv)
            {
                string tableName = Gval.Current.curBookName + "_Tree";
                string sql = string.Empty;
                sql += string.Format("delete from {0};", tableName); //清空数据
                sql += string.Format("update sqlite_sequence SET seq = 0 where name = '{0}';", tableName);//自增长ID为0
                sql += string.Format("CREATE TABLE IF NOT EXISTS {0}(id CHAR PRIMARY KEY, 级别 INTEGER,索引 INTEGER,父id CHAR, 名称 CHAR);", tableName);
                SqliteOperate.ExecuteNonQuery(sql);
                Gsql = string.Empty;
                ReTraversal(tv);
                SqliteOperate.ExecuteNonQuery(Gsql);

            }

            static void ReTraversal(TreeView tv)
            {
                string tableName = Gval.Current.curBookName + "_Tree";
                foreach (TreeViewItem item in tv.Items)
                {
                    item.Uid = TreeOperate.GetItemIndex(item);
                    Gsql += string.Format("insert or ignore into {0} (id, 级别, 索引, 父id, 名称) values ('{1}', {2}, {3}, '{4}', '{5}');", tableName, item.Uid, GetLevel(item), tv.Items.IndexOf(item), 0, item.Header.ToString());
                    //SqliteOperate.ExecuteNonQuery(sql);             
                    if (item.HasItems)
                    {
                        ReTraversal(item);
                    }
                }

            }

            static void ReTraversal(TreeViewItem parentItem)
            {
                string tableName = Gval.Current.curBookName + "_Tree";
                foreach (TreeViewItem item in parentItem.Items)
                {
                    item.Uid = TreeOperate.GetItemIndex(item);
                    Gsql += string.Format("insert or ignore into {0} (id, 级别, 索引, 父id, 名称) values ('{1}', {2}, {3}, '{4}', '{5}');", tableName, item.Uid, GetLevel(item), parentItem.Items.IndexOf(item), parentItem.Uid, item.Header.ToString());
                    //SqliteOperate.ExecuteNonQuery(sql);                                     
                    if (item.HasItems)
                    {
                        ReTraversal(item);
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
