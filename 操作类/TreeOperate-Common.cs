﻿using System;
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
        /// 更新当前书籍的指向信息
        /// </summary>
        /// <param name="tv"></param>
        public static void ReNewCurrent(TreeView tv)
        {
            TreeViewItem selectedItem = tv.SelectedItem as TreeViewItem;
            if (selectedItem != null)
            {
                TreeViewItem bookItem = GetBookItem(selectedItem);
                TreeViewItem volumeItem = GetVolumeItem(selectedItem);

                //更新公共变量数据
                Gval.CurrentBook.curItem = selectedItem;
                Gval.CurrentBook.curVolumeItem = volumeItem;
                Gval.CurrentBook.curBookItem = bookItem;
                Gval.CurrentBook.curTv = bookItem.Parent as TreeView;
                Gval.CurrentBook.curBookPath = Gval.Base.AppPath + "/books/" + bookItem.Header.ToString();
                Gval.CurrentBook.curVolumePath = Gval.CurrentBook.curBookPath + '/' + volumeItem.Header.ToString();
                Gval.CurrentBook.curTextFullName = Gval.CurrentBook.curVolumePath + '/' + selectedItem.Header.ToString() + ".txt";


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
        /// 获取当前选中节点的书籍节点
        /// </summary>
        /// <param name="selectedItem">当前item</param>
        /// <returns></returns>
        public static TreeViewItem GetBookItem(TreeViewItem selectedItem)
        {
            if (selectedItem != null)
            {
                while (selectedItem.Name != "book" && (selectedItem.Parent as TreeViewItem) != null)
                {
                    selectedItem = selectedItem.Parent as TreeViewItem;
                }
            }
            return selectedItem;
        }

        /// <summary>
        /// 获取当前选中节点的分卷节点
        /// </summary>
        /// <param name="selectedItem">当前item</param>
        /// <returns></returns>
        public static TreeViewItem GetVolumeItem(TreeViewItem selectedItem)
        {
            if (selectedItem != null)
            {
                while (selectedItem.Name != "volume" && (selectedItem.Parent as TreeViewItem) != null)
                {
                    selectedItem = selectedItem.Parent as TreeViewItem;
                }
            }
            return selectedItem;
        }
    }
}
