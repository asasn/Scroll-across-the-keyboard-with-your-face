using NSMain.Bricks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
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
    /// WRecycleBin.xaml 的交互逻辑
    /// </summary>
    public partial class WRecycleBin : Window
    {
        public WRecycleBin()
        {
            InitializeComponent();
        }

        private void Udl1_Loaded(object sender, RoutedEventArgs e)
        {
            UDelList udl = sender as UDelList;
            udl.Udl1_Loaded();
        }

        private void Udl2_Loaded(object sender, RoutedEventArgs e)
        {
            UDelList udl = sender as UDelList;
            udl.Udl2_Loaded();
        }
    }
}
