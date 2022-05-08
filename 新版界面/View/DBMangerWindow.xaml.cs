using RootNS.Helper;
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
    /// DBMangerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DBMangerWindow : Window
    {
        public DBMangerWindow()
        {
            InitializeComponent();
        }

        private void LoadSize(string bookName, Label lbSize)
        {
            System.IO.FileInfo fileInfo = null;
            try
            {
                fileInfo = new System.IO.FileInfo(Gval.Path.Books + "\\" + bookName + ".db");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (fileInfo != null && fileInfo.Exists)
            {
                decimal fileSize = Math.Round(Convert.ToDecimal(fileInfo.Length / 1024.0 / 1024.0), 2, MidpointRounding.AwayFromZero);
                lbSize.Content = string.Format("库大小：{0}mb", fileSize);
            }
        }

        private void Vacuum(string bookName, Label lbSize)
        {
            SqliteHelper.PoolOperate.Add(bookName);
            SqliteHelper.PoolDict[bookName].Vacuum();
            LoadSize(bookName, lbSize);
        }


        private void LbSize_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSize(Gval.CurrentBook.Name, LbSize);
        }

        private void BtnZip_Click(object sender, RoutedEventArgs e)
        {
            Vacuum(Gval.CurrentBook.Name, LbSize);
        }

        private void LbSize1_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSize(Gval.MaterialBook.Name, LbSize1);
        }

        private void BtnZip1_Click(object sender, RoutedEventArgs e)
        {
            Vacuum(Gval.MaterialBook.Name, LbSize1);
        }
    }
}
