using Microsoft.Win32;
using NSMain.Bricks;
using System;
using System.Collections;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NSMain.Tools
{
    /// <summary>
    /// MapWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WToolMap : Window
    {
        public WToolMap(string curBookName, string typeOfTree)
        {
            InitializeComponent();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
        }

        ArrayList BtnLocations = new ArrayList();
        double Scale = 1;
        double OriginWidth;
        double OriginHeight;

        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(WToolMap), new PropertyMetadata(null));



        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(WToolMap), new PropertyMetadata(null));


        /// <summary>
        /// 设置当前地图
        /// </summary>
        /// <param name="imgPath"></param>
        void SetCurMap(string imgPath)
        {
            LbImagePath.Content = imgPath;
            MapImage.Source = CFileOperate.GetImgObject(imgPath);
        }

        Button CreateLocation(double pointX, double pointY)
        {
            //为了免除各种麻烦，这里不进行最大值最小值的设定，一概随缩放比率增大缩小
            double sideLength = 24.0;
            Button btnLocation = new Button();
            btnLocation.Padding = new Thickness(0);
            btnLocation.Background = Brushes.Transparent;
            btnLocation.Width = btnLocation.Height = sideLength * Scale;
            btnLocation.BorderThickness = new Thickness(0);
            Image image = new Image
            {
                Source = CFileOperate.GetImgObject(GlobalVal.Path.Resourses + "/图标/工具栏/ic_action_location.png"),
            };
            btnLocation.Content = image;
            btnLocation.Click += BtnLocation_Click;
            btnLocation.MouseRightButtonDown += BtnLocation_MouseRightButtonDown;
            btnLocation.MouseRightButtonUp += BtnLocation_MouseRightButtonUp;
            btnLocation.MouseMove += BtnLocation_MouseMove;
            btnLocation.HorizontalAlignment = HorizontalAlignment.Left;
            btnLocation.VerticalAlignment = VerticalAlignment.Top;
            btnLocation.Margin = new Thickness(pointX - (btnLocation.Width / 2), pointY - (btnLocation.Height * 0.8), 0, 0);
            BtnLocations.Add(btnLocation);
            MapGrid.Children.Add(btnLocation);
            return btnLocation;
        }


        private void MapGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width == 0)
            {
                return;
            }
            if (OriginWidth == 0 && OriginHeight == 0)
            {
                OriginWidth = e.PreviousSize.Width;
                OriginHeight = e.PreviousSize.Height;
            }
            Scale = e.NewSize.Width / OriginWidth;
            double scale = e.NewSize.Width / e.PreviousSize.Width;

            foreach (Button btn in BtnLocations)
            {
                btn.Width = scale * btn.Width;
                btn.Height = scale * btn.Height;
                btn.Margin = new Thickness(scale * btn.Margin.Left, scale * btn.Margin.Top, 0, 0);
            }
        }


        private void BtnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            MapGrid.Children.Clear();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "图像文件(*.jpg,*.jpeg,*.bmp,*.png)|*.jpg;*.jpeg;*.bmp;*.png;";
            if (dialog.ShowDialog() == DialogResult.HasValue)
            {
                string file = dialog.FileName;
            }
            SetCurMap(dialog.FileName);
            MySettings.Set(CurBookName, "CurMapPath", dialog.FileName);
            string md5 = Common.GetMD5HashFromFile(dialog.FileName);
            MySettings.Set(CurBookName, "CurMapMd5", md5);
        }

        private void MapImage_Loaded(object sender, RoutedEventArgs e)
        {
            string imgPath = MySettings.Get(CurBookName, "CurMapPath");
            string imgMd5 = MySettings.Get(CurBookName, "CurMapMd5");
            SetCurMap(imgPath);
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("select * from 地图地点表 where Md5='{0}';", imgMd5);
            SQLiteDataReader reader = cSqlite.ExecuteQuery(sql);
            MapGrid.Children.Clear();
            while (reader.Read())
            {
                Button btnLocation = CreateLocation(Convert.ToDouble(reader["PointX"]), Convert.ToDouble(reader["PointY"]));

            }
            reader.Close();
        }

        private void BtnLocation_MouseMove(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Tag != null && (bool)btn.Tag == true)
            {
                Point offsetPoint = e.GetPosition(this.MapGrid);
                btn.Margin = new Thickness(offsetPoint.X - btn.ActualWidth / 2, offsetPoint.Y - btn.ActualHeight / 2, 0, 0);
            }
        }

        private void BtnLocation_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            btn.Tag = true;
        }

        private void BtnLocation_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            btn.Tag = false;
        }

        private void BtnLocation_Click(object sender, RoutedEventArgs e)
        {

        }

        bool flagCreate = false;
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            window.Cursor = Cursors.Cross;
            flagCreate = true;
        }


        private void MapImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (flagCreate == true)
            {
                Point point = Mouse.GetPosition(MapGrid);
                CreateLocation(point.X, point.Y);
                flagCreate = false;
                window.Cursor = null;
            }

        }

    }
}

