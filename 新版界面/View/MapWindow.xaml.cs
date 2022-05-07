using Microsoft.Win32;
using RootNS.Helper;
using RootNS.Model;
using RootNS.Workfolw;
using System;
using System.Collections;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RootNS.View
{
    /// <summary>
    /// MapWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MapWindow : Window
    {
        public MapWindow()
        {
            InitializeComponent();
        }

        ArrayList BtnLocations = new ArrayList();
        double Scale = 1;
        double OriginWidth;
        double OriginHeight;


        /// <summary>
        /// 设置当前地图
        /// </summary>
        /// <param name="imgPath"></param>
        void SetCurMap(string imgPath)
        {
            LbImagePath.Content = imgPath;
            MapImage.Source = IOHelper.GetImgObject(imgPath);
        }

        Button CreateLocation(string uid, string title, string tip, double pointX, double pointY)
        {
            //为了免除各种麻烦，这里不进行最大值最小值的设定，一概随缩放比率增大缩小
            double sideLength = 24.0;
            UcLocation btnLocation = new UcLocation();
            btnLocation.Uid = uid;
            ToolTipService.SetShowDuration(btnLocation, 60000);
            btnLocation.Padding = new Thickness(0);
            btnLocation.Background = Brushes.Transparent;
            btnLocation.Width = btnLocation.Height = sideLength * Scale;
            btnLocation.BorderThickness = new Thickness(0);
            btnLocation.FontSize = sideLength * Scale * 0.75;
            btnLocation.Content = "\ue860";
            btnLocation.Foreground = Brushes.GreenYellow;
            btnLocation.Click += BtnLocation_Click;
            btnLocation.MouseRightButtonDown += BtnLocation_MouseRightButtonDown;
            btnLocation.MouseRightButtonUp += BtnLocation_MouseRightButtonUp;
            btnLocation.MouseMove += BtnLocation_MouseMove;
            btnLocation.HorizontalAlignment = HorizontalAlignment.Left;
            btnLocation.VerticalAlignment = VerticalAlignment.Top;
            btnLocation.Margin = new Thickness(pointX - (btnLocation.Width / 2), pointY - (btnLocation.Height * 0.9), 0, 0);
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
                btn.FontSize = btn.Width * 0.75;
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
            SettingsHelper.Set(Gval.CurrentBook.Name, "CurMapPath", dialog.FileName);
            string md5 = FunctionPack.GetMD5HashFromFile(dialog.FileName);
            SettingsHelper.Set(Gval.CurrentBook.Name, "CurMapMd5", md5);
        }

        private void MapImage_Loaded(object sender, RoutedEventArgs e)
        {
            string imgPath = SettingsHelper.Get(Gval.CurrentBook.Name, "CurMapPath")?.ToString();
            string imgMd5 = SettingsHelper.Get(Gval.CurrentBook.Name, "CurMapMd5")?.ToString();
            SetCurMap(imgPath);
            string sql = string.Format("select * from 地图 where Md5='{0}';", imgMd5);
            SQLiteDataReader reader = SqliteHelper.PoolDict[Gval.CurrentBook.Name].ExecuteQuery(sql);
            MapGrid.Children.Clear();
            while (reader.Read())
            {
                Button btnLocation = CreateLocation(reader["Uid"].ToString(), reader["Title"].ToString(), reader["Summary"].ToString(), Convert.ToDouble(reader["PointX"]), Convert.ToDouble(reader["PointY"]));

            }
            reader.Close();
        }

        private void BtnLocation_MouseMove(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (IsMoving == true)
            {
                Point offsetPoint = e.GetPosition(MapGrid);
                btn.Margin = new Thickness(offsetPoint.X - btn.ActualWidth / 2, offsetPoint.Y - btn.ActualHeight * 0.9, 0, 0);
            }
        }

        bool IsMoving = false;
        private void BtnLocation_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsMoving = true;
        }

        private void BtnLocation_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMoving = false;
            Button btn = sender as Button;
            double pointX = (btn.Margin.Left + (btn.Width / 2));
            double pointY = (btn.Margin.Top + (btn.Height * 0.8));
            string sql = string.Format("UPDATE 地图 SET PointX='{0}', PointY='{1}' where Uid='{2}';", pointX / Scale, pointY / Scale, btn.Uid);
            SqliteHelper.PoolDict[Gval.CurrentBook.Name].ExecuteNonQuery(sql);
        }

        private void BtnLocation_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (IsDeleting == true)
            {
                MapGrid.Children.Remove(btn);
                string sql = string.Format("DELETE FROM 地图 where Uid='{0}';", btn.Uid);
                SqliteHelper.PoolDict[Gval.CurrentBook.Name].ExecuteNonQuery(sql);
            }
        }

        bool IsCreating = false;
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (IsCreating == true)
            {
                window.Cursor = null;
                IsCreating = false;
            }
            else
            {
                window.Cursor = Cursors.Cross;
                IsCreating = true;
            }

        }


        private void MapImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCreating == true)
            {
                Point point = Mouse.GetPosition(MapGrid);
                Button btn = CreateLocation(Guid.NewGuid().ToString(), "", "", point.X, point.Y);
                double pointX = (btn.Margin.Left + (btn.Width / 2));
                double pointY = (btn.Margin.Top + (btn.Height * 0.8));
                string sql = string.Format("INSERT INTO 地图 (Uid, Md5 , Title, Summary, PointX, PointY) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');", btn.Uid, SettingsHelper.Get(Gval.CurrentBook.Name, "CurMapMd5")?.ToString(), "", "", pointX / Scale, pointY / Scale);
                SqliteHelper.PoolDict[Gval.CurrentBook.Name].ExecuteNonQuery(sql);
            }
        }


        private void window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsCreating == true)
            {
                IsCreating = false;
                window.Cursor = null;
            }
            if (IsDeleting == true)
            {
                IsDeleting = false;
                window.Cursor = null;
            }
        }

        bool IsDeleting = false;
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (IsDeleting == true)
            {
                window.Cursor = null;
                IsDeleting = false;
            }
            else
            {
                window.Cursor = Cursors.Hand;
                IsDeleting = true;
            }
        }

    }
}

