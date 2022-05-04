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
        public TagsSelectWindow(object datacontext)
        {
            InitializeComponent();
            this.DataContext = datacontext;
        }



        public ObservableCollection<Tags.Tag> All
        {
            get { return (ObservableCollection<Tags.Tag>)GetValue(AllProperty); }
            set { SetValue(AllProperty, value); }
        }

        // Using a DependencyProperty as the backing store for All.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllProperty =
            DependencyProperty.Register("All", typeof(ObservableCollection<Tags.Tag>), typeof(TagsSelectWindow), new PropertyMetadata(new ObservableCollection<Tags.Tag>()));


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            All = DataIn.LoadTags(this.DataContext as Tags);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Tags.Tag tag = new Tags.Tag
            {
                Uid = (sender as Button).Uid,
                Title = (sender as Button).Content.ToString()
            };
            foreach (Tags.Tag t in (this.DataContext as Tags).ChildItems)
            {
                if (t.Uid == tag.Uid)
                {
                    this.Close();
                    return;
                }
            }
            (this.DataContext as Tags).ChildItems.Add(tag);
            this.Close();
        }
    }
}
