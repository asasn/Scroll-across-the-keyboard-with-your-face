using RootNS.Model;
using RootNS.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RootNS.Converter
{
    /// <summary>
    /// 多路绑定：根据是否目录的布尔值判断图标
    /// </summary>
    public class CustomMultiValueConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null)
            {
                return null;
            }
            try
            {
                if ((bool)values[0] == true)
                {
                    if ((bool)values[1] == true)
                    {
                        return "\ue80e";
                    }
                    else
                    {
                        return "\ue80f";
                    }
                }
                else
                {
                    return "\ue855";
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 从节点中获取悬浮信息
    /// </summary>
    public class GetToolTipInfo : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            Card.Line nickNameLine = (Card.Line)value;
            try
            {
                string content = string.Empty;
                foreach (Card.Tip tip in nickNameLine.Tips)
                {
                    content += tip.Title + " ";
                }
                return content;
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
            return null;
        }
    }

    /// <summary>
    /// TabName决定节点模板
    /// </summary>
    public class NodeConvertToNodeTemplate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return new NodeItemForDoc();
            }
            Node node = (Node)value;
            try
            {
                if (node.TabName == "线索")
                {
                    return new NodeItemForClue();
                }
                if (node.TabName == "场景")
                {
                    return new NodeItemForSecens();
                }
                return new NodeItemForDoc();
            }
            catch
            {
                return new NodeItemForDoc();
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// TabName决定是否显现
    /// </summary>
    public class NodeConvertToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            Node node = (Node)value;
            try
            {
                if (node.TabName == "草稿" || node.TabName == "暂存")
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// TabName决定是否显现
    /// </summary>
    public class TabName2BoolReConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }
            try
            {
                if (value.ToString() == "草稿" || value.ToString() == "暂存")
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return true;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }


    /// <summary>
    /// TabName决定是否显现2
    /// </summary>
    public class NodeConvertToVisibility2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            Node node = (Node)value;
            try
            {
                if (node.TabName == "草稿")
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 布尔值决定是否显现
    /// </summary>
    public class BoolConvertToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            try
            {
                if ((bool)value == true)
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 布尔值决定是否显现
    /// </summary>
    public class BoolConvertToReVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }
            try
            {
                if ((bool)value == true)
                {
                    return Visibility.Collapsed;
                }

                return Visibility.Visible;
            }
            catch
            {
                return Visibility.Visible;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 图标决定字号
    /// </summary>
    public class IconCodeConvertToFontSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return 16;
            }
            try
            {
                //是章节的时候，把图标字体调小，其他默认情况皆为18
                if (value.ToString() == "\ue855")
                {
                    return 16;
                }
                else
                {
                    return 18;
                }
            }
            catch
            {
                return 18;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 删除决定透明度
    /// </summary>
    public class IsDelConvertOpacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return 1;
            }
            try
            {
                //删除为真的时候，进行设置
                if ((bool)value == true)
                {
                    return 0.5;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                return 1;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }


    /// <summary>
    /// 聚焦决定边框
    /// </summary>
    public class BoolConvertToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Brushes.Transparent;
            }
            try
            {
                //是焦点的时候，进行设置
                if ((bool)value == true)
                {
                    return Brushes.LightGoldenrodYellow;
                }
                else
                {
                    return Brushes.Transparent;
                }
            }
            catch
            {
                return Brushes.Transparent;
            }
        }

        //这里只有在TwoWay的时候才有用
        public object ConvertBack(object value, Type targetType, object parameter,
         System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }

    /// <summary>
    /// 类型决定字样
    /// </summary>
    public class TypeConvertToShowText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            try
            {
                //是目录的时候，显示为“章”，否则显示为“字”
                if ((bool)value == true)
                {
                    return "章";
                }
                else
                {
                    return "字";
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
            return null;
        }
    }

    /// <summary>
    /// 多路绑定，统计字数或者章节数
    /// </summary>
    public class BoolConvertorCount : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null)
            {
                return null;
            }
            try
            {
                if ((bool)values[0] == true)
                {
                    return (values[1] as Node).ChildNodes.Count();
                }
                else
                {
                    return (values[1] as Node).WordsCount;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
