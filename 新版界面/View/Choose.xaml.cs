﻿using RootNS.Helper;
using RootNS.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RootNS.View
{
    /// <summary>
    /// Choose.xaml 的交互逻辑
    /// </summary>
    public partial class Choose : Window
    {
        public Choose()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (IsBookNameTrue(TbName) == true)
            {
                if (IOHelper.IsFileExists(Gval.Path.Books + "/" + TbName.Text + ".db") == true)
                {
                    MessageBox.Show("该书籍已经存在\n请换一个新书名！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }
                else
                {
                    string oldNameDB = Gval.Path.Books + "/" + Gval.CurrentBook.Name + ".db";
                    string newNameDB = Gval.Path.Books + "/" + TbName.Text + ".db";
                    string oldNameJpg = Gval.Path.Books + "/" + Gval.CurrentBook.Name + ".jpg";
                    string newNameJpg = Gval.Path.Books + "/" + TbName.Text + ".jpg";
                    SqlitetHelper.PoolOperate.Remove(Gval.CurrentBook.Name);
                    SqlitetHelper.PoolOperate.Add(TbName.Text);
                    IOHelper.RenameFile(oldNameDB, newNameDB);
                    IOHelper.RenameFile(oldNameJpg, newNameJpg);
                    //注意处理的先后顺序
                    UpdateCurrentBookInfo();
                    DataOut.UpdateBookInfo(Gval.CurrentBook);
                }
            }
        }

        /// <summary>
        /// 更新当前书籍信息
        /// </summary>
        private void UpdateCurrentBookInfo()
        {
            Gval.CurrentBook.Name = TbName.Text;
            Gval.CurrentBook.Summary = TbSummary.Text;
            Gval.CurrentBook.Price = Convert.ToDouble(TbPrice.Text);
            Gval.CurrentBook.CurrentYear = Convert.ToInt64(TbCurrentYear.Text);
        }

        private void BtnBuild_Click(object sender, RoutedEventArgs e)
        {
            if (IsBookNameTrue(TbBuild) == true)
            {
                if (IOHelper.IsFileExists(Gval.Path.Books + "/" + TbBuild.Text + ".db") == true)
                {
                    MessageBox.Show("该书籍已经存在\n请换一个新书名！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    return;
                }
                else
                {
                    DataOut.CreateNewBook(TbBuild.Text);
                }
            }
        }

        private bool IsBookNameTrue(TextBox tb)
        {
            bool result = false;
            if (hasInvalidChar == false && String.IsNullOrWhiteSpace(tb.Text) == false)
            {
                tb.Background = Brushes.White;
                result = true;
            }
            else
            {
                tb.Background = Brushes.Violet;
                result = false;
            }
            return result;
        }

        private void BtnDelBook_Click(object sender, RoutedEventArgs e)
        {
            DataOut.DeleteBook(Gval.CurrentBook);
            SqlitetHelper.PoolOperate.Remove(Gval.CurrentBook.Name);
            string dbFullName = Gval.Path.Books + "/" + Gval.CurrentBook.Name + ".db";
            IOHelper.DeleteFile(dbFullName);
            Gval.BooksBank.Remove(Gval.CurrentBook);
            Gval.CurrentBook = new Book();
        }

        private void TbBuild_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnBuild.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void TbPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            double.TryParse(tb.Text, out double str);
            tb.Text = str.ToString();
        }


        private void TbCurrentYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            long.TryParse(tb.Text, out long str);
            tb.Text = str.ToString();
        }

        public bool hasInvalidChar { get; set; } = false;

        private void BookName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = (sender as TextBox).Text;
            hasInvalidChar = false;
            foreach (char c in text)
            {
                if (IOHelper.invalidCharsInFileName.Contains(c) || (sender as TextBox).Text.Contains('.') == true)
                {
                    hasInvalidChar = true;
                    break;
                }
            }
            IsBookNameTrue(sender as TextBox);
        }

        Button PreviousButton = new Button();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Gval.CurrentBook != (sender as Button).DataContext as Book)
            {
                Gval.CurrentBook = (sender as Button).DataContext as Book;
                DataIn.LoadCurrentBookContent(Gval.CurrentBook);
                (sender as Button).BorderBrush = null;
                PreviousButton.BorderBrush = null;
                (sender as Button).BorderBrush = Brushes.Orange;
                PreviousButton = sender as Button;
                SettingsHelper.Set("index", "CurBookUid", Gval.CurrentBook.Uid);
            }
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if (((sender as Button).DataContext as Book).Uid == Gval.CurrentBook.Uid)
            {
                (sender as Button).BorderBrush = Brushes.Orange;
                PreviousButton = sender as Button;
            }
        }
    }
}
