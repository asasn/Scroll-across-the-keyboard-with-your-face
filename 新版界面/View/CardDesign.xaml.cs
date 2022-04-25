using RootNS.Helper;
using RootNS.Model;
using RootNS.Workfolw;
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
using static RootNS.Helper.DataPer;

namespace RootNS.View
{
    /// <summary>
    /// WCardEdit.xaml 的交互逻辑
    /// </summary>
    public partial class CardDesign : Window
    {
        public CardDesign(object sender, UserControl uc)
        {
            InitializeComponent();
            RootCard.TabName = ((sender as Button).DataContext as Card).TabName;
            RootCard.OwnerName = ((sender as Button).DataContext as Card).OwnerName;
            Card.Line line = new Card.Line() { LineTitle = "设计" };
            RootCard.Lines.Add(line);
            this.DataContext = DataIn.CardDesginLoad(RootCard);

            ViewSet.ForViewPoint(this, uc);

        }
        public Card RootCard { get; set; } = new Card();

        private void ThisWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (Card.Tip tip in (this.DataContext as Card).Lines[0].Tips)
            {
                DataOut.ReplaceIntoCardDesign(tip);
            }
            this.DataContext = DataIn.CardDesginLoad(RootCard);
            RootCard.CanSave = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            //填充信息之后，将保存状态拨回，以实现初始化
            RootCard.CanSave = false;
        }



    }
}
