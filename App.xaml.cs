using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace 脸滚键盘
{

    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
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
