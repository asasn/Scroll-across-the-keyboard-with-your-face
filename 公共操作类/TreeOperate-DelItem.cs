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
        public static class DelItem
        {
            /// <summary>
            /// 删除当前选中的节点
            /// </summary>
            /// <param name="selectedItem"></param>
            public static void Do(TreeViewItem selectedItem)
            {
                if (selectedItem != null)
                {
                    TreeViewItem parentItem = selectedItem.Parent as TreeViewItem;
                    if (parentItem != null)
                    {
                        //parentItem不为空，删除非根节点
                        //【注意】执行此删除操作之后，原selectedItem节点已经不存在，后续操作需要谨慎
                        parentItem.Items.Remove(selectedItem);

                    }
                    else
                    {
                        //parentItem为空，删除根节点（书籍节点）
                        TreeView tv = selectedItem.Parent as TreeView;
                        tv.Items.Remove(selectedItem);
                    }
                    selectedItem.IsSelected = false;
                }
            }
        }

    }
}
