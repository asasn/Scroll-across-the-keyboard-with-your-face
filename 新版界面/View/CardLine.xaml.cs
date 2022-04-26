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
    /// Line.xaml 的交互逻辑
    /// </summary>
    public partial class CardLine : UserControl
    {
        public CardLine()
        {
            InitializeComponent();
        }



        public Visibility ShowAddButton
        {
            get { return (Visibility)GetValue(ShowAddButtonProperty); }
            set { SetValue(ShowAddButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowAddButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowAddButtonProperty =
            DependencyProperty.Register("ShowAddButton", typeof(Visibility), typeof(CardLine), new PropertyMetadata(Visibility.Visible));



        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Card.Tip tip = new Card.Tip();
            (this.DataContext as Card.Line).Tips.Add(tip);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((this.DataContext as Card.Line).Parent as Card).CanSave = true;
        }
    }
}
