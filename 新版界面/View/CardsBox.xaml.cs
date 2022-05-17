using RootNS.Helper;
using RootNS.Model;
using RootNS.View;
using RootNS.Workfolw;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace RootNS.View
{
    /// <summary>
    /// CardsBox.xaml 的交互逻辑
    /// </summary>
    public partial class CardsBox : UserControl
    {
        public CardsBox()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbNew.Text) == true)
            {
                return;
            }
            BookBase owner = (this.DataContext as Card).Owner as BookBase;
            owner.LoadForAllCardTabs();
            Card[] CardBoxs = { owner.CardRole, owner.CardOther, owner.CardWorld };
            foreach (Card rootCard in CardBoxs)
            {
                foreach (Card card in rootCard.ChildNodes)
                {
                    if (card.Title.Equals(TbNew.Text) || card.IsEqualsNickNames(TbNew.Text, card.NickNames))
                    {
                        BtnSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        return;
                    }
                }
            }
            Card newCard = new Card() { Title = TbNew.Text };
            (this.DataContext as Card).AddChildNode(newCard);
            TbNew.Clear();
            if (Gval.EditorTabControl.SelectedItem == null || owner.Name == Gval.MaterialBook.Name)
            {
                return;
            }
            EditorHelper.RefreshKeyWordForAllEditor(newCard);
            EditorHelper.RefreshIsContainFlagForAllCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as Editorkernel).ThisTextEditor.Text);


        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string title = TbNew.Text.Trim();
            foreach (Card card in (this.DataContext as Card).ChildNodes)
            {
                if (title.Equals(card.Title) == true || card.IsEqualsNickNames(title, card.NickNames))
                {
                    CardWindow cw = new CardWindow(card);
                    ViewSet.ForViewPointX(cw, this, -6);
                    ViewSet.ForViewPointY(cw, this, 50);
                    cw.ShowDialog();
                    return;
                }
            }
        }

        public bool LookMore;
        private void BtnLookMore_Click(object sender, RoutedEventArgs e)
        {
            if (LookMore == true)
            {
                //屏蔽部分
                LookMore = false;
                (sender as Button).Content = "\ue8a3";
                (sender as Button).ToolTip = "更多";
                foreach (Node node in (this.DataContext as Card).ChildNodes)
                {
                    (node as Card).IsShowCard = (node as Card).IsContain;
                }
            }
            else
            {
                //全部展示
                LookMore = true;
                (sender as Button).Content = "\ue8c1";
                (sender as Button).ToolTip = "更少";
                foreach (Node node in (this.DataContext as Card).ChildNodes)
                {
                    (node as Card).IsShowCard = true;
                }
            }
        }

        private void TbNew_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void BtnDesign_Click(object sender, RoutedEventArgs e)
        {
            CardDesign we = new CardDesign(sender, ThisControl);
            we.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((sender as Button).DataContext as Card).IsDel == true)
            {
                return;
            }
            CardWindow cw = new CardWindow(sender, ThisControl);
            cw.ShowDialog();
        }

        private void ButtonMenu_Opened(object sender, RoutedEventArgs e)
        {

        }

        private void Command_Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (((sender as Button).DataContext as Card).IsDel == true)
            {
                ((sender as Button).DataContext as Card).RemoveThisCard();
            }
            else
            {
                ((sender as Button).DataContext as Card).ChangeDelFlag(true);
            }
            if (Gval.EditorTabControl.SelectedItem == null)
            {
                return;
            }
            BookBase owner = (this.DataContext as Card).Owner as BookBase;
            owner.LoadForAllCardTabs();
            EditorHelper.RefreshKeyWordForAllEditor((sender as Button).DataContext as Card);
            EditorHelper.RefreshIsContainFlagForAllCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as Editorkernel).ThisTextEditor.Text);

        }
        private void Command_UnDel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((sender as Button).DataContext as Card).ChangeDelFlag(false);
            if (Gval.EditorTabControl.SelectedItem == null)
            {
                return;
            }
            BookBase owner = (this.DataContext as Card).Owner as BookBase;
            owner.LoadForAllCardTabs();
            EditorHelper.RefreshKeyWordForAllEditor((sender as Button).DataContext as Card);
            EditorHelper.RefreshIsContainFlagForAllCardsBox(((Gval.EditorTabControl.SelectedItem as HandyControl.Controls.TabItem).Content as Editorkernel).ThisTextEditor.Text);
        }

        private void Button_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Button).Focus();
        }

        private void Button_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (((sender as Button).DataContext as Card).IsShowCard == true && (sender as Button).ToolTip == null)
            {
                (sender as Button).ToolTip = new CardHover((sender as Button).DataContext as Card);
            }
        }

        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            Timer.Tick += TimeRuner;
            Timer.Start();
        }
        private void ThisControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        private DispatcherTimer Timer = new DispatcherTimer();

        /// <summary>
        /// 方法：每次间隔运行的内容
        /// </summary>
        private void TimeRuner(object sender, EventArgs e)
        {
            if (Gval.EditorTabControl.SelectedItem == null)
            {
                return;
            }
            string text = ((Gval.EditorTabControl.SelectedItem as TabItem).Content as Editorkernel).ThisTextEditor.Text;
            EditorHelper.RefreshIsContainFlagForTab(this.DataContext as Card, text);
            EditorHelper.RefreshShowFlagForTab(this.DataContext as Card, LookMore);
        }


    }
}
