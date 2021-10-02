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
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
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
    }
}