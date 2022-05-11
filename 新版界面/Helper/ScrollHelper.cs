using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RootNS.Helper
{
    public class ScrollHelper
    {

        //private void MyMethod()
        //{
        //    int n = Convert.ToInt32(MySettings.Get(CurBookName, tableName + "CurIndex"));
        //    if (WpNotes.Children.Count > 0)
        //    {
        //        try
        //        {
        //            (WpNotes.Children[n] as UNote).Focus();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            (WpNotes.Children[0] as UNote).Focus();
        //        }

        //        Sv.ScrollToHorizontalOffset((60 * (n + 1)) - Sv.ActualWidth / 2);
        //        Sv.ScrollToVerticalOffset((60 * (n + 1)) - Sv.ActualHeight / 2);
        //    }
        //    this.IsCanSave = false;
        //}

        /// <summary>
        /// 接受鼠标滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ScrollIt(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,

                Source = sender
            };
            scrollviewer.RaiseEvent(eventArg);
        }

        /// <summary>
        /// 根据鼠标滚动左右卷动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ScrollLR(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
            {
                scrollviewer.LineLeft();
                scrollviewer.LineLeft();
                scrollviewer.LineLeft();
            }
            else
            {
                scrollviewer.LineRight();
                scrollviewer.LineRight();
                scrollviewer.LineRight();
            }
            e.Handled = true;
        }


        /// <summary>
        /// 根据鼠标滚动上下卷动
        /// </summary>
        /// <param name="sender"></param>
        public static void ScrollUD(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
            {
                scrollviewer.LineUp();
                scrollviewer.LineUp();
                scrollviewer.LineUp();
            }
            else
            {
                scrollviewer.LineDown();
                scrollviewer.LineDown();
                scrollviewer.LineDown();
            }
            e.Handled = true;
        }
    }
}
