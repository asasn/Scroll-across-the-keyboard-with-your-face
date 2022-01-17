using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NSMain.Tools
{
    /// <summary>
    /// MapWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WToolMap : Window
    {
        public WToolMap()
        {
            InitializeComponent();
        }

        private void Btn_MouseMove(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Tag != null && (bool)btn.Tag == true)
            {
                Point offsetPoint = e.GetPosition(this.RectMapImage);
                btn.Margin = new Thickness(offsetPoint.X - btn.ActualWidth / 2, offsetPoint.Y - btn.ActualHeight / 2, 0, 0);
            }
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            btn.Tag = true;
        }

        private void Btn_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            btn.Tag = false;
        }

        private void Map_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RectMapImage.Width = MapImage.ActualWidth;
            RectMapImage.Height = MapImage.ActualHeight;
        }

        private void Rect_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width == 0)
            {
                return;
            }
            double newWidth = (e.NewSize.Width / e.PreviousSize.Width) * BtnLocation.Width;
            double newHeight = (e.NewSize.Width / e.PreviousSize.Width) * BtnLocation.Height;
            double left = (e.NewSize.Width / e.PreviousSize.Width) * BtnLocation.Margin.Left;
            double top = (e.NewSize.Height / e.PreviousSize.Height) * BtnLocation.Margin.Top;
            BtnLocation.Margin = new Thickness(left, top, 0, 0);
            BtnLocation.Width = newWidth;
            BtnLocation.Height = newHeight;
        }
    }
}

