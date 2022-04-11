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
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using RootNS.Behavior;
using RootNS.Model;

namespace RootNS.Brick
{
    /// <summary>
    /// MyEditor.xaml 的交互逻辑
    /// </summary>
    public partial class EditorBase : UserControl
    {
        public EditorBase()
        {
            InitializeComponent();
        }



        public string MainText
        {
            get { return (string)GetValue(MainTextProperty); }
            set { SetValue(MainTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.Register("MainText", typeof(string), typeof(EditorBase), new PropertyMetadata("　　"));




        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnFormat_Click(object sender, RoutedEventArgs e)
        {
            HelperEditor.TypeSetting(ThisTextEditor);
        }

        private void BtnPaste_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCopyTitle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {

        }

        public void BtnSaveText_Click(object sender, RoutedEventArgs e)
        {
            BtnSaveDoc.IsEnabled = false;
            Node node = this.DataContext as Node;
            node.Text = ThisTextEditor.Text;
            node.WordsCount = HelperEditor.CountWords(ThisTextEditor.Text);
            try
            {
                CSqlitePlus cSqlite = CSqlitePlus.PoolDict[node.OwnerName];
                string sql = string.Format("UPDATE {0} set Text='{1}', WordsCount='{2}' WHERE Uid='{3}';", node.TabName, node.Text.Replace("'", "''"), node.WordsCount, node.Uid);
                cSqlite.ExecuteNonQuery(sql);

                //保持连接会导致文件占用，不能及时同步和备份，过多重新连接则是不必要的开销。
                //故此在数据库占用和重复连接之间选择了一个平衡，允许保存之后的数据库得以上传。
                cSqlite.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("本次保存失败！\n{0}", ex));
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Gval.OpeningDocList.Remove((sender as Button).DataContext as Node);
        }

        private void ThisTextEditor_TextChanged(object sender, EventArgs e)
        {
            BtnSaveDoc.IsEnabled = true;
            LbWorksCount.Content = HelperEditor.CountWords(ThisTextEditor.Text);
        }

        private void ThisTextEditor_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void ThisTextEditor_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void ThisTextEditor_MouseHoverStopped(object sender, MouseEventArgs e)
        {

        }

        private void ThisTextEditor_MouseHover(object sender, MouseEventArgs e)
        {

        }

        private void ThisTextEditor_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ThisTextEditor_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ThisTextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            BtnSaveDoc.IsEnabled = false;
        }


        private void Command_SaveText_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnSaveText_Click(null, null);
        }
    }
}
