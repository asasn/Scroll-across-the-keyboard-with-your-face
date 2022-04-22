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
        /// 信息卡位置
        /// </summary>
        /// <param name="thisWin"></param>
        /// <param name="uc"></param>
        public static void ForCardPoint(Window thisWin, UIElement uc)
        {
            thisWin.Left = uc.TranslatePoint(new Point(), Gval.View.MainWindow).X - 5;
            thisWin.Top = 300;
        }

    }
}
