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
            public static class FromBookTree
            {
                /// <summary>
                /// 保存书目结构和关联的所有书籍结构
                /// </summary>
                /// <param name="tv"></param>
                public static void SaveAll(TreeView tv)
                {
                    //保存书目
                    SaveRoot(tv);
                    //保存书籍
                    foreach (TreeViewItem bookItem in tv.Items)
                    {
                        SaveCurBook(bookItem);
                    }
                }

                /// <summary>
                /// 流程1：保存书目结构
                /// </summary>
                /// <param name="tv"></param>
                public static void SaveRoot(TreeView tv)
                {
                    if (tv != null)
                    {
                        string fullXmlName_main = Gval.Base.AppPath + "/books/index.xml";
                        XmlDocument doc = new XmlDocument();
                        XmlElement eleRoot = ItemToRootElement(doc, "books");
                        foreach (TreeViewItem bookItem in tv.Items)
                        {
                            XmlElement eleBook = ItemToElement(doc, eleRoot, bookItem, bookItem.Name);
                        }
                        doc.Save(fullXmlName_main);
                    }
                }
                /// <summary>
                /// 流程2：保存当前书籍结构
                /// </summary>
                /// <param name="tv">目录树控件</param>
                /// <param name="bookItem">指向的书籍节点</param>
                public static void SaveCurBook(TreeViewItem bookItem)
                {
                    if (bookItem != null && GetLevel(bookItem) == 1)
                    {
                        //获取当前书籍对应的完整xml文件名
                        string fullXmlName_book = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString() + "/index.xml";

                        XmlDocument doc = new XmlDocument();
                        XmlElement eleRoot = ItemToRootElement(doc, "book");
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


            public static class FromNoteTree
            {
                public static void SaveAll(TreeView tv, TreeViewItem bookItem, string ucTag)
                {
                    if (tv != null && bookItem != null)
                    {
                        //获取当前笔记对应的完整xml文件名
                        string fullXmlName_notes = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString() + "/" + ucTag + ".xml";
                        XmlDocument doc = new XmlDocument();
                        XmlElement eleRoot = ItemToRootElement(doc, "notes");

                        foreach (TreeViewItem dirItem in tv.Items)
                        {
                            XmlElement eleDir = ItemToElement(doc, eleRoot, dirItem, "dir");
                            DoSaveToXml(dirItem, eleDir, doc);
                        }
                        doc.Save(fullXmlName_notes);
                    }
                }
            }

            public static class FromMaterialTree
            {
                public static void SaveAll(TreeView tv)
                {
                    if (tv != null)
                    {
                        //获取当前资料对应的完整xml文件名
                        string fullXmlName_material = Gval.Base.AppPath + "/material/index.xml";
                        XmlDocument doc = new XmlDocument();
                        XmlElement eleRoot = ItemToRootElement(doc, "material");
                        
                        foreach (TreeViewItem dirItem in tv.Items)
                        {
                            XmlElement eleDir = ItemToElement(doc, eleRoot, dirItem, "dir");
                            DoSaveToXml(dirItem, eleDir, doc);
                        }
                        doc.Save(fullXmlName_material);
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
            static XmlElement ItemToRootElement(XmlDocument doc, string eleName)
            {
                XmlElement ele = doc.CreateElement(eleName);
                doc.AppendChild(ele);
                return ele;
            }

            //递归保存
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
