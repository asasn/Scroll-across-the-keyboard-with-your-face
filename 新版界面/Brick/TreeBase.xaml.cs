using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RootNS.Brick
{
    /// <summary>
    /// TreeBase.xaml 的交互逻辑
    /// </summary>
    public partial class TreeBase : TreeBaseBase
    {
        public TreeBase()
        {
            InitializeComponent();
        }

        private void TreeViewMenu_Opened(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("open");
        }

        private void Command_AddBrotherNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("add");
        }

        private void Command_AddChildNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("addchild");
        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("delete");
        }

        private void Command_Import_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("import");
        }
    }
}
