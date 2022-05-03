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
                if (node.TabName == Book.NoteTabName.线索.ToString())
                {
                    return new NodeItemForClue();
                }
                if (node.TabName == Book.NoteTabName.场景.ToString() && node.IsDir == false)
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
    /// TabName决定按钮是否显现
    /// </summary>
    public class TabName2AddFolderButtonIsEnabled : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }
            try
            {
                if (value.ToString() == Book.ChapterTabName.草稿.ToString() || 
                    value.ToString() == Book.ChapterTabName.暂存.ToString())
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
    /// TabName决定按钮是否显现
    /// </summary>
    public class TabName2ImportExportButtonVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            try
            {
                if (value.ToString() == Book.ChapterTabName.草稿.ToString() ||
                    value.ToString() == Book.ChapterTabName.暂存.ToString() ||
                    value.ToString() == Book.ChapterTabName.已发布.ToString() ||
                    value.ToString() == Material.MaterialTabName.范文.ToString() ||
                    value.ToString() == Material.MaterialTabName.资料.ToString())
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
    /// TabName决定按钮是否显现
    /// </summary>
    public class TabName2KeepButtonVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            try
            {
                if (value.ToString() == Book.ChapterTabName.草稿.ToString())
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
    /// TabName决定按钮是否显现
    /// </summary>
    public class TabName2SendButtonVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            try
            {
                if (value.ToString() == Book.ChapterTabName.草稿.ToString() ||
                    value.ToString() == Book.ChapterTabName.暂存.ToString())
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
                    return 15;
                }
                else
                {
                    return 16;
                }
            }
            catch
            {
                return 16;
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
