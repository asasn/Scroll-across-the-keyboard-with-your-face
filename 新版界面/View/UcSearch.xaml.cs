using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using RootNS.Helper;
using RootNS.Model;
using RootNS.Workfolw;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace RootNS.View
{
    /// <summary>
    /// uc_Searcher.xaml 的交互逻辑
    /// </summary>
    public partial class UcSearch : UserControl
    {
        public UcSearch()
        {
            InitializeComponent();
        }

        public Searcher ThisSearcher { get; set; } = new Searcher();

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            ThisSearcher.Start();
        }


        private void TbKeyWords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxOfResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Searcher.Item lbItem = (Searcher.Item)ListBoxOfResults.SelectedItem;
            WSearchResult rtWin = new WSearchResult(lbItem);
            rtWin.ShowDialog();
        }

    }
}
