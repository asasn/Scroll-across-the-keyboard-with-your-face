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
    public partial class UTipBox : UserControl
    {
        URecords UcRecords;

        /// <summary>
        /// 信息卡文字记录控件
        /// </summary>
        /// <param name="wpParent">父控件容器</param>
        /// <param name="text">填入的内容</param>
        public UTipBox(URecords ucRecords, string text)
        {
            InitializeComponent();
            UcRecords = ucRecords;
            UcRecords.WpMain.Children.Add(this);
            Text = text;
            this.Tag = false;            
        }


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value);}
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(UTipBox), new PropertyMetadata(string.Empty));



        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            //改变标志，向上传递给父控件容器
            this.Tag = true;
            UcRecords.IsCanSave = true;
        }


        private void Uc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                int i = UcRecords.WpMain.Children.IndexOf(this);
                if (i > 0)
                {
                    string temp = (UcRecords.WpMain.Children[i - 1] as UTipBox).Text;
                    (UcRecords.WpMain.Children[i - 1] as UTipBox).Text = this.Text;
                    (UcRecords.WpMain.Children[i] as UTipBox).Text = temp;
                    (UcRecords.WpMain.Children[i - 1] as UTipBox).Tb.Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
            {
                int i = UcRecords.WpMain.Children.IndexOf(this);
                if (i < UcRecords.WpMain.Children.Count - 1)
                {
                    string temp = (UcRecords.WpMain.Children[i + 1] as UTipBox).Text;
                    (UcRecords.WpMain.Children[i + 1] as UTipBox).Text = this.Text;
                    (UcRecords.WpMain.Children[i] as UTipBox).Text = temp;
                    (UcRecords.WpMain.Children[i + 1] as UTipBox).Tb.Focus();
                }
            }
        }


    }
}
