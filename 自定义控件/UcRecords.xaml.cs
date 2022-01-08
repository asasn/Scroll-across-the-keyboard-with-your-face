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

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcRecords.xaml 的交互逻辑
    /// </summary>
    public partial class UcRecords : UserControl
    {
        public UcRecords()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            WpMain.Children.Add(new UcTipBox());
        }
    }
}
