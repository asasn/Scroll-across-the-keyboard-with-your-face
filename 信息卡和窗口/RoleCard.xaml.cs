using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using 脸滚键盘.公共操作类;
using 脸滚键盘.自定义控件;

namespace 脸滚键盘.信息卡和窗口
{
    /// <summary>
    /// RoleCard.xaml 的交互逻辑
    /// </summary>
    public partial class RoleCard : Window
    {
        /// <summary>
        /// 父容器控件
        /// </summary>
        Button BtnParent;
        WrapPanel[] wrapPanels;
        string CurBookName;
        string TypeOfTree;

        public struct ThisCard
        {
            public static string id;
            public static string weight;
            public static string realAge;
            public static string 相对年龄;
        }

        public RoleCard(string curBookName, string typeOfTree, Button btnParent)
        {
            InitializeComponent();

            if (CurBookName == "index")
            {
                this.Left = 305;
                this.Top = 115;
            }
            else
            {
                this.Left = 305;
                this.Top = 115;
            }

            CurBookName = curBookName;
            TypeOfTree = typeOfTree;

            //添加拖曳面板事件
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };

            //根据外来调用传入的参数填充变量，以备给类成员方法使用
            BtnParent = btnParent;
            TbName.Text = btnParent.Content.ToString();

            WrapPanel[] temp = { 别称.WpMain, 身份.WpMain, 外观.WpMain, 阶级.WpMain, 所属.WpMain, 物品.WpMain, 能力.WpMain, 经历.WpMain, };
            wrapPanels = temp;

            //填充窗口信息
            GetDataAndFillCard();

            BtnSave.IsEnabled = false;
        }



        /// <summary>
        /// 显示信息卡的主流程：从数据库中获取信息以填充卡片
        /// </summary>
        void GetDataAndFillCard()
        {
            if (BtnParent != null)
            {
                FillBaseInfo();
                CardOperate.FillMainInfo(CurBookName, TypeOfTree, wrapPanels, BtnParent.Uid);

            }
        }

        void FillBaseInfo()
        {
            string tableName = TypeOfTree;
            SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
            string sql = string.Format("select * from {0}主表 where {0}id = '{1}';", tableName, BtnParent.Uid);
            SQLiteDataReader reader = sqlConn.ExecuteQuery(sql);
            while (reader.Read())
            {
                if (reader["备注"].ToString() != "")
                {
                    Tb备注.Text = reader["备注"].ToString();
                }
                if (reader["权重"].ToString() != "")
                {
                    ThisCard.weight = reader["权重"].ToString();
                }
                if (reader["相对年龄"].ToString() == "")
                {
                    TbBornYear.Text = "0";
                }
                else
                {
                    TbBornYear.Text = reader["相对年龄"].ToString();
                }

            }
            reader.Close();
            

            int realAge = Gval.CurrentBook.CurrentYear - int.Parse(TbBornYear.Text);
            Grid grid = new Grid();
            TextBlock lb1 = new TextBlock();
            TextBlock lb2 = new TextBlock();
            lb1.Text = "权重：";
            lb2.Text = "真实年龄："; 
            lb1.Margin = new Thickness(7, 0, 0, 0);
            lb2.Margin = new Thickness(152, 0, 0, 0);
            TextBlock tbk1 = new TextBlock();
            TextBlock tbk2 = new TextBlock();
            tbk1.Text = ThisCard.weight;
            tbk2.Text = realAge.ToString(); ;
            tbk1.Margin = new Thickness(50, 0, 0, 0);
            tbk2.Margin = new Thickness(219, 0, 0, 0);
            grid.Children.Add(lb1);
            grid.Children.Add(lb2);
            grid.Children.Add(tbk1);
            grid.Children.Add(tbk2);
            card.Header = grid;
        }


        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    Button b = sender as Button;
        //    string wpName = "Wp" + b.Uid;
        //    WrapPanel wp = gCard.FindName(wpName) as WrapPanel;
        //    TextBox tb = CardOperate.AddTextBox();
        //    tb.TextChanged += Tb_TextChanged;
        //    wp.Children.Add(tb);
        //}

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSave.IsEnabled = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            BtnSave.IsEnabled = false;
            string tableName = TypeOfTree;
            SqliteOperate sqlConn = Gval.SQLClass.Pools[CurBookName];
            SQLiteDataReader reader = sqlConn.ExecuteQuery(string.Format("select * from {0}主表 where 名称='{1}'", tableName, TbName.Text.Replace("'", "''")));
            while (reader.Read())
            {
                if (BtnParent.Uid != reader.GetString(0).ToString())
                {
                    MessageBox.Show("数据库中已经存在同名不同id条目，请修改成为其他名称！");
                    reader.Close();
                    
                    return;
                }
            }
            reader.Close();

            if (false == string.IsNullOrEmpty(BtnParent.Content.ToString()))
            {
                if (string.IsNullOrEmpty(ThisCard.weight))
                {
                    ThisCard.weight = 0.ToString();
                }
                if (string.IsNullOrEmpty(TbBornYear.Text))
                {
                    TbBornYear.Text = 0.ToString();
                }

                string sql = string.Format("update {0}主表 set 名称='{1}', 备注='{2}', 权重={3}, 相对年龄={4} where {0}id = '{5}';", tableName, TbName.Text.Replace("'", "''"), Tb备注.Text.Replace("'", "''"), ThisCard.weight, TbBornYear.Text, BtnParent.Uid);
                sqlConn.ExecuteNonQuery(sql);

                //传递给父容器
                BtnParent.Content = TbName.Text;

                CardOperate.SaveMainInfo(CurBookName, TypeOfTree, wrapPanels, BtnParent.Uid);
            }

            Gval.Uc.RoleCards.RefreshKeyWords();
            Gval.Uc.RoleCards.MarkNamesInChapter();
            
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (BtnSave.IsEnabled == true)
            {
                MessageBoxResult dr = MessageBox.Show("有数据尚未保存\n要在退出前保存更改吗？", "Tip", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (dr == MessageBoxResult.Yes)
                {
                    BtnSave_Click(null, null);
                }
                if (dr == MessageBoxResult.No)
                {

                }
                if (dr == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            this.Close();            
        }


        private void TbBornYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int.TryParse(tb.Text, out int str);
            tb.Text = str.ToString();

            Tb_TextChanged(sender, e);
        }
    }
}