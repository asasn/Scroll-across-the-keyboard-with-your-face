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

namespace RootNS.View
{
    /// <summary>
    /// LinesBox.xaml 的交互逻辑
    /// </summary>
    public partial class LinesBox : UserControl
    {
        public LinesBox()
        {
            InitializeComponent();
        }

        public Visibility ShowAddButton
        {
            get { return (Visibility)GetValue(ShowAddButtonProperty); }
            set { SetValue(ShowAddButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowAddButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowAddButtonProperty =
            DependencyProperty.Register("ShowAddButton", typeof(Visibility), typeof(LinesBox), new PropertyMetadata(Visibility.Visible));

    }
}
