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
