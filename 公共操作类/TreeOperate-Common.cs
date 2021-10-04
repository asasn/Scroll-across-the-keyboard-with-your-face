using System;
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
        public static void ReNewCurrent(TreeView tv)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                TreeViewItem bookItem = GetRootItem(selectedItem);
                TreeViewItem volumeItem = GetItemByLevel(selectedItem, 2);

                //更新公共变量数据
                Gval.CurrentBook.curItem = selectedItem;
                Gval.CurrentBook.curVolumeItem = volumeItem;
                Gval.CurrentBook.curBookItem = bookItem;
                Gval.CurrentBook.curTv = bookItem.Parent as TreeView;
                Gval.CurrentBook.curBookPath = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString();
                if (volumeItem != null)
                {
                    Gval.CurrentBook.curVolumePath = Gval.CurrentBook.curBookPath + '/' + volumeItem.Header.ToString();
                    Gval.CurrentBook.curTextFullName = Gval.CurrentBook.curVolumePath + '/' + selectedItem.Header.ToString() + ".txt";
                }



            }
            else
            {
                //更新公共变量数据
                Gval.CurrentBook.curItem = null;
                Gval.CurrentBook.curVolumeItem = null;
                Gval.CurrentBook.curBookItem = null;
                Gval.CurrentBook.curTv = null;
                Gval.CurrentBook.curBookPath = null;
                Gval.CurrentBook.curVolumePath = null;
                Gval.CurrentBook.curTextFullName = null;
            }
        }

        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <param name="selectedItem">当前item</param>
        /// <returns></returns>
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
        /// 向上获取指定层级的节点
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="tl">想要获取的目标层级</param>
        /// <returns></returns>
        public static TreeViewItem GetItemByLevel(TreeViewItem selectedItem, int tl)
        {
            if (tl > 0)
            {
                int level = GetLevel(selectedItem);
                //选择节点的层级必须比想要获取的目标深
                if (selectedItem != null)
                {
                    while (level > tl && (selectedItem.Parent as TreeViewItem) != null)
                    {
                        selectedItem = selectedItem.Parent as TreeViewItem;
                        level = GetLevel(selectedItem);
                    }
                    return selectedItem;
                }
                else
                {
                    Console.WriteLine("选择的节点错误！");
                    return null;
                }
                
            }
            else
            {
                Console.WriteLine("想要获取的目标层级错误！");
                return null;
            }

        }


    }
}
