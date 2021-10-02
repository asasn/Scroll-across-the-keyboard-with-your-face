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
        static TreeViewItem CreateItem(typeOfItem t)
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
                    newItem.AllowDrop = false;
                    return newItem;
                case typeOfItem.note词条:
                    newItem.Name = "note";
                    newItem.Header = "note词条";
                    newItem.AllowDrop = true;
                    return newItem;
                case typeOfItem.note内容:
                    newItem.Name = "content";
                    newItem.Header = "note内容";
                    newItem.AllowDrop = false;
                    return newItem;
                default:
                    MessageBox.Show("无效的newItem");
                    return newItem;
            }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        static TreeViewItem AddItem(TreeView tv, typeOfItem t)
        {
            TreeViewItem newItem = CreateItem(t);
            tv.Items.Add(newItem);
            return newItem;
        }

        /// <summary>
        /// 非根节点
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        static TreeViewItem AddItem(TreeViewItem selectedItem, typeOfItem t)
        {
            TreeViewItem newItem = CreateItem(t);
            selectedItem.Items.Add(newItem);
            return newItem;
        }

        public static class BookTree
        {
            /// <summary>
            /// 添加新书籍
            /// </summary>
            /// <param name="tv"></param>
            /// <returns>newItem</returns>
            public static TreeViewItem AddNewBook(TreeView tv)
            {
                if (tv != null)
                {
                    TreeViewItem newItem = AddItem(tv, typeOfItem.书籍);
                    return newItem;
                }
                return null;
            }

            /// <summary>
            /// 添加新分卷
            /// </summary>
            /// <param name="tv"></param>
            /// <returns>newItem</returns>
            public static TreeViewItem AddNewVolume(TreeViewItem selectedItem)
            {
                if (selectedItem.Name == "book")
                {
                    TreeViewItem newItem = AddItem(selectedItem, typeOfItem.分卷);
                    return newItem;
                }
                return null;
            }

            /// <summary>
            /// 添加新章节
            /// </summary>
            /// <param name="selectedItem"></param>
            /// <returns>newItem</returns>
            public static TreeViewItem AddNewChapter(TreeViewItem selectedItem)
            {
                if (selectedItem.Name == "volume")
                {
                    TreeViewItem newItem = AddItem(selectedItem, typeOfItem.章节);
                    return newItem;
                }
                if (selectedItem.Name == "chapter")
                {
                    TreeViewItem newItem = AddItem(selectedItem.Parent as TreeViewItem, typeOfItem.章节);
                    return newItem;
                }
                return null;
            }
        }


    }
}
