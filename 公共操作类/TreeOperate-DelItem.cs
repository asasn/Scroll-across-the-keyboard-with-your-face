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
                        TreeView tv = TreeOperate.GetRootItem(parentItem).Parent as TreeView;                        
                        //TreeOperate.Save.SaveTree(tv);
                    }
                    else
                    {
                        //parentItem为空，删除根节点（书籍节点）
                        (selectedItem.Parent as TreeView).Items.Remove(selectedItem);
                        //TreeOperate.Save.SaveTree(selectedItem.Parent as TreeView);
                    }                    
                }
            }


            public static void toTable(TreeViewItem selectedItem, string tableName)
            {
                if (tableName == "material")
                {
                    SqliteOperate.NewConnection(Gval.Base.AppPath + "/" + "material", "material.db");
                }

                tableName = "Tree_" + tableName;                
                string sql = string.Format("delete from {0} where Uid = '{1}';", tableName, selectedItem.Uid);
                SqliteOperate.ExecuteNonQuery(sql);
                ReTraversal(selectedItem, tableName);

                //恢复默认连接
                SqliteOperate.NewConnection();
            }


            static void ReTraversal(TreeViewItem selectedItem, string tableName)
            {
                foreach (TreeViewItem item in selectedItem.Items)
                {                    
                    string sql = string.Format("delete from {0} where Uid = '{1}';", tableName, selectedItem.Uid);
                    SqliteOperate.ExecuteNonQuery(sql);
                    ReTraversal(item, tableName);
                }
            }
        }

    }
}
