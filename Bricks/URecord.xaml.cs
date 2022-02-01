using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NSMain.Bricks
{
    /// <summary>
    /// URecord.xaml 的交互逻辑
    /// </summary>
    public partial class URecord : UserControl
    {
        public URecord()
        {
            InitializeComponent();
        }



        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(URecord), new PropertyMetadata(string.Empty));

        public bool IsCanSave
        {
            get { return (bool)GetValue(IsCanSaveProperty); }
            set { SetValue(IsCanSaveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCanSave.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCanSaveProperty =
            DependencyProperty.Register("IsCanSave", typeof(bool), typeof(URecord), new PropertyMetadata(false));


        internal void SetBinding(bool isCanSave, Binding boolBinding)
        {
            this.SetBinding(IsCanSaveProperty, boolBinding);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            UTip tipBox = new UTip(this, null);
        }

        private void Uc_Loaded(object sender, RoutedEventArgs e)
        {
            LbName.Content = Title;
        }

        private void LbName_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string content = string.Empty;
            foreach (UTip uTip in WpMain.Children)
            {
                content += uTip.Text + "\n";
            }
            Clipboard.SetText(content);
            HandyControl.Controls.Growl.SuccessGlobal("已复制Record内容到剪贴板！");
        }
    }
}
