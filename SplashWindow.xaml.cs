using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using 脸滚键盘.公共操作类;

namespace 脸滚键盘
{
    /// <summary>
    /// SplashWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            AngleTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            AngleTimer.Tick += Timer_Tick;
            Gval.Flag.Loading = true;
        }

        Stopwatch StopWatch = new Stopwatch();
        public DispatcherTimer AngleTimer = new DispatcherTimer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AngleTimer.Start();
            StopWatch.Start();
            AngleImg.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            StopWatch.Stop();
            AngleTimer.Stop();
            AngleTimer.Tick -= Timer_Tick;
        }

        public double Angle;
        public double ImgWidth = 0;
        public void Timer_Tick(object sender, EventArgs e)
        {
            Angle += 7.2;
            AngleImg.RenderTransform = new RotateTransform(Angle);
            ImgWidth += 3;
            AngleImg.Width = AngleImg.Height = ImgWidth;
            if (Gval.Flag.Loading == false)
            {
                AngleImg.Opacity -= 0.02;
            }
            if (AngleImg.Opacity <= 0)
            {
                StopWatch.Stop();
                AngleTimer.Stop();
                AngleTimer.Tick -= Timer_Tick;
                AngleImg.Visibility = Visibility.Hidden;
                AngleImg.Opacity = 1;                
            }
        }


    }
}
