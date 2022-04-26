﻿using RootNS.Helper;
using RootNS.Model;
using RootNS.View;
using RootNS.Workfolw;
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

namespace RootNS.View
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
            Card[] CardBoxs = { Gval.CurrentBook.CardRole, Gval.CurrentBook.CardOther, Gval.CurrentBook.CardWorld };
            foreach (Card rootCard in CardBoxs)
            {
                foreach (Card card in rootCard.ChildNodes)
                {
                    if (card.Title.Equals(TbNew.Text) || card.IsEqualsNickNames(TbNew.Text, card.NickNames))
                    {
                        FunctionPack.ShowMessageBox("该信息卡已经存在\n请换一个名称！");
                        TbNew.Clear();
                        return;
                    }
                }
            }
            Card newCard = new Card() { Title = TbNew.Text };
            (this.DataContext as Card).AddChildNode(newCard);
            EditorHelper.RefreshKeyWordForAllEditor(newCard);
            if (Gval.EditorTabControl.SelectedItem != null)
            {
                EditorHelper.RefreshIsContainFlagForCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as Editorkernel).ThisTextEditor.Text);
            }
            TbNew.Clear();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        public bool LookLessCards;
        private void BtnLookLess_Click(object sender, RoutedEventArgs e)
        {
            if (LookLessCards == true)
            {
                LookLessCards = false;
                (sender as Button).Content = "\ue8c1";
                foreach (Node node in (this.DataContext as Card).ChildNodes)
                {
                    (node as Card).IsShowCard = true;
                }
            }
            else
            {
                LookLessCards = true;
                (sender as Button).Content = "\ue8a3";
                foreach (Node node in (this.DataContext as Card).ChildNodes)
                {
                    (node as Card).IsShowCard = (node as Card).IsContain;
                }
            }
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
                EditorHelper.RefreshKeyWordForAllEditor((sender as Button).DataContext as Card);
                EditorHelper.RefreshIsContainFlagForCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as Editorkernel).ThisTextEditor.Text);
            }
        }
        private void Command_UnDel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((sender as Button).DataContext as Card).IsDel = false;
            EditorHelper.RefreshKeyWordForAllEditor((sender as Button).DataContext as Card);
            EditorHelper.RefreshIsContainFlagForCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as Editorkernel).ThisTextEditor.Text);
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
