using RootNS.Behavior;
using RootNS.Brick;
using RootNS.Model;
using RootNS.View;
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

            DataJoin.ReadyForBaseInfo();
        }

        private void WinMain_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.View.MainWindow = this;
        }

        private void WinMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void TabBook_GotFocus(object sender, RoutedEventArgs e)
        {
            Gval.SelectedChapterTab = sender as TabControl;
        }

        private void TabBook_Loaded(object sender, RoutedEventArgs e)
        {
            TabBook_SelectionChanged(sender, null);
        }

        private void TabBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabBook_GotFocus(sender, null);
            Gval.CurrentBook.LoadBookChapters();
        }

        private void TabNote_GotFocus(object sender, RoutedEventArgs e)
        {
            Gval.SelectedNoteTab = sender as TabControl;
        }

        private void TabNote_Loaded(object sender, RoutedEventArgs e)
        {
            TabNote_SelectionChanged(sender, null);
        }

        private void TabNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabNote_GotFocus(sender, null);
            Gval.CurrentBook.LoadBookNotes();
        }
        private void TabCard_GotFocus(object sender, RoutedEventArgs e)
        {
            Gval.SelectedCardTab = sender as TabControl;
        }

        private void TabCard_Loaded(object sender, RoutedEventArgs e)
        {
            TabCard_SelectionChanged(sender, null);
        }

        private void TabCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabCard_GotFocus(sender, null);
            Gval.CurrentBook.LoadForCards(Gval.SelectedCardTab);
        }

        private void TabMaterial_GotFocus(object sender, RoutedEventArgs e)
        {
            Gval.SelectedMaterialTab = sender as TabControl;
        }

        private void TabMaterial_Loaded(object sender, RoutedEventArgs e)
        {
            TabMaterial_SelectionChanged(sender, null);
        }

        private void TabMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabMaterial_GotFocus(sender, null);
            Gval.MaterialBook.LoadForMaterialPart();
        }

        private void TabPublicCard_GotFocus(object sender, RoutedEventArgs e)
        {
            Gval.SelectedPublicCardTab = sender as TabControl;
        }

        private void TabPublicCard_Loaded(object sender, RoutedEventArgs e)
        {
            TabPublicCard_SelectionChanged(sender, null);
        }

        private void TabPublicCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabPublicCard_GotFocus(sender, null);
            Gval.MaterialBook.LoadForCards(Gval.SelectedPublicCardTab);
        }

        private void BtnChoose_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Choose();
            win.ShowDialog();
        }

        /// <summary>
        /// 内容渲染完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinMain_ContentRendered(object sender, EventArgs e)
        {
            Gval.FlagLoadingCompleted = true;
        }

        private void BtnCardModel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
