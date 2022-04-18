using RootNS.Behavior;
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

namespace RootNS.Brick
{
    /// <summary>
    /// CardsBox.xaml 的交互逻辑
    /// </summary>
    public partial class CardsBox : UserControl
    {
        public CardsBox()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbNew.Text) == true)
            {
                return;
            }
            Card card = new Card();
            card.Title = TbNew.Text;
            (this.DataContext as Card).AddChildNode(card);
            HelperEditor.RefreshKeyWordForAllEditor(card);
            HelperEditor.RefreshStyleForCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as EditorBase).ThisTextEditor);
            TbNew.Clear();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLookMore_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TbNew_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAdd_Click(null, null);
            }
        }

        private void BtnDesign_Click(object sender, RoutedEventArgs e)
        {
            CardDesign we = new CardDesign(sender, ThisControl);
            we.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((sender as Button).DataContext as Card).IsDel == true)
            {
                return;
            }
            CardWindow cw = new CardWindow(sender, ThisControl);
            cw.ShowDialog();
        }

        private void ButtonMenu_Opened(object sender, RoutedEventArgs e)
        {

        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (((sender as Button).DataContext as Card).IsDel == true)
            {
                ((sender as Button).DataContext as Card).RemoveThisCard();
               }
            else
            {
                ((sender as Button).DataContext as Card).IsDel = true;
                HelperEditor.RefreshKeyWordForAllEditor((sender as Button).DataContext as Card);
                HelperEditor.RefreshStyleForCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as EditorBase).ThisTextEditor);
            }
        }
        private void Command_UnDel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((sender as Button).DataContext as Card).IsDel = false;
            HelperEditor.RefreshKeyWordForAllEditor((sender as Button).DataContext as Card);
            HelperEditor.RefreshStyleForCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as EditorBase).ThisTextEditor);
        }

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Button).Focus();
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
