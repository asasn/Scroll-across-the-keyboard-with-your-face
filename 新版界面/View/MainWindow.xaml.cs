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

        public Book CurrentBook { get; set; } = Gval.CurrentBook;
        public Material Material { get; set; } = Gval.Material;
        public ObservableCollection<Node> OpenDocList { get; set; } = Gval.OpenedDocList;

        private void WinMain_Loaded(object sender, RoutedEventArgs e)
        {
            
            CurrentBook.BoxDraft.Add(new Node("测试草稿箱测试草稿箱测试草稿箱测试草稿箱测试草稿箱测试草稿箱"));
            CurrentBook.BoxDraft[0].IsDir = true;
            CurrentBook.BoxDraft[0].ChildNodes.Add(new Node("测试") { WordsCount = 99 });
            Gval.OpenedDocList.Add(new Node("打开的章节111111111111111111111111111111111"));
            Gval.OpenedDocList[0].NodeName = "333333333";
            Gval.OpenedDocList[0].Content = "文章内容";
            Gval.OpenedDocList[0].WordsCount = 20;
            CurrentBook.NoteClues.Add(new Node("节点名字111111111111111111111111111111111111111"));
            CurrentBook.NoteClues[0].IsChecked = true;
            CurrentBook.NoteClues[0].ChildNodes.Add(new Node("节点名字"));
        }



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

        private void BtnChoose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
