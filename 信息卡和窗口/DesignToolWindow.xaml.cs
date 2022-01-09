using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using 脸滚键盘.公共操作类;
using 脸滚键盘.自定义控件;

namespace 脸滚键盘.信息卡和窗口
{

    /// <summary>
    /// DesignToolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DesignToolWindow : Window
    {
        public DesignToolWindow(string curBookName, string typeOfTree)
        {
            InitializeComponent();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
        }

        string CurBookName;
        string TypeOfTree;

        private void BtnAddScene_Click(object sender, RoutedEventArgs e)
        {
            UcScenesCard sCard = new UcScenesCard();
            WpScenes.Children.Add(sCard);
            sCard.TbTitle.Text = string.Format("第{0}幕 {1}", WpScenes.Children.IndexOf(sCard) + 1, TbTitle.Text);
            TbTitle.Clear();
        }

        /// <summary>
        /// 鼠标滚轮控制滚动（增加了对左右滚动的处理）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WpScenes_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,

                Source = sender
            };
            if (e.Delta > 0)
            {
                ScMain.LineLeft();
                ScMain.LineLeft();
                ScMain.LineLeft();
            }
            else
            {
                ScMain.LineRight();
                ScMain.LineRight();
                ScMain.LineRight();
            }
            if (e.Delta > 0)
            {
                ScMain.LineUp();
                ScMain.LineUp();
                ScMain.LineUp();
            }
            else
            {
                ScMain.LineDown();
                ScMain.LineDown();
                ScMain.LineDown();
            }

            WpScenes.RaiseEvent(eventArg);
            e.Handled = true;
        }

        private void WpScenes_Loaded(object sender, RoutedEventArgs e)
        {
            string tableName = TypeOfTree;
            SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
            //尝试建立新表（IF NOT EXISTS）
            string sql = string.Format("CREATE TABLE IF NOT EXISTS 场记大纲表 (Uid CHAR PRIMARY KEY, 索引 INTEGER, 标题 CHAR, 名称 CHAR, 内容 CHAR);");
            sql += string.Format("CREATE INDEX IF NOT EXISTS 场记大纲表Uid ON 大纲表 (Uid);");
            sqlConn.ExecuteNonQuery(sql);

            sql = string.Format("select * from 场记大纲表 ORDER BY 索引;", tableName);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            WpScenes.Children.Clear();
            while (reader.Read())
            {
                UcScenesCard sCard = new UcScenesCard();
                sCard.GotFocus += SCard_GotFocus;
                sCard.Uid = reader["Uid"].ToString();
                sCard.Index = Convert.ToInt32(reader["索引"]);
                sCard.StrTitile = reader["标题"].ToString();
                sCard.StrName = reader["名称"].ToString();
                sCard.StrContent = reader["内容"].ToString();
                WpScenes.Children.Add(sCard);
            }
            reader.Close();
        }

        private void SCard_GotFocus(object sender, RoutedEventArgs e)
        {
            UcScenesCard sCard = sender as UcScenesCard;
            TbShowTitle.Text = sCard.StrTitile;
            TbShowName.Text = sCard.StrName;
            TbShowContent.Text = sCard.StrContent;
        }
    }
}
