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
        /// 将xml文件内的目录内容显示在控件上
        /// </summary>
        public static void Show(TreeView tv)
        {
            //展示书目
            ShowBooks(tv);

            //展示书籍
            foreach (TreeViewItem rootItem in tv.Items)
            {
                ShowBook(tv, rootItem);
            }
        }

        static XmlNodeList GetXmlList(string fullXmlName, string nodeName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fullXmlName);
            XmlNode root = doc.SelectSingleNode(nodeName);
            XmlNodeList rlist = root.ChildNodes;
            return rlist;
        }

        static void NodeToItem(TreeView tv, XmlNode ch)
        {
            XmlElement rn = (XmlElement)ch;
            TreeViewItem item = new TreeViewItem();
            item.Name = rn.Name;
            item.Header = rn.GetAttribute("title");
            if (item.Name == "txt")
            {
                item.AllowDrop = false;
            }
            else
            {
                item.AllowDrop = true;
            }
            tv.Items.Add(item);
        }

        static TreeViewItem NodeToItem(TreeViewItem parentItem, XmlNode ch)
        {
            XmlElement rn = (XmlElement)ch;
            TreeViewItem item = new TreeViewItem();
            item.Name = rn.Name;
            item.Header = rn.GetAttribute("title");
            if (item.Name == "txt")
            {
                item.AllowDrop = false;
            }
            else
            {
                item.AllowDrop = true;
            }
            parentItem.Items.Add(item);
            return item;
        }

        static void ShowBooks(TreeView tv)
        {
            if (tv != null)
            {
                string fullXmlName_main = Gval.Base.AppPath + "/books/index.xml";

                XmlNodeList rlist = GetXmlList(fullXmlName_main, "books");

                foreach (XmlNode ch in rlist)
                {
                    NodeToItem(tv, ch);
                }
            }
        }

        static void ShowBook(TreeView tv, TreeViewItem rootItem)
        {
            if (tv != null)
            {
                string fullXmlName_book = Gval.Base.AppPath + "/books/" + rootItem.Header.ToString() + "/index.xml";

                XmlNodeList rlist = GetXmlList(fullXmlName_book, "book");

                //展示分卷
                foreach (XmlNode ch in rlist)
                {
                    XmlElement rn = (XmlElement)ch;
                    TreeViewItem volumeItem = NodeToItem(rootItem, ch);
                    ShowChapter(volumeItem, rn.ChildNodes);
                }
            }
        }

        static void ShowChapter(TreeViewItem volumeItem, XmlNodeList rlist)
        {
            //展示章节
            foreach (XmlNode ch in rlist)
            {
                NodeToItem(volumeItem, ch);
            }
        }
    }
}
