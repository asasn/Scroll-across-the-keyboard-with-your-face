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
