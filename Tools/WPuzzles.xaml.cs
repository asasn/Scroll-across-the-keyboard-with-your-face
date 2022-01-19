using NSMain.Bricks;
using NSMain.Cards;
using System;
using System.Collections;
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



        private void Page1_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList wps = new ArrayList();

            wps.Add("历史");
            wps.Add("当代");
            wps.Add("力量体系");
            wps.Add("洲陆");
            wps.Add("阶级");
            wps.Add("度量衡");

            Page1.MyRecords.WpMain_Build(wps);
        }
    }
}
