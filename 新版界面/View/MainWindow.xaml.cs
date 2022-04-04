using RootNS.Behavior;
using RootNS.Brick;
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
            DataEntry.ReadyForBegin();

            CurrentBook.BoxDraft.ChildNodes.Add(new Node() { Title = "测试草稿箱测试草稿箱测试草稿箱测试草稿箱测试草稿箱测试草稿箱" });
            CurrentBook.BoxDraft.ChildNodes[0].IsDir = true;
            CurrentBook.BoxDraft.ChildNodes[0].ChildNodes.Add(new Node() { Title = "测试", WordsCount = 99 });
            Gval.OpenedDocList.Add(new Node() { Title = "打开的章节111111111111111111111111111111111" });
            Gval.OpenedDocList[0].Title = "333333333";
            Gval.OpenedDocList[0].Text = "文章内容";
            Gval.OpenedDocList[0].WordsCount = 20;
            CurrentBook.NoteClues.ChildNodes.Add(new Node() { Title = "线索线索线索线索线索线索线索线索线索" });
            CurrentBook.NoteClues.ChildNodes[0].IsChecked = true;
            CurrentBook.NoteClues.ChildNodes[0].ChildNodes.Add(new Node() { Title = "线索线索线索线索" });
        }



        private void TabBook_Loaded(object sender, RoutedEventArgs e)
        {
            TabBook_SelectionChanged(sender, null);
        }

        private void TabBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentBook.LoadBookPart(sender as TabControl);
        }

        private void TabNote_Loaded(object sender, RoutedEventArgs e)
        {
            TabNote_SelectionChanged(sender, null);
        }

        private void TabNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentBook.LoadBookNote(sender as TabControl);
        }

        private void TabCard_Loaded(object sender, RoutedEventArgs e)
        {
            TabCard_SelectionChanged(sender, null);
        }

        private void TabCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentBook.LoadForCardsBox(sender as TabControl, BookBase.WorkSpace.当前);
        }

        private void TabMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            TabMaterial_SelectionChanged(sender, null);
        }

        private void TabMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Material.LoadForMaterialPart(sender as TabControl);
        }

        private void TabPublicCard_Loaded(object sender, RoutedEventArgs e)
        {
            TabPublicCard_SelectionChanged(sender, null);
        }

        private void TabPublicCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Material.LoadForCardsBox(sender as TabControl, BookBase.WorkSpace.公共);
        }

        private void BtnChoose_Click(object sender, RoutedEventArgs e)
        {
            Node selectedNode = ((TabBook.SelectedItem as TabItem).Content as MyTreeBook).SelectedItem as Node;
            selectedNode.ParentNode.Remove(selectedNode);
        }
    }
}
