using NSMain.Bricks;
using NSMain.Cards;
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
using System.Windows.Shapes;

namespace NSMain.Tools
{
    /// <summary>
    /// WPuzzles.xaml 的交互逻辑
    /// </summary>
    public partial class WPuzzles : Window
    {
        public WPuzzles()
        {
            InitializeComponent();
        }

        private void TabMain_Loaded(object sender, RoutedEventArgs e)
        {
            URecord uRecords = new URecord();
            uRecords.Title = "测试";
            TabItem tabItem = new TabItem();
            tabItem.Header = "测试";
            tabItem.Content = uRecords;
            TabMain.Items.Add(tabItem);
        }
    }
}
