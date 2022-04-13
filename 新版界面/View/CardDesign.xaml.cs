﻿using NSMain.Bricks;
using RootNS.Behavior;
using RootNS.Model;
using System;
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
using System.Windows.Shapes;
using static RootNS.Behavior.HelperDataObject;

namespace RootNS.View
{
    /// <summary>
    /// WCardEdit.xaml 的交互逻辑
    /// </summary>
    public partial class CardDesign : Window
    {
        public CardDesign(UserControl uc)
        {
            InitializeComponent();

            this.Left = uc.TranslatePoint(new Point(), Gval.View.MainWindow).X - 5;
            this.Top = 300;

            //添加拖曳面板事件
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };

        }



        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            BtnSave.IsEnabled = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

            //填充信息之后，将保存状态拨回，以实现初始化
            BtnSave.IsEnabled = false;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
