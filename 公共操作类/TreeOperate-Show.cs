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
        /// 将xml文件内的目录内容显示在TreeView控件上
        /// </summary>
        public static partial class Show
        {
            public static class ToBookTree
            {
                /// <summary>
                /// 从两个xml文件当中展示
                /// </summary>
                /// <param name="tv"></param>
                public static void ShowAll(TreeView tv)
                {
                    //展示书目
                    ToRoot(tv);

                    //展示书籍
                    foreach (TreeViewItem rootItem in tv.Items)
                    {
                        ToCurBook(rootItem);
                    }
                }

                /// <summary>
                /// 流程1：从./books/index.xml文件载入，在TreeView控件上显示根节点（书籍名称）
                /// </summary>
                /// <param name="tv"></param>
                /// <param name="UcTag"></param>
                /// <param name="WorkXmlName"></param>
                static void ToRoot(TreeView tv)
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
                /// 流程2：从./books/当前书籍/index.xml文件载入，以根节点（书籍名称）为父控件，展示当前书籍的目录
                /// </summary>
                /// <param name="tv"></param>
                /// <param name="rootItem"></param>
                static void ToCurBook(TreeViewItem rootItem)
                {
                    //获取当前书籍对应的完整xml文件名
                    string fullXmlName_book = Gval.Base.AppPath + "/books/" + rootItem.Header.ToString() + "/index.xml";

                    XmlDocument doc = new XmlDocument();
                    doc.Load(fullXmlName_book);
                    XmlNode root = doc.SelectSingleNode("book");
                    XmlNodeList rlist = root.ChildNodes;
                    XmlNodeListToTree(rlist, rootItem);
                }
            }

            /// <summary>
            /// 从xml文件载入到控件进行展示（single模式）
            /// </summary>
            /// <param name="tv"></param>
            /// <param name="bookItem"></param>
            /// <param name="ucTag"></param>
            public static void FromSingleXml(TreeView tv, TreeViewItem bookItem, string ucTag)
            {

                //获取当前notes对应的完整xml文件名
                string fullXmlName;
                if (ucTag == "material")
                {
                    fullXmlName = Gval.Base.AppPath + "/" + ucTag + "/index.xml";
                }
                else
                {
                    fullXmlName = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString() + "/" + ucTag + ".xml";
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(fullXmlName);
                XmlNode root = doc.SelectSingleNode(ucTag);
                XmlNodeList rlist = root.ChildNodes;

                //以TreeView控件为起始
                XmlNodeListToTree(rlist, tv);
            }






            //递归载入(双xml文件，从根节点开始)
            static void XmlNodeListToTree(XmlNodeList nlist, TreeViewItem parentItem)
            {
                foreach (XmlNode ch in nlist)
                {
                    XmlElement rn = (XmlElement)ch;
                    TreeViewItem newItem = BuildItem(ch, parentItem);
                    if (ch.HasChildNodes)
                    {
                        XmlNodeListToTree(rn.ChildNodes, newItem);
                    }
                }
            }

            // 在控件中显示TreeViewItem（双xml文件，从根节点开始）
            static TreeViewItem BuildItem(XmlNode ch, TreeViewItem parentItem)
            {
                XmlElement rn = (XmlElement)ch;
                TreeViewItem newItem = new TreeViewItem();
                newItem.Name = rn.Name;
                newItem.Header = rn.GetAttribute("title");
                newItem.Uid = rn.GetAttribute("id");
                newItem.AllowDrop = true;
                parentItem.Items.Add(newItem);
                return newItem;
            }

            // 在控件中显示TreeViewItem（单xml模式：从控件开始）
            static TreeViewItem BuildItem(XmlNode ch, TreeView tv)
            {
                XmlElement rn = (XmlElement)ch;
                TreeViewItem newItem = new TreeViewItem();
                newItem.Name = rn.Name;
                newItem.Header = rn.GetAttribute("title");
                newItem.Uid = rn.GetAttribute("id");
                newItem.AllowDrop = true;
                tv.Items.Add(newItem);
                return newItem;
            }

            //递归载入(单xml模式：从TreeView控件开始)
            static void XmlNodeListToTree(XmlNodeList nlist, TreeView tv)
            {
                foreach (XmlNode ch in nlist)
                {
                    XmlElement rn = (XmlElement)ch;
                    TreeViewItem newItem = BuildItem(ch, tv);
                    if (ch.HasChildNodes)
                    {
                        //开始进入第二级
                        XmlNodeListToTree(rn.ChildNodes, newItem);
                    }
                }
            }




        }
    }
}





