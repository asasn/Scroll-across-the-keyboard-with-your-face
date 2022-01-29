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

namespace NSMain.Bricks
{
    /// <summary>
    /// ULocation.xaml 的交互逻辑
    /// </summary>
    public partial class ULocation : UserControl
    {
        public ULocation(Grid mapGrid)
        {
            InitializeComponent();
            MapGrid = mapGrid;
        }





        public string StrTitle
        {
            get { return (string)GetValue(StrTitleProperty); }
            set { SetValue(StrTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrTitleProperty =
            DependencyProperty.Register("StrTitle", typeof(string), typeof(ULocation), new PropertyMetadata(null));





        public string StrContent
        {
            get { return (string)GetValue(StrContentProperty); }
            set { SetValue(StrContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrContentProperty =
            DependencyProperty.Register("StrContent", typeof(string), typeof(ULocation), new PropertyMetadata(null));




        public Grid MapGrid
        {
            get { return (Grid)GetValue(MapGridProperty); }
            set { SetValue(MapGridProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapGrid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapGridProperty =
            DependencyProperty.Register("MapGrid", typeof(Grid), typeof(ULocation), new PropertyMetadata(null));


        bool IsMoving = false;
        private void Btn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsMoving = true;
        }

        private void Btn_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMoving = false;
        }

        private void Btn_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMoving == true)
            {
                Point offsetPoint = e.GetPosition(MapGrid);
                Btn.Margin = new Thickness(offsetPoint.X - Btn.ActualWidth / 2, offsetPoint.Y - Btn.ActualHeight * 0.8, 0, 0);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
