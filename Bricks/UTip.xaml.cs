using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NSMain.Bricks
{
    /// <summary>
    /// UcTipBox.xaml 的交互逻辑
    /// </summary>
    public partial class UTip : TextBox
    {

        /// <summary>
        /// 信息卡文字记录控件
        /// </summary>
        /// <param name="wpParent">父控件容器</param>
        /// <param name="text">填入的内容</param>
        public UTip(URecord ucRecord, string text)
        {
            InitializeComponent();
            ucRecord.WpMain.Children.Add(this);
            UcRecord = ucRecord;
            Text = text;
            this.Tag = false;
        }





        public URecord UcRecord
        {
            get { return (URecord)GetValue(UcRecordProperty); }
            set { SetValue(UcRecordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcRecord.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcRecordProperty =
            DependencyProperty.Register("UcRecord", typeof(URecord), typeof(UTip), new PropertyMetadata(null));





        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            //改变标志，向上传递给父控件容器
            this.Tag = true;
            UcRecord.IsCanSave = true;
        }


        private void Uc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
                int i = UcRecord.WpMain.Children.IndexOf(this);
                if (i > 0)
                {
                    //因为数据库中相关字段设置了唯一约束，需要多用足两个临时变量来交换，确保交换过程不会冲突
                    string tempPrevious = (UcRecord.WpMain.Children[i - 1] as UTip).Text;
                    string tempThis = (UcRecord.WpMain.Children[i] as UTip).Text;
                    (UcRecord.WpMain.Children[i - 1] as UTip).Text = Guid.NewGuid().ToString();
                    (UcRecord.WpMain.Children[i] as UTip).Text = Guid.NewGuid().ToString();
                    (UcRecord.WpMain.Children[i - 1] as UTip).Text = tempThis;
                    (UcRecord.WpMain.Children[i] as UTip).Text = tempPrevious;                   
                    (UcRecord.WpMain.Children[i - 1] as UTip).Tb.Focus();
                }
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.J)
            {
                int i = UcRecord.WpMain.Children.IndexOf(this);
                if (i < UcRecord.WpMain.Children.Count - 1)
                {
                    string tempNext = (UcRecord.WpMain.Children[i + 1] as UTip).Text;
                    string tempThis = (UcRecord.WpMain.Children[i] as UTip).Text;
                    (UcRecord.WpMain.Children[i + 1] as UTip).Text = Guid.NewGuid().ToString();
                    (UcRecord.WpMain.Children[i] as UTip).Text = Guid.NewGuid().ToString();
                    (UcRecord.WpMain.Children[i + 1] as UTip).Text = tempThis;
                    (UcRecord.WpMain.Children[i] as UTip).Text = tempNext;
                    (UcRecord.WpMain.Children[i + 1] as UTip).Tb.Focus();
                }
            }
        }


    }
}
