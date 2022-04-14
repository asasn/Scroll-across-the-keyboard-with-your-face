﻿using RootNS.Behavior;
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
            CardWindow cw = new CardWindow(sender , ThisControl);
            cw.ShowDialog();
        }
    }
}
