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




        public string  StrTitle
        {
            get { return (string )GetValue(StrTitleProperty); }
            set { SetValue(StrTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrTitleProperty =
            DependencyProperty.Register("StrTitle", typeof(string ), typeof(UNoteHorizontal), new PropertyMetadata(string.Empty));


    }
}
