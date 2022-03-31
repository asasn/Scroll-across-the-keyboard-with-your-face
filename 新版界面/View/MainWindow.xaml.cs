using RootNS.Model;
using RootNS.Service;
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

namespace RootNS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        public Book CurrentBook { get; set; } = Gval.Books.CurrentBook;

        private void TabBookLoad(object sender)
        {

        }

        private void TabBook_Loaded(object sender, RoutedEventArgs e)
        {
            TabBook_SelectionChanged(sender, null);
        }

        private void TabBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookService Service = new BookService(Gval.Books.CurrentBook, (sender as TabControl).SelectedIndex);
            Service.LoadForTabBook();
        }

        private void TabNote_Loaded(object sender, RoutedEventArgs e)
        {
            TabNote_SelectionChanged(sender, null);
        }

        private void TabNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookService Service = new BookService(Gval.Books.CurrentBook, (sender as TabControl).SelectedIndex);
            Service.LoadForTabNote();
        }

        private void TabCard_Loaded(object sender, RoutedEventArgs e)
        {
            TabCard_SelectionChanged(sender, null);
        }

        private void TabCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookService Service = new BookService(Gval.Books.CurrentBook, (sender as TabControl).SelectedIndex);
            Service.LoadForTabCard();
        }
    }
}
