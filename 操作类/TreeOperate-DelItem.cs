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
        /// 删除当前选中的节点
        /// </summary>
        /// <param name="selectedItem"></param>
        public static void DelItem(TreeViewItem selectedItem)
        {
            if (null == selectedItem)
                return;
            TreeViewItem parentItem = selectedItem.Parent as TreeViewItem;

            //注意，执行删除操作之后，原选择节点已经不存在
            if (parentItem != null)
            {
                //parentItem不为空，删除书籍内节点
                parentItem.Items.Remove(selectedItem);
            }
            else
            {
                TreeView tv = selectedItem.Parent as TreeView;
                //parentItem为空，删除整个书籍
                tv.Items.Remove(selectedItem);
            }
            selectedItem.IsSelected = false;
        }
    }
}
