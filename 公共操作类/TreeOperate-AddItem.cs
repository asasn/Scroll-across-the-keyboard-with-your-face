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
        public enum ItemType : int
        {
            目录,
            文档,
        }


        public static class AddItem
        {
            static TreeViewItem CreateNewItem(string title, ItemType itemType)
            {
                TreeViewItem newItem = new TreeViewItem();
                newItem.AllowDrop = true;
                switch (itemType)
                {
                    case ItemType.目录:
                        newItem.Name = "dir";
                        newItem.Header = title;
                        break;
                    case ItemType.文档:
                        newItem.Name = "doc";
                        newItem.Header = title;
                        break;
                    default:
                        MessageBox.Show("无效的newItem");
                        break;
                }
                return newItem;
            }

            /// <summary>
            /// 添加根节点
            /// </summary>
            /// <param name="tv"></param>
            /// <param name="title"></param>
            /// <param name="itemType"></param>
            /// <returns></returns>
            public static TreeViewItem RootItem(TreeView tv, string title, ItemType itemType)
            {
                TreeViewItem newItem = CreateNewItem(title, itemType);
                tv.Items.Add(newItem);
                SelectIt(newItem);
                return newItem;
            }

            /// <summary>
            /// 添加同级节点
            /// </summary>
            /// <param name="selectedItem"></param>
            /// <param name="title"></param>
            /// <param name="itemType"></param>
            /// <returns></returns>
            public static TreeViewItem BrotherItem(TreeViewItem selectedItem, string title, ItemType itemType)
            {
                TreeViewItem newItem = null;
                TreeViewItem parentItem = selectedItem.Parent as TreeViewItem;
                TreeView tv = selectedItem.Parent as TreeView;
                if (selectedItem != null)
                {
                    newItem = CreateNewItem(title, itemType);
                    if (parentItem != null)
                    {
                        parentItem.Items.Add(newItem);
                    }
                    else
                    {
                        tv.Items.Add(newItem);
                    }
                }
                else
                {
                    MessageBox.Show("未选择基础节点");
                }
                SelectIt(newItem);
                return newItem;
            }

            /// <summary>
            /// 添加子节点
            /// </summary>
            /// <param name="selectedItem"></param>
            /// <param name="title"></param>
            /// <param name="itemType"></param>
            /// <returns></returns>
            public static TreeViewItem ChildItem(TreeViewItem selectedItem, string title, ItemType itemType)
            {
                TreeViewItem newItem = null;
                if (selectedItem != null)
                {
                    newItem = CreateNewItem(title, itemType);
                    selectedItem.Items.Add(newItem);
                }
                else
                {
                    MessageBox.Show("未选择基础节点");
                }
                SelectIt(newItem);
                return newItem;
            }
        }


        //设置Item焦点
        static void SelectIt(TreeViewItem thisItem)
        {
            TreeViewItem parentItem = thisItem.Parent as TreeViewItem;
            if (parentItem != null)
            {
                parentItem.IsExpanded = true;
            }
            thisItem.IsSelected = true;
            thisItem.Focus();
        }

        //static TreeViewItem CreateANewCard(typeOfInfoCard t)
        //{
        //    TreeViewItem newItem = new TreeViewItem();
        //    newItem.AllowDrop = true;
        //    switch (t)
        //    {
        //        case typeOfInfoCard.角色:
        //            newItem.Name = "note";
        //            newItem.Header = "新角色";
        //            return newItem;
        //        case typeOfInfoCard.场景:
        //            newItem.Name = "note";
        //            newItem.Header = "新场景";
        //            return newItem;
        //        case typeOfInfoCard.道具:
        //            newItem.Name = "note";
        //            newItem.Header = "新道具";
        //            return newItem;
        //        case typeOfInfoCard.势力:
        //            newItem.Name = "note";
        //            newItem.Header = "新势力";
        //            return newItem;
        //        default:
        //            MessageBox.Show("无效的newItem");
        //            return newItem;
        //    }
        //}

        ////根据输入的类型创建NoteItem
        //static TreeViewItem CreateANewNote(typeOfNote t)
        //{
        //    TreeViewItem newItem = new TreeViewItem();
        //    newItem.AllowDrop = true;
        //    switch (t)
        //    {
        //        case typeOfNote.资料分卷:
        //            newItem.Name = "note";
        //            newItem.Header = "新资料分卷";
        //            return newItem;
        //        case typeOfNote.资料文档:
        //            newItem.Name = "line";
        //            newItem.Header = "新文档";
        //            return newItem;
        //        case typeOfNote.备忘:
        //            newItem.Name = "note";
        //            newItem.Header = "新年表";
        //            return newItem;
        //        case typeOfNote.备忘行:
        //            newItem.Name = "line";
        //            newItem.Header = "新备忘";
        //            return newItem;
        //        case typeOfNote.大纲:
        //            newItem.Name = "note";
        //            newItem.Header = "新大纲条目";
        //            return newItem;
        //        case typeOfNote.大纲行:
        //            newItem.Name = "line";
        //            newItem.Header = "新行";
        //            return newItem;
        //        default:
        //            MessageBox.Show("无效的newItem");
        //            return newItem;
        //    }
        //}


        ////根据输入的类型创建item
        //static TreeViewItem CreateANewItem(typeOfItem t)
        //{
        //    TreeViewItem newItem = new TreeViewItem();
        //    switch (t)
        //    {
        //        case typeOfItem.书籍:
        //            newItem.Name = "book";
        //            newItem.Header = "新书籍";
        //            newItem.AllowDrop = true;
        //            return newItem;
        //        case typeOfItem.分卷:
        //            newItem.Name = "volume";
        //            newItem.Header = "新分卷";
        //            newItem.AllowDrop = true;
        //            return newItem;
        //        case typeOfItem.章节:
        //            newItem.Name = "chapter";
        //            newItem.Header = "新章节";
        //            newItem.AllowDrop = true;
        //            return newItem;
        //        default:
        //            MessageBox.Show("无效的newItem");
        //            return newItem;
        //    }
        //}

        //public static partial class BookTree
        //{
        //    //在指定节点添加指定的item
        //    public static void AddThisItem(TreeViewItem baseItem, TreeViewItem thisItem)
        //    {
        //        if (baseItem == null)
        //            return;
        //        if (thisItem.Name == "chapter")
        //        {
        //            if (baseItem.Name == "chapter")
        //            {
        //                TreeViewItem volumeItem = baseItem.Parent as TreeViewItem;
        //                //volumeItem.Items.Insert(volumeItem.Items.IndexOf(baseItem) + 1, thisItem);
        //                volumeItem.Items.Add(thisItem);
        //            }
        //            if (baseItem.Name == "volume")
        //            {
        //                baseItem.Items.Add(thisItem);
        //            }
        //        }

        //        if (thisItem.Name == "volume")
        //        {
        //            if (baseItem.Name == "volume")
        //            {
        //                TreeViewItem bookItem = baseItem.Parent as TreeViewItem;
        //                //bookItem.Items.Insert(bookItem.Items.IndexOf(baseItem) + 1, thisItem);
        //                bookItem.Items.Add(thisItem);
        //            }
        //            if (baseItem.Name == "book")
        //            {
        //                baseItem.Items.Add(thisItem);
        //            }
        //        }

        //        if (thisItem.Name == "book")
        //        {

        //            if (baseItem.Name == "book")
        //            {
        //                TreeView tv = baseItem.Parent as TreeView;
        //                //tv.Items.Insert(tv.Items.IndexOf(baseItem) + 1, thisItem);
        //                tv.Items.Add(thisItem);
        //            }
        //            if (baseItem == null)
        //            {
        //                Console.WriteLine("baseItem错误！");
        //            }
        //        }

        //        SelectIt(thisItem);
        //    }

        //    //在指定节点添加指定的item
        //    public static void AddThisItem(TreeView tv, TreeViewItem thisItem)
        //    {
        //        if (tv == null)
        //            return;
        //        if (thisItem.Name == "book")
        //        {
        //            tv.Items.Add(thisItem);
        //            SelectIt(thisItem);
        //        }
        //    }

        //    /// <summary>
        //    /// 创建并添加新书籍
        //    /// </summary>
        //    /// <param name="tv"></param>
        //    /// <returns>newItem</returns>
        //    public static TreeViewItem AddNewBook(TreeView tv)
        //    {
        //        if (tv != null)
        //        {
        //            TreeViewItem newItem = CreateANewItem(typeOfItem.书籍);
        //            AddThisItem(tv, newItem);
        //            return newItem;
        //        }
        //        return null;
        //    }

        //    /// <summary>
        //    /// 创建并添加新分卷
        //    /// </summary>
        //    /// <param name="tv"></param>
        //    /// <returns>newItem</returns>
        //    public static TreeViewItem AddNewVolume(TreeViewItem selectedItem)
        //    {
        //        if (selectedItem != null && (selectedItem.Name == "book" || selectedItem.Name == "volume"))
        //        {
        //            TreeViewItem newItem = CreateANewItem(typeOfItem.分卷);
        //            AddThisItem(selectedItem, newItem);
        //            return newItem;
        //        }
        //        return null;
        //    }

        //    /// <summary>
        //    /// 创建并添加新章节
        //    /// </summary>
        //    /// <param name="selectedItem"></param>
        //    /// <returns>newItem</returns>
        //    public static TreeViewItem AddNewChapter(TreeViewItem selectedItem)
        //    {
        //        if (selectedItem != null && (selectedItem.Name == "volume" || selectedItem.Name == "chapter"))
        //        {
        //            TreeViewItem newItem = CreateANewItem(typeOfItem.章节);
        //            AddThisItem(selectedItem, newItem);
        //            return newItem;
        //        }
        //        return null;
        //    }
        //}

        //public static partial class NoteTree
        //{
        //    //在指定节点添加指定的item
        //    public static void AddThisItem(TreeViewItem baseItem, TreeViewItem thisItem)
        //    {
        //        if (baseItem == null)
        //            return;
        //        if (thisItem.Name == "line")
        //        {
        //            if (baseItem.Name == "line")
        //            {
        //                TreeViewItem noteItem = baseItem.Parent as TreeViewItem;
        //                //volumeItem.Items.Insert(volumeItem.Items.IndexOf(baseItem) + 1, thisItem);
        //                noteItem.Items.Add(thisItem);
        //            }
        //            if (baseItem.Name == "note")
        //            {
        //                baseItem.Items.Add(thisItem);
        //            }
        //        }

        //        if (thisItem.Name == "note")
        //        {

        //            if (baseItem.Name == "note")
        //            {
        //                TreeView tv = baseItem.Parent as TreeView;
        //                //tv.Items.Insert(tv.Items.IndexOf(baseItem) + 1, thisItem);
        //                tv.Items.Add(thisItem);
        //            }
        //            if (baseItem == null)
        //            {
        //                Console.WriteLine("baseItem错误！");
        //            }
        //        }

        //        SelectIt(thisItem);
        //    }

        //    //在指定节点添加指定的item
        //    public static void AddThisItem(TreeView tv, TreeViewItem thisItem)
        //    {
        //        if (tv == null)
        //            return;
        //        if (thisItem.Name == "note")
        //        {
        //            tv.Items.Add(thisItem);
        //            SelectIt(thisItem);
        //        }
        //    }



        //    /// <summary>
        //    /// 创建一级记事
        //    /// </summary>
        //    /// <param name="tv"></param>
        //    /// <returns>newItem</returns>
        //    public static TreeViewItem AddNewNote(TreeView tv, typeOfNote t)
        //    {
        //        if (tv != null)
        //        {
        //            TreeViewItem newItem = CreateANewNote(t);
        //            AddThisItem(tv, newItem);
        //            return newItem;
        //        }
        //        return null;
        //    }

        //    /// <summary>
        //    /// 创建一级记事
        //    /// </summary>
        //    /// <param name="tv"></param>
        //    /// <returns>newItem</returns>
        //    public static TreeViewItem AddNewNote(TreeView tv, typeOfInfoCard t)
        //    {
        //        if (tv != null)
        //        {
        //            TreeViewItem newItem = CreateANewCard(t);
        //            AddThisItem(tv, newItem);
        //            return newItem;
        //        }
        //        return null;
        //    }

        //    /// <summary>
        //    /// 创建二级记事
        //    /// </summary>
        //    /// <param name="tv"></param>
        //    /// <returns>newItem</returns>
        //    public static TreeViewItem AddNewLine(TreeViewItem selectedItem, typeOfNote t)
        //    {
        //        if (selectedItem != null && (selectedItem.Name == "note" || selectedItem.Name == "line"))
        //        {
        //            TreeViewItem newItem = CreateANewNote(t);
        //            AddThisItem(selectedItem, newItem);
        //            return newItem;
        //        }
        //        return null;
        //    }

        //}
    }
}
