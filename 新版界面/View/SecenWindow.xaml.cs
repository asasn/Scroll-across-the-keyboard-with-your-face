﻿using RootNS.Helper;
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
using System.Windows.Shapes;

namespace RootNS.View
{
    /// <summary>
    /// SecenWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SecenWindow : Window
    {
        public SecenWindow()
        {
            InitializeComponent();
        }
        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //添加拖曳面板事件
            DragMove();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (GMian.DataContext as Summary).CanSave = false;
        }
        private void BoxContent_Loaded(object sender, RoutedEventArgs e)
        {
            (GMian.DataContext as Summary).Node.IsExpanded = !string.IsNullOrWhiteSpace(TbContent.Text);
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveSecen(GMian.DataContext as Summary, this.DataContext as Node);

            foreach (Node linkNode in Gval.CurrentBook.GetSecenNodes())
            {
                if ((linkNode.Extra as Summary).CanSave == true)
                {
                    SaveSecen(linkNode.Extra as Summary, linkNode);
                }
            }

        }

        private void SaveSecen(Summary secen, Node node)
        {
            string json = JsonHelper.ObjToJson(secen.Json);
            DataOut.UpdateNodeProperty(secen.Node, nameof(Node.Text), secen.Node.Text);
            DataOut.UpdateNodeProperty(secen.Node, nameof(Node.Summary), json);
            secen.CanSave = false;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (GMian.DataContext as Summary).CanSave = true;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = DataIn.LoadNodeContent(this.DataContext as Node);
            this.Close();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            (GMian.DataContext as Summary).CanSave = true;
            (GMian.DataContext as Summary).Json.Time = (sender as TextBox).Text;
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            (GMian.DataContext as Summary).CanSave = true;
            (GMian.DataContext as Summary).Json.Place = (sender as TextBox).Text;
        }
    }
}
