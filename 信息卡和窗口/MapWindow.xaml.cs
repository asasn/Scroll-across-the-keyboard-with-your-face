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

namespace 脸滚键盘.信息卡和窗口
{
    public partial class DragButton : Button
    {
        public static readonly DependencyProperty IsDragProperty = DependencyProperty.Register("IsDrag", typeof(Boolean), typeof(DragButton));
        public static readonly DependencyProperty CurrentPosProperty = DependencyProperty.Register("CurrentPos", typeof(Point), typeof(DragButton));
        public static readonly DependencyProperty ClickPosProperty = DependencyProperty.Register("ClickPos", typeof(Point), typeof(DragButton));

        /// <summary>
        /// 是否拖拽
        /// </summary>
        public bool IsDrag
        {
            get
            {
                return (bool)this.GetValue(IsDragProperty);
            }
            set
            {
                this.SetValue(IsDragProperty, value);
            }
        }

        /// <summary>
        /// 按钮的定位位置
        /// </summary>
        public Point CurrentPos
        {
            get
            {
                return (Point)this.GetValue(CurrentPosProperty);
            }
            set
            {
                this.SetValue(CurrentPosProperty, value);
            }
        }

        /// <summary>
        /// 当前鼠标点在按钮上的位置
        /// </summary>
        public Point ClickPos
        {
            get
            {
                return (Point)this.GetValue(ClickPosProperty);
            }
            set
            {
                this.SetValue(ClickPosProperty, value);
            }
        }
    }

    /// <summary>
    /// MapWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MapWindow : Window
    {
        public MapWindow()
        {
            InitializeComponent();


            //添加事件
            AddHandler(btn);
            AddHandler(btn2);
        }
        
        void AddHandler(DragButton dbtn)
        {
            dbtn.AddHandler(Button.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.CanvasButtonLeftUp), true);
            dbtn.AddHandler(Button.MouseMoveEvent, new MouseEventHandler(this.Canvas_MouseMove), true);
            dbtn.AddHandler(Button.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.MouseButtonLeftDown), true);
            dbtn.CurrentPos = new Point((double)dbtn.GetValue(Canvas.LeftProperty), (double)dbtn.GetValue(Canvas.TopProperty));
        }

        /// <summary>
        /// 区域移动事件
        /// </summary>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            DragButton dbtn = sender as DragButton;
            if (dbtn == null)
            {
                return;
            }
            if (dbtn.IsDrag)
            {
                Point offsetPoint = e.GetPosition(this.canvas);
                double xOffset = offsetPoint.X - dbtn.CurrentPos.X - dbtn.ClickPos.X;
                double yOffset = offsetPoint.Y - dbtn.CurrentPos.Y - dbtn.ClickPos.Y;

                Rectangle rect = LogicalTreeHelper.FindLogicalNode(this, "rect") as Rectangle;
                TranslateTransform transform = (TranslateTransform)rect.RenderTransform;

                transform.X += xOffset;
                transform.Y += yOffset;
                dbtn.CurrentPos = new Point(offsetPoint.X - dbtn.ClickPos.X, offsetPoint.Y - dbtn.ClickPos.Y);
            }
        }

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        private void MouseButtonLeftDown(object sender, MouseButtonEventArgs e)
        {
            DragButton dbtn = sender as DragButton;
            if (dbtn == null)
            {
                return;
            }
            if (!dbtn.IsDrag)
            {
                //btn.Background = Brushes.White;
                dbtn.ClickPos = e.GetPosition(dbtn);

                VisualBrush visualBrush = new VisualBrush(dbtn);
                Rectangle rect = new Rectangle() { Width = dbtn.ActualWidth, Height = dbtn.Height, Fill = visualBrush, Name = "rect" };
                rect.SetValue(Canvas.LeftProperty, dbtn.GetValue(Canvas.LeftProperty));
                rect.SetValue(Canvas.TopProperty, dbtn.GetValue(Canvas.TopProperty));
                rect.RenderTransform = new TranslateTransform(0d, 0d);
                rect.Opacity = 0.6;
                this.canvas.Children.Add(rect);

                dbtn.IsDrag = true;                
            }
        }

        /// <summary>
        /// 区域鼠标左键抬起
        /// </summary>
        private void CanvasButtonLeftUp(object sender, MouseButtonEventArgs e)
        {
            DragButton dbtn = sender as DragButton;
            if (dbtn == null)
            {
                return;
            }
            if (dbtn.IsDrag)
            {
                dbtn.SetValue(Canvas.LeftProperty, dbtn.CurrentPos.X);
                dbtn.SetValue(Canvas.TopProperty, dbtn.CurrentPos.Y);

                Rectangle rect = LogicalTreeHelper.FindLogicalNode(this, "rect") as Rectangle;
                this.canvas.Children.Remove(rect);

                dbtn.IsDrag = false;
            }
        }


    }



    
}

