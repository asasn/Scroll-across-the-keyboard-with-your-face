using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// UcTipBox.xaml 的交互逻辑
    /// </summary>
    public partial class UcTipBox : UserControl
    {
        WrapPanel WpParent;

        /// <summary>
        /// 信息卡文字记录控件
        /// </summary>
        /// <param name="wpParent">父控件容器</param>
        /// <param name="text">填入的内容</param>
        public UcTipBox(WrapPanel wpParent, string text)
        {
            InitializeComponent();
            WpParent = wpParent;
            WpParent.Children.Add(this);
            Text = text;
            IsTextChange = false;            
        }

        public bool IsTextChange
        {
            get { return (bool)GetValue(IsTextChangeProperty); }
            set { SetValue(IsTextChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTextChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTextChangeProperty =
            DependencyProperty.Register("IsTextChange", typeof(bool), typeof(UcTipBox), new PropertyMetadata(false, null, OnTextChange));

        private static object OnTextChange(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(UcTipBox), new PropertyMetadata(string.Empty));

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsTextChange = true;

            //由于不会写属性变更事件，这里采用了强依赖的摆烂写法
            try
            {
                TextBox tb = sender as TextBox;
                UcTipBox tipBox = (tb.Parent as Grid).Parent as UcTipBox;
                WrapPanel wp = tipBox.Parent as WrapPanel;
                UcRecords record = (wp.Parent as Grid).Parent as UcRecords;
                Grid gCard = record.Parent as Grid;
                (gCard.FindName("BtnSave") as Button).IsEnabled = true;
            }
            catch (Exception)
            {
            }
        }


        private void Uc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                int i = WpParent.Children.IndexOf(this);
                if (i > 0)
                {
                    string temp = (WpParent.Children[i - 1] as UcTipBox).Text;
                    (WpParent.Children[i - 1] as UcTipBox).Text = this.Text;
                    (WpParent.Children[i] as UcTipBox).Text = temp;
                    (WpParent.Children[i - 1] as UcTipBox).Tb.Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
            {
                int i = WpParent.Children.IndexOf(this);
                if (i < WpParent.Children.Count - 1)
                {
                    string temp = (WpParent.Children[i + 1] as UcTipBox).Text;
                    (WpParent.Children[i + 1] as UcTipBox).Text = this.Text;
                    (WpParent.Children[i] as UcTipBox).Text = temp;
                    (WpParent.Children[i + 1] as UcTipBox).Tb.Focus();
                }
            }
        }


    }
}
