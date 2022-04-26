﻿using RootNS.Helper;
using RootNS.View;
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
using RootNS.Workfolw;

namespace RootNS.View
{
    /// <summary>
    /// CardWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CardWindow : Window
    {
        public CardWindow(object sender, UserControl uc)
        {
            InitializeComponent();
            DataIn.LoadCardContent((sender as Button).DataContext as Card);
            this.DataContext = (sender as Button).DataContext as Card;
            ViewSet.ForViewPoint(this, uc);

            //添加拖曳面板事件
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            BtnSave.IsEnabled = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DataIn.LoadCardContent(this.DataContext as Card);
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DataOut.ReplaceIntoCard(this.DataContext as Card);
            DataIn.LoadCardContent(this.DataContext as Card);
            foreach (Card.Line line in (this.DataContext as Card).Lines)
            {
                if (line.LineTitle == "别称")
                {
                    (this.DataContext as Card).NickNames = line;
                    break;
                }
            }
            EditorHelper.RefreshKeyWordForAllEditor(this.DataContext as Card);
            BtnSave.IsEnabled = false;
        }

        bool IsShow = false;
        private void BtnSee_Click(object sender, RoutedEventArgs e)
        {
            if (IsShow == true)
            {
                foreach (Card.Line line in (this.DataContext as Card).Lines)
                {
                    line.HasTip = Convert.ToBoolean(line.Tips.Count);
                }
                IsShow = false;
                ExpandPath.RenderTransform = new RotateTransform(-90);
            }
            else
            {
                foreach (Card.Line line in (this.DataContext as Card).Lines)
                {
                    line.HasTip = true;
                }
                IsShow = true;
                ExpandPath.RenderTransform = new RotateTransform();
            }

        }
    }
}
