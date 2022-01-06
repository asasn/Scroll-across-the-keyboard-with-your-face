using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using 脸滚键盘.公共操作类;

namespace 脸滚键盘
{

    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ////根据图片路径，实例化启动图片
            SplashScreen splashScreen = new SplashScreen("/Resourses/startup.jpg");
            splashScreen.Show(autoClose: true, topMost: true);

            ////Show()方法中设置为true时，程序启动完成后启动图片就会自动关闭，
            ////设置为false时，启动图片不会自动关闭，需要使用下面一句设置显示时间，例如5s
            ////splashScreen.Close(new TimeSpan(0, 0, 5));

            base.OnStartup(e);
        }

        //await RunNewWindowAsync<SplashWindow>(); //可异步等待
        private Task RunNewWindowAsync<TWindow>() where TWindow : System.Windows.Window, new()
        {
            TaskCompletionSource<object> tc = new TaskCompletionSource<object>();
            // 新线程
            Thread t = new Thread(() =>
            {
                TWindow win = new TWindow();
                if (win is SplashWindow)
                    Gval.Uc.SpWin = win as SplashWindow;
                win.Closed += (d, k) =>
                {
                    // 当窗口关闭后马上结束消息循环
                    System.Windows.Threading.Dispatcher.ExitAllFrames();
                };
                win.Show();
                // Run 方法必须调用，否则窗口一打开就会关闭
                // 因为没有启动消息循环
                System.Windows.Threading.Dispatcher.Run();
                // 这句话是必须的，设置Task的运算结果
                // 但由于此处不需要结果，故用null
                tc.SetResult(null);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            Gval.Threads.Task1 = t;
            // 新线程启动后，将Task实例返回
            // 以便支持 await 操作符
            return tc.Task;
        }

    }



    /// <summary>
    /// 布尔值反转
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
             System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    /// <summary>
    /// 布尔值决定颜色
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if ((bool)value == true)
                {
                    return Brushes.Silver;
                }
                else
                {
                    //(Brush)new BrushConverter().ConvertFromString("#FF212121")
                    return Brushes.Black;
                }
            }
            catch
            {
                return null;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if ((value as TextBlock).Foreground == Brushes.Silver)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }


    /// <summary>
    /// 布尔值决定显示/隐藏
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if ((bool)value == false)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
            catch
            {
                return null;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if ((value as TextBlock).Visibility == Visibility.Visible)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }
    }
}
