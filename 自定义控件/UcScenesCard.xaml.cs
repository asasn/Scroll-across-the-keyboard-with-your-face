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
    /// UcScenesCard.xaml 的交互逻辑
    /// </summary>
    public partial class UcScenesCard : UserControl
    {
        public UcScenesCard()
        {
            InitializeComponent();
            
        }




        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(UcScenesCard), new PropertyMetadata(0));



        public string StrContent
        {
            get { return (string)GetValue(StrContentProperty); }
            set { SetValue(StrContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrContentProperty =
            DependencyProperty.Register("StrContent", typeof(string), typeof(UcScenesCard), new PropertyMetadata(string.Empty));


        public string StrTitile
        {
            get { return (string)GetValue(StrTitileProperty); }
            set { SetValue(StrTitileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrTitile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrTitileProperty =
            DependencyProperty.Register("StrTitile", typeof(string), typeof(UcScenesCard), new PropertyMetadata(string.Empty));



        public string StrName
        {
            get { return (string)GetValue(StrNameProperty); }
            set { SetValue(StrNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrNameProperty =
            DependencyProperty.Register("StrName", typeof(string), typeof(UcScenesCard), new PropertyMetadata(string.Empty));




        private void Uc_GotFocus(object sender, RoutedEventArgs e)
        {
            BorderBrush = Brushes.Orange;
            this.BorderThickness = new Thickness(2, 2, 2, 2);
        }

        private void Uc_LostFocus(object sender, RoutedEventArgs e)
        {
            BorderBrush = null;
            this.BorderThickness = new Thickness(0, 0, 0, 0);
        }

        private void VerticalDisplay(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            string str = tb.Text;
            string ret = "";
            foreach (char c in str)
            {
                if (false == string.IsNullOrWhiteSpace(c.ToString()))
                {
                    ret += c.ToString() +"\n";
                }
            }
            tb.Text = ret;
            tb.CaretIndex = ret.Length;
        }
    }
}
