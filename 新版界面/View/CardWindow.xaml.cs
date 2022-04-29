using RootNS.Helper;
using RootNS.View;
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
using RootNS.Workfolw;

namespace RootNS.View
{
    /// <summary>
    /// CardWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CardWindow : Window
    {
        public CardWindow(object sender, UserControl uc)
        {
            InitializeComponent();
            this.DataContext = DataIn.LoadCardContent((sender as Button).DataContext as Card);
            ViewSet.ForViewPoint(this, uc);
        }
        public CardWindow(Card card)
        {
            InitializeComponent();
            this.DataContext = DataIn.LoadCardContent(card);
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //添加拖曳面板事件
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnSave.IsEnabled = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            BtnSave.IsEnabled = false;
            (this.DataContext as Card).Title = CpTitle.TbContent.ToString().Trim();
            DataOut.ReplaceIntoCard(this.DataContext as Card);
            DataIn.LoadCardContent(this.DataContext as Card);
            foreach (Card.Line line in (this.DataContext as Card).Lines)
            {
                if (line.LineTitle == "别称")
                {
                    (this.DataContext as Card).NickNames = line;
                    break;
                }
            }
            if (Gval.EditorTabControl.SelectedItem == null || ((this.DataContext as Card).Owner as BookBase).Name == Gval.MaterialBook.Name)
            {
                return;
            }
            EditorHelper.RefreshKeyWordForAllEditor(this.DataContext as Card);
            EditorHelper.RefreshIsContainFlagForCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as Editorkernel).ThisTextEditor.Text);
            
        }

        bool IsShow = false;
        private void BtnSee_Click(object sender, RoutedEventArgs e)
        {
            if (IsShow == true)
            {
                (this.DataContext as Card).ShowYearLine = !string.IsNullOrWhiteSpace((this.DataContext as Card).BornYear);
                foreach (Card.Line line in (this.DataContext as Card).Lines)
                {
                    line.HasTip = Convert.ToBoolean(line.Tips.Count);
                }
                IsShow = false;
                ExpandPath.RenderTransform = new RotateTransform(-90);
            }
            else
            {
                (this.DataContext as Card).ShowYearLine = true;
                foreach (Card.Line line in (this.DataContext as Card).Lines)
                {
                    line.HasTip = true;
                }
                IsShow = true;
                ExpandPath.RenderTransform = new RotateTransform();
            }

        }

    }
}
