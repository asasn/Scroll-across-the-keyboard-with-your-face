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
        }

        public static class BookTreeToXml
        {
            /// <summary>
            /// 保存书目结构和关联的所有书籍结构
            /// </summary>
            /// <param name="tv"></param>
            public static void SaveAllBooks(TreeView tv)
            {
                //保存书目
                SaveBooks(tv);

                //保存书籍
                foreach (TreeViewItem bookItem in tv.Items)
                {
                    SaveBook(bookItem);
                }

            }

            /// <summary>
            /// 保存书目结构
            /// </summary>
            /// <param name="tv"></param>
            public static void SaveBooks(TreeView tv)
            {
                if (tv != null)
                {
                    string fullXmlName_main = Gval.Base.AppPath + "/books/index.xml";
                    XmlDocument doc = new XmlDocument();
                    XmlElement eleRoot = ItemToElement(doc, "books");

                    foreach (TreeViewItem bookItem in tv.Items)
                    {
                        XmlElement eleBook = ItemToElement(doc, eleRoot, bookItem, bookItem.Name);
                    }
                    doc.Save(fullXmlName_main);
                }

            }

            /// <summary>
            /// 保存当前书籍结构
            /// </summary>
            /// <param name="tv">目录树控件</param>
            /// <param name="bookItem">指向的书籍节点</param>
            public static void SaveBook(TreeViewItem bookItem)
            {
                if (bookItem != null && bookItem.Name == "book")
                {
                    //获取当前书籍对应的完整xml文件名
                    string fullXmlName_book = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString() + "/index.xml";

                    XmlDocument doc = new XmlDocument();
                    XmlElement eleRoot = ItemToElement(doc, bookItem.Name);
                    DoSaveToXml(bookItem, eleRoot, doc);
                    doc.Save(fullXmlName_book);
                    Console.WriteLine("保存至：" + fullXmlName_book);
                }
                else
                {
                    MessageBox.Show("参数可能存在错误！", "提醒");
                }
            }
        }

        public static class NoteTreeToXml
        {
            public static void SaveNotes(TreeView tv, TreeViewItem bookItem, string XmlName)
            {
                if (tv != null)
                {
                    //获取当前notes对应的完整xml文件名
                    string fullXmlName_notes = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString() + "/" + XmlName;

                    XmlDocument doc = new XmlDocument();
                    XmlElement eleRoot = ItemToElement(doc, "notes");

                    foreach (TreeViewItem note in tv.Items)
                    {
                        XmlElement eleBook = ItemToElement(doc, eleRoot, note, bookItem.Name);
                    }
                    doc.Save(fullXmlName_notes);
                }
            }
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
            eleParent.AppendChild(ele);
            return ele;
        }

        /// <summary>
        /// 建立xml文档的根节点
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="eleName"></param>
        static XmlElement ItemToElement(XmlDocument doc, string eleName)
        {
            XmlElement ele = doc.CreateElement(eleName);
            doc.AppendChild(ele);
            return ele;
        }



        //递归保存
        private static void DoSaveToXml(TreeViewItem curItem, XmlElement parentXmlElement, XmlDocument doc)
        {
            foreach (TreeViewItem childitem in curItem.Items)
            {
                XmlElement ele = ItemToElement(doc, parentXmlElement, childitem, childitem.Name);
                DoSaveToXml(childitem, ele, doc);
            }
        }
    }
}
