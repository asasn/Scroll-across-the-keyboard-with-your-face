using RootNS.Model;
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

namespace RootNS.View
{
    /// <summary>
    /// CardProperty.xaml 的交互逻辑
    /// </summary>
    public partial class CardProperty : UserControl
    {
        public CardProperty()
        {
            InitializeComponent();
        }



        public string TbContent
        {
            get { return (string)GetValue(TbContentProperty); }
            set { SetValue(TbContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TbContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TbContentProperty =
            DependencyProperty.Register("TbContent", typeof(string), typeof(CardProperty), new PropertyMetadata(null));




        public string LbContent
        {
            get { return (string)GetValue(LbContentProperty); }
            set { SetValue(LbContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LbContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LbContentProperty =
            DependencyProperty.Register("LbContent", typeof(string), typeof(CardProperty), new PropertyMetadata(null));

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((sender as TextBox).DataContext as Card).CanSave = true;
        }
    }
}
