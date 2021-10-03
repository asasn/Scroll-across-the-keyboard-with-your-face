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
        /// 双xml文件到Tree
        /// </summary>
        public static class XmlToBookTree
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

            /// <summary>
            /// 流程1：在TreeView控件上显示根目录，展示各书籍
            /// </summary>
            /// <param name="tv"></param>
            /// <param name="WorkPath"></param>
            /// <param name="WorkXmlName"></param>
            private static void ShowBooks(TreeView tv)
            {
                string fullXmlName_main = Gval.Base.AppPath + "/books/index.xml";

                XmlDocument doc = new XmlDocument();
                doc.Load(fullXmlName_main);
                XmlNode root = doc.SelectSingleNode("books");
                XmlNodeList rlist = root.ChildNodes;
                foreach (XmlNode ch in rlist)
                {
                    XmlElement rn = (XmlElement)ch;
                    TreeViewItem rootItem = new TreeViewItem();
                    rootItem.Name = rn.Name;
                    rootItem.Header = rn.GetAttribute("title");
                    rootItem.AllowDrop = true;
                    tv.Items.Add(rootItem);
                }
            }

            /// <summary>
            /// 流程2：以根节点为书籍父控件，展示所有书籍
            /// </summary>
            /// <param name="tv"></param>
            /// <param name="workDir"></param>
            /// <param name="xmlName"></param>
            private static void ShowBook(TreeView tv, TreeViewItem rootItem)
            {
                //获取当前书籍对应的完整xml文件名
                string fullXmlName_book = Gval.Base.AppPath + "/books/" + rootItem.Header.ToString() + "/index.xml";

                XmlDocument doc = new XmlDocument();
                doc.Load(fullXmlName_book);
                XmlNode root = doc.SelectSingleNode("book");
                XmlNodeList rlist = root.ChildNodes;
                LoadInTree(rlist, rootItem);
            }



        }


        public static class XmlToNoteTree
        {
            public static void Show(TreeView tv, string fullXmlName_notes)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fullXmlName_notes);
                XmlNode root = doc.SelectSingleNode("notes");
                XmlNodeList rlist = root.ChildNodes;

                //以TreeView控件为起始
                LoadInTree(rlist, tv);
            }

        }

        //递归载入(双xml文件，从根节点开始)
        private static void LoadInTree(XmlNodeList nlist, TreeViewItem parentItem)
        {
            foreach (XmlNode ch in nlist)
            {
                XmlElement rn = (XmlElement)ch;
                TreeViewItem newItem = ShowXmlNodeToTreeView(ch, parentItem);
                if (ch.HasChildNodes)
                {
                    LoadInTree(rn.ChildNodes, newItem);
                }
            }
        }

        // 在控件中显示TreeViewItem（双xml文件，从根节点开始）
        private static TreeViewItem ShowXmlNodeToTreeView(XmlNode ch, TreeViewItem parentItem)
        {
            XmlElement rn = (XmlElement)ch;
            TreeViewItem newItem = new TreeViewItem();
            newItem.Name = rn.Name;
            newItem.Header = rn.GetAttribute("title");
            newItem.Tag = rn.GetAttribute("content");
            newItem.AllowDrop = true;
            parentItem.Items.Add(newItem);
            return newItem;
        }


        //递归载入(单xml模式：从TreeView控件开始)
        private static void LoadInTree(XmlNodeList nlist, TreeView tv)
        {
            foreach (XmlNode ch in nlist)
            {
                XmlElement rn = (XmlElement)ch;
                TreeViewItem newItem = ShowXmlNodeToTreeView(ch, tv);
                if (ch.HasChildNodes)
                {
                    //开始进入第二级
                    LoadInTree(rn.ChildNodes, newItem);
                }
            }
        }

        // 在控件中显示TreeViewItem（单xml模式：从控件开始）
        private static TreeViewItem ShowXmlNodeToTreeView(XmlNode ch, TreeView tv)
        {
            XmlElement rn = (XmlElement)ch;
            TreeViewItem newItem = new TreeViewItem();
            newItem.Name = rn.Name;
            newItem.Header = rn.GetAttribute("title");
            newItem.Tag = rn.GetAttribute("content");
            newItem.AllowDrop = true;
            
            tv.Items.Add(newItem);
            return newItem;
        }
    }
}
