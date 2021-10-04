using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace 脸滚键盘
{
    static partial class TreeOperate
    {
        public static class DragDropItem
        {
            static Point _lastMouseDown;
            /// <summary>
            /// 拖曳移动中
            /// </summary>
            /// <param name="tv"></param>
            /// <param name="e"></param>
            public static void DragMove(TreeView tv, MouseEventArgs e)
            {

                try
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        //获取鼠标移动的距离
                        Point currentPosition = e.GetPosition(tv);

                        //判断鼠标是否移动
                        if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 5.0) ||
                            (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 5.0))
                        {
                            //获取鼠标选中的节点数据
                            TreeViewItem draggedItem = (TreeViewItem)tv.SelectedItem;
                            if (draggedItem != null)
                            {
                                //启动拖放操作
                                //DragDropEffects finalDropEffect = DragDrop.DoDragDrop(tv, tv.SelectedValue,System.Windows.DragDropEffects.Move);
                                DragDrop.DoDragDrop(tv, tv.SelectedValue, System.Windows.DragDropEffects.Move);
                                e.Handled = true;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            /// <summary>
            /// 在指定的节点放下一个item
            /// </summary>
            /// <param name="dragItem">来源节点</param>
            /// <param name="dropItem">目标节点</param>
            /// <returns>成功返回true，未成功则返回false</returns>
            public static bool DoDrop(TreeViewItem dragItem, TreeViewItem dropItem)
            {
                int dragLevel = GetLevel(dragItem);
                int dropLevel = GetLevel(dropItem);

                if (false == IsHasSameHeader(dragItem, dropItem))
                {
                    //源和目标节点同级的情况
                    if (dragLevel == dropLevel)
                    {
                        if (dropItem.Parent as TreeViewItem != null)
                        {
                            //不是顶层
                            DelItem.Do(Gval.DragDrop.dragItem);
                            (dropItem.Parent as TreeViewItem).Items.Add(dragItem);
                            return true;
                        }
                        else
                        {
                            //是顶层
                            DelItem.Do(Gval.DragDrop.dragItem);
                            (dropItem.Parent as TreeView).Items.Add(dragItem);
                            return true;
                        }
                        
                    }

                    //源比目标节点刚好深一级的情况
                    if (dragLevel - dropLevel == 1)
                    {
                        DelItem.Do(Gval.DragDrop.dragItem);
                        dropItem.Items.Add(dragItem);
                        return true;
                    }

                }
                    return false;
            }


            /// <summary>
            /// 判断拖动的源节点和目标节点之间是否存在同名节点（同时对比目标节点的子节点）
            /// </summary>
            /// <param name="dragItem"></param>
            /// <param name="dropItem"></param>
            /// <returns>存在同名节点则返回true</returns>
            static bool IsHasSameHeader(TreeViewItem dragItem, TreeViewItem dropItem)
            {
                foreach (TreeViewItem ditem in dropItem.Items)
                {
                    if (dragItem.Header.ToString() == ditem.Header.ToString())
                    {
                        return true;
                    }
                }
                if (dragItem.Header.ToString() == dropItem.Header.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}