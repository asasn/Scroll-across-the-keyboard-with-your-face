using RootNS.Behavior;
using RootNS.Brick;
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

        private void BtnReName_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (IsBookNameTrue(TbName) == true)
            {

            }
        }

        private void BtnBuild_Click(object sender, RoutedEventArgs e)
        {
            if (IsBookNameTrue(TbBuild) == true)
            {
                if (CFileOperate.IsFileExists(Gval.Path.Books + "/" + TbBuild.Text + ".db") == true)
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
                if (CFileOperate.invalidCharsInFileName.Contains(c) || (sender as TextBox).Text.Contains('.') == true)
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
                DataJoin.LoadCurrentBookContent(Gval.CurrentBook);
            }
            Gval.CurrentBook = (sender as Button).DataContext as Book;
            PreviousButton.BorderBrush = null;
            (sender as Button).BorderBrush = Brushes.Orange;
            PreviousButton = sender as Button;
            CSettingsOperate.Set("index", "CurBookUid", Gval.CurrentBook.Uid);

        }


    }
}
