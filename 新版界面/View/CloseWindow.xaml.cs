using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace RootNS.View
{
    /// <summary>
    /// CloseWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CloseWindow : Window
    {
        public CloseWindow(object dataContext, object sender)
        {
            InitializeComponent();
            this.DataContext = dataContext;
            Sender = (sender as Button).DataContext;
            this.Left = (sender as Button).PointToScreen(new Point()).X + (sender as Button).ActualWidth - 5;
            this.Top = (sender as Button).PointToScreen(new Point()).Y - (sender as Button).ActualHeight + 5;
        }


        public object Sender { get; set; }


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as ObservableCollection<object>).Remove(Sender);
            this.Deactivated -= Window_Deactivated;
            this.Close();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
