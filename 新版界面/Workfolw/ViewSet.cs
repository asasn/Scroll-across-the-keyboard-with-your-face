using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RootNS.Workfolw
{
    /// <summary>
    /// 页面表现设置
    /// </summary>
    internal class ViewSet
    {
        /// <summary>
        /// 弹窗位置
        /// </summary>
        /// <param name="thisWin"></param>
        /// <param name="uc"></param>
        public static void ForViewPoint(Window thisWin, UIElement uc, int offsetX = 0, int offsetY = 0)
        {
            thisWin.Left = uc.TranslatePoint(new Point(), Gval.View.MainWindow).X + offsetX;
            thisWin.Top = uc.TranslatePoint(new Point(), Gval.View.MainWindow).Y + offsetY;
        }

    }
}
