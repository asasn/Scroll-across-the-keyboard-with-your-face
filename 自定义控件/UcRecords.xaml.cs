﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcRecords.xaml 的交互逻辑
    /// </summary>
    public partial class UcRecords : UserControl
    {
        public UcRecords()
        {
            InitializeComponent();
        }


        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(UcRecords), new PropertyMetadata(string.Empty));





        public bool IsCanSave
        {
            get { return (bool)GetValue(IsCanSaveProperty); }
            set { SetValue(IsCanSaveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCanSave.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCanSaveProperty =
            DependencyProperty.Register("IsCanSave", typeof(bool), typeof(UcRecords), new PropertyMetadata(false));




        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            UcTipBox tipBox = new UcTipBox(WpMain, null); 
        }



        private void Uc_Loaded(object sender, RoutedEventArgs e)
        {
            LbName.Content = Title + "：";
        }
    }
}
