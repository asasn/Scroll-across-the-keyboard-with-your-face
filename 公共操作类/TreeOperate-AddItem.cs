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
            /// <summary>
            /// 创建一个新节点
            /// </summary>
            /// <param name="title"></param>
            /// <param name="itemType"></param>
            /// <returns></returns>
            static TreeViewItem CreateNewItem(string title, ItemType itemType)
            {
                TreeViewItem newItem = new TreeViewItem();
                newItem.AllowDrop = true;
                switch (itemType)
                {
                    case ItemType.目录:
                        newItem.Name = "dir";
                        newItem.Header = title;
                        newItem.Uid = Guid.NewGuid().ToString();
                        break;
                    case ItemType.文档:
                        newItem.Name = "doc";
                        newItem.Header = title;
                        newItem.Uid = Guid.NewGuid().ToString();
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
                if (selectedItem != null)
                {
                    newItem = CreateNewItem(title, itemType);
                    if (parentItem != null)
                    {
                        parentItem.Items.Add(newItem);
                    }
                    else
                    {
                        (selectedItem.Parent as TreeView).Items.Add(newItem);
                    }
                    TreeView tv = TreeOperate.GetRootItem(selectedItem).Parent as TreeView;
                    //TreeOperate.Save.SaveTree(tv);
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

                    TreeView tv = TreeOperate.GetRootItem(selectedItem).Parent as TreeView;
                    //TreeOperate.Save.SaveTree(tv);

                    //string tableName = TreeOperate.GetRootItem(newItem).Header.ToString() + "_Tree";
                    //TreeView tv = TreeOperate.GetRootItem(newItem).Parent as TreeView;
                    //newItem.Uid = TreeOperate.GetIndex(newItem);
                    //string sql = string.Format("insert or replace into {0} (id, 级别, 索引, 父id, 名称) values ('{1}', {2}, {3}, '{4}', '{5}');", tableName, newItem.Uid, GetLevel(newItem), selectedItem.Items.IndexOf(newItem), selectedItem.Uid, newItem.Header.ToString());
                    //SqliteOperate.ExecuteNonQuery(sql);
                }
                else
                {
                    MessageBox.Show("未选择基础节点");
                }
                SelectIt(newItem);
                return newItem;
            }

            public static void toTable(TreeViewItem newItem, string tableName)
            {
                if (tableName == "material")
                {
                    SqliteOperate.NewConnection(Gval.Base.AppPath + "/" + "material", "material.db");
                }

                tableName = "Tree_" + tableName;
                string bsql = string.Format("CREATE TABLE IF NOT EXISTS {0}(Uid CHAR PRIMARY KEY, Pid CHAR, Header CHAR, Name CHAR, Uid CHAR, Tag CHAR, DataContext CHAR, WordsCount INTEGER, IsExpanded BOOLEAN);", tableName);
                SqliteOperate.ExecuteNonQuery(bsql);
                TreeViewItem parentItem = newItem.Parent as TreeViewItem;
                string Pid;
                if (parentItem != null)
                {
                    Pid = parentItem.Uid;
                }
                else
                {
                    Pid = "";
                }
                int WordsCount = EditorOperate.WordCount(newItem.DataContext.ToString());
                string sql = string.Format("insert or ignore into {0} (Uid, Pid, Header, Name, Tag, DataContext, WordsCount, IsExpanded ) values ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', {7}, {8});", tableName, newItem.Uid, Pid, newItem.Header.ToString(), newItem.Name, newItem.Tag, newItem.DataContext, WordsCount, newItem.IsExpanded);
                SqliteOperate.ExecuteNonQuery(sql);

                //恢复默认连接
                SqliteOperate.NewConnection();
            }
        }


        /// <summary>
        /// 选择item并设置焦点
        /// </summary>
        /// <param name="thisItem"></param>
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




    }
}
