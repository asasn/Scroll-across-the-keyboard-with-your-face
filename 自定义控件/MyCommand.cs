using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 脸滚键盘.自定义控件
{
    public static class MyCommand
    {
        public static RoutedCommand MoveUp = new RoutedCommand("MoveUp", typeof(UcontrolTreeBook));
        public static RoutedCommand MoveDown = new RoutedCommand("MoveDown", typeof(UcontrolTreeBook));
        public static RoutedCommand AddBrotherNode = new RoutedCommand("AddBrotherNode", typeof(UcontrolTreeBook));
        public static RoutedCommand AddChildNode = new RoutedCommand("AddChildNode", typeof(UcontrolTreeBook));
        public static RoutedCommand Import = new RoutedCommand("Import", typeof(UcontrolTreeBook));
    }
}
