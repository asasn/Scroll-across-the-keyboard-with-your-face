﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// MapWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MapWindow : Window
    {
        public MapWindow()
        {
            InitializeComponent();
        }

        private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Tag != null && (bool)btn.Tag == true)
            {
                Point offsetPoint = e.GetPosition(this.rect);
                btn.Margin = new Thickness(offsetPoint.X - btn.ActualWidth / 2, offsetPoint.Y - btn.ActualHeight / 2, 0, 0);
            }
        }
        private void btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            btn.Tag = true;
        }

        private void btn_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            btn.Tag = false;
        }

        private void map_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            rect.Width = map.ActualWidth;
            rect.Height = map.ActualHeight;
        }

        private void rect_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width == 0)
            {
                return;
            }
            double newWidth = (e.NewSize.Width / e.PreviousSize.Width) * btn.Width;
            double newHeight = (e.NewSize.Width / e.PreviousSize.Width) * btn.Height;
            double left = (e.NewSize.Width / e.PreviousSize.Width) * btn.Margin.Left;
            double top = (e.NewSize.Height / e.PreviousSize.Height) * btn.Margin.Top;
            btn.Margin = new Thickness(left, top, 0, 0);
            btn.Width = newWidth;
            btn.Height = newHeight;
        }
    }
}

