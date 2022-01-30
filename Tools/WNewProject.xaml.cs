using NSMain.Bricks;
using NSMain.Cards;
using System;
using System.Collections;
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

namespace NSMain.Tools
{
    /// <summary>
    /// WPuzzles.xaml 的交互逻辑
    /// </summary>
    public partial class WNewProjects : Window
    {

        public WNewProjects(string curBookName, string typeOfTree)
        {
            InitializeComponent();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
            GlobalVal.Uc.NewProjects = this;
        }

        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(WNewProjects), new PropertyMetadata(null));




        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(WNewProjects), new PropertyMetadata(null));




        private void BtnAddScene_Click(object sender, RoutedEventArgs e)
        {
            WpMain.AddCard(TbTitle.Text);
            TbTitle.Clear();
        }



        private void TbShowTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WpMain.CurCard == null)
            {
                return;
            }
            BtnSave.IsEnabled = true;
        }

        private void TbShowContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WpMain.CurCard == null)
            {
                return;
            }
            BtnSave.IsEnabled = true;
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (WpMain.CurCard == null)
            {
                return;
            }
            CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
            string sql = string.Format("UPDATE 题材主表 set 标题='{0}', 内容='{1}' where Uid='{2}';", TbShowTitle.Text.Replace("'", "''"), TbShowContent.Text.Replace("'", "''"), WpMain.CurCard.Uid);
            cSqlite.ExecuteNonQuery(sql);
            CCards.SaveMainInfo(CurBookName, "题材", R1.WpMain.Children, WpMain.CurCard.Uid);
            CCards.SaveMainInfo(CurBookName, "题材", R2.WpMain.Children, WpMain.CurCard.Uid);
            CCards.SaveMainInfo(CurBookName, "题材", R3.WpMain.Children, WpMain.CurCard.Uid);
            CCards.SaveMainInfo(CurBookName, "题材", R4.WpMain.Children, WpMain.CurCard.Uid);
            CCards.SaveMainInfo(CurBookName, "题材", R5.WpMain.Children, WpMain.CurCard.Uid);
            CCards.SaveMainInfo(CurBookName, "题材", R6.WpMain.Children, WpMain.CurCard.Uid);
            CCards.SaveMainInfo(CurBookName, "题材", R7.WpMain.Children, WpMain.CurCard.Uid);
            BtnSave.IsEnabled = false;
        }

        private void TbTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnAddScene.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void TbShowTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && BtnSave.IsEnabled == true)
            {
                BtnSave.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }



        void LoadBoxRecords(UBoxRecords uBR, ArrayList headers)
        {
            uBR.WpMain.Children.Clear();
            uBR.BtnSave = BtnSave;
            ArrayList uRecords = new ArrayList();
            foreach (string header in headers)
            {
                uRecords.Add(new CCards.属性条目()
                {
                    Uid = header,
                    Text = header,
                });
            }
            uBR.WpMain_Build(uRecords);
            CCards.FillMainInfo(CurBookName, "题材", uBR.WpMain.Children, WpMain.CurCard.Uid);
            BtnSave.IsEnabled = false;
        }


        private void R1_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList headers = new ArrayList() { "主题", "风格"};
            LoadBoxRecords(R1, headers);
        }

        private void R2_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList headers = new ArrayList() { "大纲", };
            LoadBoxRecords(R2, headers);
        }

        private void R3_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList headers = new ArrayList() { "角色", };
            LoadBoxRecords(R3, headers);
        }

        private void R4_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList headers = new ArrayList() { "线索", };
            LoadBoxRecords(R4, headers);
        }

        private void R5_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList headers = new ArrayList() {"卖点", "金手指" ,};
            LoadBoxRecords(R5, headers);
        }

        private void R6_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList headers = new ArrayList() { "阶级", };
            LoadBoxRecords(R6, headers);
        }

        private void R7_Loaded(object sender, RoutedEventArgs e)
        {
            ArrayList headers = new ArrayList() { "设定", };
            LoadBoxRecords(R7, headers);
        }

        private void WpMain_GotFocus(object sender, RoutedEventArgs e)
        {
            R1_Loaded(null, null);
            R2_Loaded(null, null);
            R3_Loaded(null, null);
            R4_Loaded(null, null);
            R5_Loaded(null, null);
            R6_Loaded(null, null);
            R7_Loaded(null, null);
        }

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
                    e.Cancel = true;
                }
            }
        }


    }
}
