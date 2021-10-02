using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace 脸滚键盘
{
    static partial class TreeOperate
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
            foreach (TreeViewItem rootItem in tv.Items)
            {
                SaveBook(rootItem);
            }

        }

        /// <summary>
        /// 建立xml文档的非根节点
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="eleParent"></param>
        /// <param name="item"></param>
        /// <param name="itemName"></param>
        static XmlElement TreeToNode(XmlDocument doc, XmlElement eleParent, TreeViewItem item, string itemName)
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
        static XmlElement TreeToNode(XmlDocument doc, string eleName)
        {
            XmlElement ele = doc.CreateElement(eleName);
            doc.AppendChild(ele);
            return ele;
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
                XmlElement eleRoot = TreeToNode(doc, "books");

                foreach (TreeViewItem rootItem in tv.Items)
                {
                    XmlElement eleBook = TreeToNode(doc, eleRoot, rootItem, "book");
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
                string fullXmlName_book = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString() + "/index.xml";

                XmlDocument doc = new XmlDocument();
                XmlElement eleRoot = TreeToNode(doc, "book");

                //保存分卷
                foreach (TreeViewItem volumeItem in bookItem.Items)
                {
                    XmlElement eleVolume = TreeToNode(doc, eleRoot, volumeItem, "volume");
                    SaveChapter(volumeItem, doc, eleVolume);
                }
                doc.Save(fullXmlName_book);
            }
            else
            {
                Console.WriteLine("参数可能存在错误！");
            }
        }

        static void SaveChapter(TreeViewItem volumeItem, XmlDocument doc, XmlElement eleVolume)
        {
            //保存章节
            foreach (TreeViewItem chapterItem in volumeItem.Items)
            {
                XmlElement eleChapter = TreeToNode(doc, eleVolume, chapterItem, "chapter"); ;
            }
        }
    }
}
