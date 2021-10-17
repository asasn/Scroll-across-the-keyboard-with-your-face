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

namespace 脸滚键盘
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Gval.MainWindow.tbPrice = tbPrice;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SqliteOperate.Close();
            Application.Current.Shutdown();
        }

        private void tbPrice_Loaded(object sender, RoutedEventArgs e)
        {
            string price = SettingsOperate.GetSettings("price");
            tbPrice.Text = price;
        }

        private void tbPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            SettingsOperate.SaveSettings("price", tbPrice.Text);
            EditorOperate.ShowValue(Editor.words, Editor.lbValue);
        }


    }
}
