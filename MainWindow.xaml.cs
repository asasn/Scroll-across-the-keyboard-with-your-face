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

namespace 脸滚键盘
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Gval.Current.curBookName = SettingsOperate.GetSettings("curBookName");
            Gval.Current.curBookPath = SettingsOperate.GetSettings("curBookPath");
            Gval.Current.tbPrice = tbPrice;
            Gval.Current.tbBornYear = tbBornYear;
            Gval.Current.tbCurYear = tbCurYear;
            tbPrice_Loaded(null, null);
            tbBornYear_Loaded(null, null);
            tbCurYear_Loaded(null, null);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SqliteOperate.Close();
            Application.Current.Shutdown();
        }

        private void tbPrice_Loaded(object sender, RoutedEventArgs e)
        {
            //初始值为空，才是载入的初始状态，需要更改，其他已经发生改变的情况，不必再更改
            if (true == string.IsNullOrEmpty(tbPrice.Text))
            {
                tbPrice.Text = SettingsOperate.GetSettings("price");
            }
        }

        private void tbBornYear_Loaded(object sender, RoutedEventArgs e)
        {
            //初始值为空，才是载入的初始状态，需要更改，其他已经发生改变的情况，不必再更改
            if (true == string.IsNullOrEmpty(tbBornYear.Text))
            {
                tbBornYear.Text = SettingsOperate.GetSettings("bornYear");
            }
        }

        private void tbCurYear_Loaded(object sender, RoutedEventArgs e)
        {
            //初始值为空，才是载入的初始状态，需要更改，其他已经发生改变的情况，不必再更改
            if (true == string.IsNullOrEmpty(tbCurYear.Text))
            {
                tbCurYear.Text = SettingsOperate.GetSettings("curYear");
            }

        }

        private void tbPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            SettingsOperate.SaveSettings("price", tbPrice.Text);
            EditorOperate.ShowValue(ucEditor.words, ucEditor.lbValue);
        }

        private void tbBornYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            SettingsOperate.SaveSettings("bornYear", tbBornYear.Text);

        }

        private void tbCurYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            SettingsOperate.SaveSettings("curYear", tbCurYear.Text);
        }

        private void btnBookSettings_Click(object sender, RoutedEventArgs e)
        {
            DrawerBottomInContainer.IsOpen = !DrawerBottomInContainer.IsOpen;
            if (true == FileOperate.IsFolderExists(Gval.Current.curBookPath))
            {
                drawerTbk.Text = "当前书籍：" + Gval.Current.curBookName;
            }

        }

        private void InfoCard_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.InfoCard = InfoCard;
        }

        private void ucRoleCard_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.ucRoleCard = ucRoleCard;
        }

        private void ucFactionCard_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.ucFactionCard = ucFactionCard;
        }

        private void ucGoodsCard_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.ucGoodsCard = ucGoodsCard;
        }

        private void ucCommonCard_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.ucCommonCard = ucCommonCard;
        }

        private void ucEditor_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.ucEditor = ucEditor;
        }

        private void ucBookTree_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.ucBookTree = ucBookTree;
        }

        private void ucMaterialTree_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.ucMaterialTree = ucMaterialTree;
        }

        private void ucNote_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.ucNote = ucNote;
        }

        private void ucTask_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.ucTask = ucTask;
        }

        private void winMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(Gval.ucBookTree.DrawerLeftInContainer);

            //if (p.X > Gval.ucBookTree.DrawerLeftInContainer.ActualWidth)
            //{
            //    Gval.ucBookTree.DrawerLeftInContainer.IsOpen = false;
            //}

            Point p2 = Mouse.GetPosition(DrawerBottomInContainer);

            //if (p2.Y < this.Height - DrawerBottomInContainer.Height)
            //{
            //    DrawerBottomInContainer.IsOpen = false;
            //}




        }

    }
}
