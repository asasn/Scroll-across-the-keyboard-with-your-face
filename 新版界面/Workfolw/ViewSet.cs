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
        public static void ForViewPointX(Window thisWin, UIElement uc, double offset = 0)
        {
            thisWin.Left = uc.TranslatePoint(new Point(), Gval.View.MainWindow).X + offset;
        }
        public static void ForViewPointY(Window thisWin, UIElement uc, double offset = 0)
        {
            thisWin.Top = uc.TranslatePoint(new Point(), Gval.View.MainWindow).Y + offset;
        }
    }
}
