using NSMain.TreeViewPlus;
using System.Windows.Input;

namespace NSMain
{
    public static class MyCommand
    {
        public static RoutedCommand MoveUp = new RoutedCommand("MoveUp", typeof(UTreeViewPlus));
        public static RoutedCommand MoveDown = new RoutedCommand("MoveDown", typeof(UTreeViewPlus));
        public static RoutedCommand AddBrotherNode = new RoutedCommand("AddBrotherNode", typeof(UTreeViewPlus));
        public static RoutedCommand AddChildNode = new RoutedCommand("AddChildNode", typeof(UTreeViewPlus));
        public static RoutedCommand Import = new RoutedCommand("Import", typeof(UTreeViewPlus));
    }
}
