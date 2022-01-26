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

namespace NSMain.Bricks
{
    /// <summary>
    /// UNote.xaml 的交互逻辑
    /// </summary>
    public partial class UNote : UserControl
    {
        public UNote()
        {
            InitializeComponent();
        }
        public enum Towards
        {
            Horizontal,
            Vertical
        }


        public int Toward
        {
            get { return (int)GetValue(TowardProperty); }
            set { SetValue(TowardProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Toward.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TowardProperty =
            DependencyProperty.Register("Toward", typeof(int), typeof(UNote), new PropertyMetadata(0));

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(UNote), new PropertyMetadata(0));



        public string StrIndex
        {
            get { return (string)GetValue(StrIndexProperty); }
            set { SetValue(StrIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrIndexProperty =
            DependencyProperty.Register("StrIndex", typeof(string), typeof(UNote), new PropertyMetadata(string.Empty));



        public string StrContent
        {
            get { return (string)GetValue(StrContentProperty); }
            set { SetValue(StrContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrContentProperty =
            DependencyProperty.Register("StrContent", typeof(string), typeof(UNote), new PropertyMetadata(string.Empty));




        public string StrTitle
        {
            get { return (string)GetValue(StrTitleProperty); }
            set { SetValue(StrTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrTitleProperty =
            DependencyProperty.Register("StrTitle", typeof(string), typeof(UNote), new PropertyMetadata(string.Empty));



        private void Uc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //控件获取焦点（子元素已经设置为无法获取焦点）
            this.Focus();
        }



        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Toward == (int)Towards.Horizontal)
            {
                ThisControl.Content = new UNoteHorizontal()
                {
                    Uid = this.Uid,
                    StrIndex = this.StrIndex,
                    StrTitle = this.StrTitle,
                    StrContent = this.StrContent,


                };
            }
            if (Toward == (int)Towards.Vertical)
            {
                ThisControl.Content = new UNoteVertical()
                {
                    Uid = this.Uid,
                    StrIndex = this.StrIndex,
                    StrTitle = this.StrTitle,
                    StrContent = this.StrContent,
                };
            }

            SetBinding(ThisControl, StrIndexProperty, ThisControl.Content, "StrIndex");
            SetBinding(ThisControl, StrTitleProperty, ThisControl.Content, "StrTitle");
            SetBinding(ThisControl, StrContentProperty, ThisControl.Content, "StrContent");
        }

        void SetBinding(UNote origin, DependencyProperty property, object source, string propertyPath)
        {
            //对绑定目标的目标属性进行绑定
            Binding thisBinding = new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyPath),
                Mode = BindingMode.TwoWay
            };
            origin.SetBinding(property, thisBinding);
        }
    }
}
