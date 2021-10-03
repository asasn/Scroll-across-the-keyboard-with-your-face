using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace 脸滚键盘
{
    static partial class TreeOperate
    {
        enum typeOfItem : int
        {
            书籍,
            分卷,
            章节,
            note词条,
            note内容,
        }

        //根据输入的类型创建item
        static TreeViewItem CreateANewItem(typeOfItem t)
        {
            TreeViewItem newItem = new TreeViewItem();
            switch (t)
            {
                case typeOfItem.书籍:
                    newItem.Name = "book";
                    newItem.Header = "新书籍";
                    newItem.AllowDrop = true;
                    return newItem;
                case typeOfItem.分卷:
                    newItem.Name = "volume";
                    newItem.Header = "新分卷";
                    newItem.AllowDrop = true;
                    return newItem;
                case typeOfItem.章节:
                    newItem.Name = "chapter";
                    newItem.Header = "新章节";
                    newItem.AllowDrop = true;
                    return newItem;
                case typeOfItem.note词条:
                    newItem.Name = "note";
                    newItem.Header = "note词条";
                    newItem.AllowDrop = true;
                    return newItem;
                case typeOfItem.note内容:
                    newItem.Name = "content";
                    newItem.Header = "note内容";
                    newItem.AllowDrop = true;
                    return newItem;
                default:
                    MessageBox.Show("无效的newItem");
                    return newItem;
            }
        }

        public static class BookTree
        {
            //在指定节点添加指定的item
            public static void AddThisItem(TreeViewItem baseItem, TreeViewItem thisItem)
            {
                if (baseItem == null)
                    return;
                if (thisItem.Name == "chapter")
                {
                    if (baseItem.Name == "chapter")
                    {
                        TreeViewItem volumeItem = baseItem.Parent as TreeViewItem;
                        //volumeItem.Items.Insert(volumeItem.Items.IndexOf(baseItem) + 1, thisItem);
                        volumeItem.Items.Add(thisItem);
                    }
                    if (baseItem.Name == "volume")
                    {
                        baseItem.Items.Add(thisItem);
                    }
                    thisItem.IsExpanded = true;
                    thisItem.Focus();
                }

                if (thisItem.Name == "volume")
                {
                    if (baseItem.Name == "volume")
                    {
                        TreeViewItem bookItem = baseItem.Parent as TreeViewItem;
                        //bookItem.Items.Insert(bookItem.Items.IndexOf(baseItem) + 1, thisItem);
                        bookItem.Items.Add(thisItem);
                    }
                    if (baseItem.Name == "book")
                    {
                        baseItem.Items.Add(thisItem);
                    }
                    thisItem.IsExpanded = true;
                    thisItem.Focus();
                }

                if (thisItem.Name == "book")
                {
                    
                    if (baseItem.Name == "book")
                    {
                        TreeView tv = baseItem.Parent as TreeView;
                        //tv.Items.Insert(tv.Items.IndexOf(baseItem) + 1, thisItem);
                        tv.Items.Add(thisItem);
                    }
                    if (baseItem == null)
                    {
                        Console.WriteLine("baseItem错误！");
                    }
                    thisItem.Focus();
                }
            }

            //在指定节点添加指定的item
            public static void AddThisItem(TreeView tv, TreeViewItem thisItem)
            {
                if (tv == null)
                    return;
                if (thisItem.Name == "book")
                {
                    tv.Items.Add(thisItem);
                    thisItem.Focus();
                }
            }

            /// <summary>
            /// 创建并添加新书籍
            /// </summary>
            /// <param name="tv"></param>
            /// <returns>newItem</returns>
            public static TreeViewItem AddNewBook(TreeView tv)
            {
                if (tv != null)
                {
                    TreeViewItem newItem = CreateANewItem(typeOfItem.书籍);
                    AddThisItem(tv, newItem);
                    return newItem;
                }
                return null;
            }

            /// <summary>
            /// 创建并添加新分卷
            /// </summary>
            /// <param name="tv"></param>
            /// <returns>newItem</returns>
            public static TreeViewItem AddNewVolume(TreeViewItem selectedItem)
            {
                if (selectedItem != null && (selectedItem.Name == "book" || selectedItem.Name == "volume"))
                {
                    TreeViewItem newItem = CreateANewItem(typeOfItem.分卷);
                    AddThisItem(selectedItem, newItem);
                    return newItem;
                }
                return null;
            }

            /// <summary>
            /// 创建并添加新章节
            /// </summary>
            /// <param name="selectedItem"></param>
            /// <returns>newItem</returns>
            public static TreeViewItem AddNewChapter(TreeViewItem selectedItem)
            {
                if (selectedItem != null && (selectedItem.Name == "volume" || selectedItem.Name == "chapter"))
                {
                    TreeViewItem newItem = CreateANewItem(typeOfItem.章节);
                    AddThisItem(selectedItem, newItem);
                    return newItem;
                }
                return null;
            }
        }


    }
}
