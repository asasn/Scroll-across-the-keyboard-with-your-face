﻿using RootNS.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RootNS.Brick
{
    /// <summary>
    /// NodeItemForDoc.xaml 的交互逻辑
    /// </summary>
    public partial class NodeItemForDoc : UserControl
    {
        public NodeItemForDoc()
        {
            InitializeComponent();
        }

        private void ThisControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Node node = this.DataContext as Node;
            if (node.IsDir == false && node.IsDel == false)
            {
                if (Gval.OpeningDocList.Contains(node) != true)
                {
                    Gval.OpeningDocList.Add(node);
                }
                Gval.CurrentDoc = node;
                foreach (HandyControl.Controls.TabItem item in Gval.EditorTabControl.Items)
                {
                    if (item.Uid == node.Uid)
                    {
                        item.IsSelected = true;
                        break;
                    }
                }
            }
        }

        private void ThisControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as Node).IsSelected = true;
        }

        private void TbReName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (this.DataContext as Node).ReNameing = false;
                e.Handled = true;
            }
        }

        private void TbReName_LostFocus(object sender, RoutedEventArgs e)
        {
            (this.DataContext as Node).ReNameing = false;
        }
    }
}
