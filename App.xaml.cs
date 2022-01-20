using NSMain.Bricks;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace NSMain
{

    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            RunningCheck();
            GlobalVal.Threads.Task1 = CommonMethod.CreateSplashWindow();

            base.OnStartup(e);
        }

        private void RunningCheck()
        {
            Process thisProc = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                MessageBoxResult dr = MessageBox.Show("程序运行中，是否强制重新运行？\n如非必要，请进行取消！", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                if (dr == MessageBoxResult.OK)
                {
                    foreach (Process p in Process.GetProcessesByName(thisProc.ProcessName))
                    {
                        if (p.Id != thisProc.Id)
                        {
                            //强制Kill其他同名进程
                            p.Kill();
                        }
                    }
                }
                if (dr == MessageBoxResult.Cancel)
                {
                    //本程序结束，不影响其他进程
                    Application.Current.Shutdown();
                }
            }

        }
    }


    /// <summary>
    /// 字符串决定传递不同的元素
    /// </summary>
    public class UItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value.ToString() == "task")
                {
                    return new UItemTask();
                }
                else
                {
                    return new UItemDoc();
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
    /// 布尔值反转
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBoolean : IValueConverter
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
