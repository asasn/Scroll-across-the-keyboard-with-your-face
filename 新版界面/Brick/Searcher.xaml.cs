using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using NSMain.Bricks;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using static NSMain.TreeViewPlus.CNodeModule;

namespace RootNS.Brick
{
    /// <summary>
    /// uc_Searcher.xaml 的交互逻辑
    /// </summary>
    public partial class Searcher : UserControl
    {
        public Searcher()
        {
            InitializeComponent();
        }


        public Book CurrentBook { get; set; } = Gval.CurrentBook;
        public Material Material { get; set; } = Gval.MaterialBook;



        /// <summary>
        /// 搜索参数初始化
        /// </summary>
        private void InitSearch()
        {

            //初始化列表框
            ListBoxOfResults.Items.Clear();

            if (CbMaterial.IsChecked == true)
            {
                //搜索资料库
                this.DataContext = CurrentBook;
            }
            else
            {
                //搜索当前书籍
                this.DataContext = Material;
            }
        }

        #region 执行搜索

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //搜索参数初始化
            InitSearch();

            //输入检校（空字符串或者为 * 时退出）
            if (string.IsNullOrEmpty(TbKeyWords.Text.Trim()))
            {
                return;
            }
            //输入检校（正则表达式错误）
            //if (CbRegex.IsChecked == true)
            //{
            //    MatchCollection matchRets;
            //    try
            //    {
            //        matchRets = Regex.Matches(CurrentBook.is, TbKeyWords.Text);
            //    }
            //    catch (Exception)
            //    {
            //        Console.WriteLine("正则表达式错误！");
            //        return null;
            //    }
            //}
        }

        private void TbKeyWords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        #endregion

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxOfResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
