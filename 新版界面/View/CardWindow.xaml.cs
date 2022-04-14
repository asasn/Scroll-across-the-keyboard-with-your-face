using RootNS.Behavior;
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
using System.Windows.Shapes;

namespace RootNS.View
{
    /// <summary>
    /// CardWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CardWindow : Window
    {
        public CardWindow(object sender, UserControl uc)
        {
            InitializeComponent();
            this.DataContext = (sender as Button).DataContext;
            this.Left = uc.TranslatePoint(new Point(), Gval.View.MainWindow).X - 5;
            this.Top = 300;

            //添加拖曳面板事件
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            BtnSave.IsEnabled = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            BtnSave.IsEnabled = false;
        }
    }
}
