using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 脸滚键盘.自定义控件
{
    class MyCommand
    {
        public static RoutedCommand AddBrotherNode = new RoutedCommand("AddBrotherNode", typeof(UcTreeBook));
        public static RoutedCommand AddChildNode = new RoutedCommand("AddChildNode", typeof(UcTreeBook));
        public static RoutedCommand Import = new RoutedCommand("Import", typeof(UcTreeBook));
    }
}
