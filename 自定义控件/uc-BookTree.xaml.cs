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

namespace 脸滚键盘
{
    /// <summary>
    /// uc_BookTree.xaml 的交互逻辑
    /// </summary>
    public partial class uc_BookTree : UserControl
    {
        public uc_BookTree()
        {
            InitializeComponent();
        }



        public string UcTitle
        {
            get { return (string )GetValue(UcTitleProperty); }
            set { SetValue(UcTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTitleProperty =
            DependencyProperty.Register("UcTitle", typeof(string ), typeof(uc_BookTree), new PropertyMetadata(null));

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tv_Loaded(object sender, RoutedEventArgs e)
        {
            TreeOperate.Show(tv);
        }

        private void btnNewBook_Click(object sender, RoutedEventArgs e)
        {
            TreeOperate.SaveAllBooks(tv);
        }
    }
}
