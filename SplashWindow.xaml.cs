using NSMain.Bricks;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace NSMain
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
            GlobalVal.Flag.Loading = true;
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
            if (GlobalVal.Flag.Loading == false)
            {
                AngleImg.Opacity -= 0.025;
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
