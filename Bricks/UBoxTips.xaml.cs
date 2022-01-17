using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NSMain.Bricks
{
    /// <summary>
    /// UcTipBox.xaml 的交互逻辑
    /// </summary>
    public partial class UBoxTips : UserControl
    {
        UBoxRecords UcRecords;

        /// <summary>
        /// 信息卡文字记录控件
        /// </summary>
        /// <param name="wpParent">父控件容器</param>
        /// <param name="text">填入的内容</param>
        public UBoxTips(UBoxRecords ucRecords, string text)
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
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(UBoxTips), new PropertyMetadata(string.Empty));



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
                    string temp = (UcRecords.WpMain.Children[i - 1] as UBoxTips).Text;
                    (UcRecords.WpMain.Children[i - 1] as UBoxTips).Text = this.Text;
                    (UcRecords.WpMain.Children[i] as UBoxTips).Text = temp;
                    (UcRecords.WpMain.Children[i - 1] as UBoxTips).Tb.Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
            {
                int i = UcRecords.WpMain.Children.IndexOf(this);
                if (i < UcRecords.WpMain.Children.Count - 1)
                {
                    string temp = (UcRecords.WpMain.Children[i + 1] as UBoxTips).Text;
                    (UcRecords.WpMain.Children[i + 1] as UBoxTips).Text = this.Text;
                    (UcRecords.WpMain.Children[i] as UBoxTips).Text = temp;
                    (UcRecords.WpMain.Children[i + 1] as UBoxTips).Tb.Focus();
                }
            }
        }


    }
}
