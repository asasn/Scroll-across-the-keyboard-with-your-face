﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
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
using Version4.Model;

namespace Version4.View
{
    /// <summary>
    /// Choose.xaml 的交互逻辑
    /// </summary>
    public partial class BookChooseWindow : Window
    {
        public BookChooseWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
           
        }


        private void BtnBuild_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void BtnDelBook_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TbBuild_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void TbPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
        }


        private void TbCurrentYear_TextChanged(object sender, TextChangedEventArgs e)
        {
        }


        private void BookName_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VM.SelectBook((sender as Button).DataContext as Book);
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
