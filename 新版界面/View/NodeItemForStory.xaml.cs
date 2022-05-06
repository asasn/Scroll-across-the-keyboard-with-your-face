using RootNS.Helper;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// NodeItemForSecens.xaml 的交互逻辑
    /// </summary>
    public partial class NodeItemForStory : UserControl
    {
        public NodeItemForStory()
        {
            InitializeComponent();
        }

        private void TbReName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (GMian.DataContext as Summary).Node.ReNameing = false;
                e.Handled = true;
            }
        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Node node = (GMian.DataContext as Summary).Node;
            if (node != null)
            {
                node.CheckChildNodes();
                node.CheckParentNodes();
            }
        }




        private void TbReName_Loaded(object sender, RoutedEventArgs e)
        {
            (GMian.DataContext as Summary).CanSave = false;
        }


        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            Gval.CurrentBook.LoadForAllNoteTabs();
            Summary secen = new Summary
            {
                Node = this.DataContext as Node
            };
            if (JsonHelper.JsonToObj<Summary>(secen.Node.Summary) != null)
            {
                secen.Json = JsonHelper.JsonToObj<Summary.JsonData>(secen.Node.Summary);
            }
            //secen.Roles.CollectionChanged += Roles_CollectionChanged;
            secen.Secens.CollectionChanged += Secens_CollectionChanged;

            //foreach (string uid in secen.Json.Roles)
            //{
            //    foreach (Card card in Gval.CurrentBook.CardRole.ChildNodes)
            //    {
            //        if (uid == card.Uid)
            //        {
            //            secen.Roles.Add(card);
            //        }
            //    }
            //}
            foreach (string uid in secen.Json.Secens)
            {
                foreach (Node node in Gval.CurrentBook.GetSecenNodes())
                {
                    if (uid == node.Uid)
                    {
                        secen.Secens.Add(node);
                    }
                }
            }
            secen.Time = secen.Json.Time;
            secen.Place = secen.Json.Place;
            (this.DataContext as Node).Extra = secen;
            secen.Node = this.DataContext as Node;
            GMian.DataContext = secen;
        }

        private void Secens_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            (GMian.DataContext as Summary).Json.Secens.Clear();
            foreach (Node item in (sender as ObservableCollection<object>))
            {
                (GMian.DataContext as Summary).Json.Secens.Add(item.Uid);
            }
            foreach (Node item in (sender as ObservableCollection<object>))
            {
                foreach (Card card in (item.Extra as Summary).Roles)
                {
                    (GMian.DataContext as Summary).Roles.Add(card);
                } 
            }
            (GMian.DataContext as Summary).CanSave = true;
        }

        private void Roles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            (GMian.DataContext as Summary).Json.Roles.Clear();
            foreach (Card item in (sender as ObservableCollection<object>))
            {
                (GMian.DataContext as Summary).Json.Roles.Add(item.Uid);
            }
            (GMian.DataContext as Summary).CanSave = true;
        }

        private void ThisControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((this.DataContext as Node).IsChecked == true)
            {
                return;
            }
            StoryWindow storyWindow = new StoryWindow();
            storyWindow.DataContext = this.DataContext as Node;
            storyWindow.GMian.DataContext = GMian.DataContext;
            Workfolw.ViewSet.ForViewPoint(storyWindow, this, -25, 50);
            storyWindow.ShowDialog();
        }
    }
}
