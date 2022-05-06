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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RootNS.View
{
    /// <summary>
    /// TagsBox.xaml 的交互逻辑
    /// </summary>
    public partial class TagsBoxView : UserControl
    {
        public TagsBoxView()
        {
            InitializeComponent();
        }



        public string BoxTitle
        {
            get { return (string)GetValue(BoxTitleProperty); }
            set { SetValue(BoxTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoxTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxTitleProperty =
            DependencyProperty.Register("BoxTitle", typeof(string), typeof(TagsBoxView), new PropertyMetadata(null));



        public int MaxChilds
        {
            get { return (int)GetValue(MaxChildsProperty); }
            set { SetValue(MaxChildsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxChilds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxChildsProperty =
            DependencyProperty.Register("MaxChilds", typeof(int), typeof(TagsBoxView), new PropertyMetadata(1));



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            TagsSelectWindow selectWindow = new TagsSelectWindow(this.DataContext, BoxTitle);
            Workfolw.ViewSet.ForViewPointX(selectWindow, this, -25);
            Workfolw.ViewSet.ForViewPointY(selectWindow, this, 50);
            selectWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow closeWindow = new CloseWindow(this.DataContext, sender);
            closeWindow.Show();
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if ((this.DataContext as ObservableCollection<object>).Count >= MaxChilds)
            {
                BtnAdd.IsEnabled = false;
            }
            else
            {
                BtnAdd.IsEnabled = true;
            }
        }

        private void Button_Unloaded(object sender, RoutedEventArgs e)
        {
            if ((this.DataContext as ObservableCollection<object>).Count >= MaxChilds)
            {
                BtnAdd.IsEnabled = false;
            }
            else
            {
                BtnAdd.IsEnabled = true;
            }
        }
    }
}
