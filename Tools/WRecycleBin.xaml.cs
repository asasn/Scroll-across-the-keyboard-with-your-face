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
            CurBookName = GlobalVal.CurrentBook.Name;
        }



        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(WRecycleBin), new PropertyMetadata(null));


    }
}
