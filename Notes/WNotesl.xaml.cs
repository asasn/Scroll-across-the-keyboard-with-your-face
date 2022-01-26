using NSMain.Bricks;
using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NSMain.Notes
{
    /// <summary>
    /// DesignToolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WNotes : Window
    {
        public WNotes(string curBookName, string typeOfTree)
        {
            InitializeComponent();
            CurBookName = curBookName;
            TypeOfTree = typeOfTree;
            GlobalVal.Uc.Notes = this;
        }

        public string CurBookName
        {
            get { return (string)GetValue(CurBookNameProperty); }
            set { SetValue(CurBookNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurBookName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurBookNameProperty =
            DependencyProperty.Register("CurBookName", typeof(string), typeof(WNotes), new PropertyMetadata(null));




        public string TypeOfTree
        {
            get { return (string)GetValue(TypeOfTreeProperty); }
            set { SetValue(TypeOfTreeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfTree.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfTreeProperty =
            DependencyProperty.Register("TypeOfTree", typeof(string), typeof(WNotes), new PropertyMetadata(null));



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
            string sql = string.Format("UPDATE 随手记录表 set 标题='{0}', 内容='{1}' where Uid='{2}';", TbShowTitle.Text.Replace("'", "''"), TbShowContent.Text.Replace("'", "''"), WpMain.CurCard.Uid);
            cSqlite.ExecuteNonQuery(sql);
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

    }
}
