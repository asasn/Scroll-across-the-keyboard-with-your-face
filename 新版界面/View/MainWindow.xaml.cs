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
            CurrentBook.OpenDocList.Add(new Node("节点名字"));
            CurrentBook.OpenDocList[0].NodeName = "333333333";
            CurrentBook.OpenDocList[0].Content = "文章内容";
        }

        public Book CurrentBook { get; set; } = Gval.CurrentBook;
        public Material Material { get; set; } = Gval.Material;

        private void TabBook_Loaded(object sender, RoutedEventArgs e)
        {
            TabBook_SelectionChanged(sender, null);
        }

        private void TabBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentBook.LoadForBookPart((sender as TabControl).SelectedIndex);
        }

        private void TabNote_Loaded(object sender, RoutedEventArgs e)
        {
            TabNote_SelectionChanged(sender, null);
        }

        private void TabNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentBook.LoadForBookNote((sender as TabControl).SelectedIndex);
        }

        private void TabCard_Loaded(object sender, RoutedEventArgs e)
        {
            TabCard_SelectionChanged(sender, null);
        }

        private void TabCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentBook.LoadForCardsBox(BookBase.WorkSpace.当前, (sender as TabControl).SelectedIndex);
        }

        private void TabMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            TabMaterial_SelectionChanged(sender, null);
        }

        private void TabMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Material.LoadForMaterialPart((sender as TabControl).SelectedIndex);
        }

        private void TabPublicCard_Loaded(object sender, RoutedEventArgs e)
        {
            TabPublicCard_SelectionChanged(sender, null);
        }

        private void TabPublicCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Material.LoadForCardsBox(BookBase.WorkSpace.公共, (sender as TabControl).SelectedIndex);
        }
    }
}
