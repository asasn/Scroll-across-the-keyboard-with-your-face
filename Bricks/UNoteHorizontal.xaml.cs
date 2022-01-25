using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NSMain.Bricks
{
    /// <summary>
    /// UcScenesCard.xaml 的交互逻辑
    /// </summary>
    public partial class UNoteHorizontal : UserControl
    {
        public UNoteHorizontal()
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
            DependencyProperty.Register("Index", typeof(int), typeof(UNoteHorizontal), new PropertyMetadata(0));





        public string StrIndex
        {
            get { return (string)GetValue(StrIndexProperty); }
            set { SetValue(StrIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrIndexProperty =
            DependencyProperty.Register("StrIndex", typeof(string), typeof(UNoteHorizontal), new PropertyMetadata(string.Empty));



        public string StrContent
        {
            get { return (string)GetValue(StrContentProperty); }
            set { SetValue(StrContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrContentProperty =
            DependencyProperty.Register("StrContent", typeof(string), typeof(UNoteHorizontal), new PropertyMetadata(string.Empty));


        public string StrTitile
        {
            get { return (string)GetValue(StrTitileProperty); }
            set { SetValue(StrTitileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrTitile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrTitileProperty =
            DependencyProperty.Register("StrTitile", typeof(string), typeof(UNoteHorizontal), new PropertyMetadata(string.Empty));


        private void Uc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //控件获取焦点（子元素已经设置为无法获取焦点）
            this.Focus();
        }
    }
}
