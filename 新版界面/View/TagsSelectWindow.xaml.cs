using RootNS.Helper;
using RootNS.Model;
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
using System.Windows.Shapes;

namespace RootNS.View
{
    /// <summary>
    /// TagsSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TagsSelectWindow : Window
    {
        public TagsSelectWindow(object datacontext, string boxTitle)
        {
            InitializeComponent();
            this.DataContext = datacontext;
            BoxTitle = boxTitle;
        }





        public string BoxTitle
        {
            get { return (string)GetValue(BoxTitleProperty); }
            set { SetValue(BoxTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoxTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxTitleProperty =
            DependencyProperty.Register("BoxTitle", typeof(string), typeof(TagsSelectWindow), new PropertyMetadata(null));



        public ObservableCollection<object> All
        {
            get { return (ObservableCollection<object>)GetValue(AllProperty); }
            set { SetValue(AllProperty, value); }
        }

        // Using a DependencyProperty as the backing store for All.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllProperty =
            DependencyProperty.Register("All", typeof(ObservableCollection<object>), typeof(TagsSelectWindow), new PropertyMetadata(new ObservableCollection<object>()));


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            All.Clear();
            if (BoxTitle == "角色")
            {
                foreach (Card card in Gval.CurrentBook.CardRole.ChildNodes)
                {
                    All.Add(card);
                }
            }
            if (BoxTitle == "前因")
            {
                foreach (Node node in Gval.CurrentBook.GetSecenNodes())
                {
                    All.Add(node);
                }
            }
            if (BoxTitle == "后果")
            {
                foreach (Node node in Gval.CurrentBook.GetSecenNodes())
                {
                    All.Add(node);
                }
            }
            if (BoxTitle == "场景")
            {
                foreach (Node node in Gval.CurrentBook.GetSecenNodes())
                {
                    All.Add(node);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<object> c = this.DataContext as ObservableCollection<object>;
            if (c.Contains((sender as Button).DataContext) == false)
            {
                c.Add((sender as Button).DataContext);

                //Node linkNode = (sender as Button).DataContext as Node;
                //if (BoxTitle == "前因")
                //{
                //    if ((linkNode.Extra as Summary).Result.Contains((linkNode.Owner as BookBase).SelectedNode) == false)
                //    {
                //        (linkNode.Extra as Summary).Result.Add((linkNode.Owner as BookBase).SelectedNode);
                //    }
                    
                //}
                //if (BoxTitle == "后果")
                //{
                //    if ((linkNode.Extra as Summary).Origin.Contains((linkNode.Owner as BookBase).SelectedNode) == false)
                //    {
                //        (linkNode.Extra as Summary).Origin.Add((linkNode.Owner as BookBase).SelectedNode);
                //    }
                //}
            }
            this.Close();

        }
    }
}
