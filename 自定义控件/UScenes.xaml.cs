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
    public partial class UScenes : UserControl
    {
        public UScenes()
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
            DependencyProperty.Register("Index", typeof(int), typeof(UScenes), new PropertyMetadata(0));





        public string StrIndex
        {
            get { return (string)GetValue(StrIndexProperty); }
            set { SetValue(StrIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrIndexProperty =
            DependencyProperty.Register("StrIndex", typeof(string), typeof(UScenes), new PropertyMetadata(string.Empty));



        public string StrContent
        {
            get { return (string)GetValue(StrContentProperty); }
            set { SetValue(StrContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrContentProperty =
            DependencyProperty.Register("StrContent", typeof(string), typeof(UScenes), new PropertyMetadata(string.Empty));


        public string StrTitile
        {
            get { return (string)GetValue(StrTitileProperty); }
            set { SetValue(StrTitileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrTitile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrTitileProperty =
            DependencyProperty.Register("StrTitile", typeof(string), typeof(UScenes), new PropertyMetadata(string.Empty));




        private void VerticalDisplay(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            string str = tb.Text;
            string text = "";
            foreach (char c in str)
            {
                if (false == string.IsNullOrWhiteSpace(c.ToString()))
                {
                    text += c.ToString() + "\n";
                }
            }
            tb.Text = text;
            tb.CaretIndex = text.Length;
        }

        private void Uc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //控件获取焦点（子元素已经设置为无法获取焦点）
            this.Focus();
        }
    }
}
