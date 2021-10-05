using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace 脸滚键盘
{
    static partial class TreeOperate
    {
        /// <summary>
        /// 获取item关联的文件/文件夹路径
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns>selectedItem为null时返回工作路径，selectedItem不为null时返回Item路径</returns>
        public static string GetItemPath(TreeViewItem selectedItem, string workPath)
        {
            ArrayList myItems = new ArrayList();
            string itemPath = string.Empty;
            TreeViewItem curItem = selectedItem;
            itemPath += Gval.Base.AppPath + "/" + workPath;
            if (selectedItem != null)
            {
                do
                {
                    myItems.Add(curItem);
                    curItem = curItem.Parent as TreeViewItem;
                } while (curItem != null);
                myItems.Reverse();
                foreach (TreeViewItem item in myItems)//倒序操作
                {
                    if (item.Name == "doc")
                    {
                        itemPath += "/" + item.Header.ToString() + ".txt";
                    }
                    else
                    {
                        itemPath += "/" + item.Header.ToString();
                    }

                }

            }
            return itemPath;
        }

        /// <summary>
        /// 获取节点所在的层级，无选中或者不在TreeView内的为-1
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        public static int GetLevel(TreeViewItem selectedItem)
        {
            int level = -1;
            if (selectedItem != null)
            {
                while ((selectedItem.Parent as TreeViewItem) != null)
                {
                    selectedItem = selectedItem.Parent as TreeViewItem;
                    level--;
                }
                level = System.Math.Abs(level);
                return level;
            }
            else
            {
                level = 0;
                return level;
            }

        }

        public enum typeOfItem : int
        {
            书籍,
            分卷,
            章节,
        }

        /// <summary>
        /// note节点类型
        /// </summary>
        public enum typeOfNote : int
        {
            资料分卷,
            资料文档,
            备忘,
            备忘行,
            大纲,
            大纲行,
        }

        /// <summary>
        /// card节点类型
        /// </summary>
        public enum typeOfInfoCard : int
        {
            角色,
            场景,
            道具,
            势力,
        }

        /// <summary>
        /// 更新当前书籍的指向信息
        /// </summary>
        /// <param name="tv"></param>
        public static void ReNewCurrent(TreeView tv, string workPath)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                TreeViewItem bookItem = GetRootItem(selectedItem);
                TreeViewItem volumeItem = GetItemByLevel(selectedItem, 2);

                //更新公共变量数据
                Gval.Current.curTv = tv;
                Gval.Current.curItem = selectedItem;
                Gval.Current.curVolumeItem = volumeItem;
                Gval.Current.curBookItem = bookItem;
                Gval.Current.curItemPath = GetItemPath(selectedItem, workPath);
                Gval.Current.curVolumePath = GetItemPath(volumeItem, workPath);
                Gval.Current.curBookPath = GetItemPath(bookItem, workPath);
            }
            else
            {
                //更新公共变量数据
                Gval.Current.curTv = null;
                Gval.Current.curItem = null;
                Gval.Current.curVolumeItem = null;
                Gval.Current.curBookItem = null;
                Gval.Current.curItemPath = null;
                Gval.Current.curVolumePath = null;
                Gval.Current.curBookPath = null;
            }
        }

        /// <summary>
        /// 向上获取根节点
        /// </summary>
        /// <param name="selectedItem">当前item</param>
        /// <returns>根节点rootItem</returns>
        public static TreeViewItem GetRootItem(TreeViewItem selectedItem)
        {
            if (selectedItem != null)
            {
                while (selectedItem.Parent as TreeViewItem != null)
                {
                    selectedItem = selectedItem.Parent as TreeViewItem;
                }
            }
            return selectedItem;
        }

        /// <summary>
        /// 向上获取控件对象
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns>控件对象TreeView</returns>
        public static TreeView GetTreeView(TreeViewItem selectedItem)
        {
            TreeView tv = null;
            if (selectedItem != null)
            {
                TreeViewItem rootItem = GetRootItem(selectedItem);
                tv = rootItem.Parent as TreeView;
            }
            return tv;
        }

        /// <summary>
        /// 向上获取指定层级的节点
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="tl">想要获取的目标层级（1~？）</param>
        /// <returns>节点对象</returns>
        public static TreeViewItem GetItemByLevel(TreeViewItem selectedItem, int tl)
        {
            if (tl > 0)
            {
                int level = GetLevel(selectedItem);
                if (selectedItem != null && level >= tl)
                {
                    while (level > tl && selectedItem.Parent as TreeViewItem != null)
                    {
                        selectedItem = selectedItem.Parent as TreeViewItem;
                        level = GetLevel(selectedItem);
                    }
                    return selectedItem;
                }
            }
            return null;
        }
    }
}
