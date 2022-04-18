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



        #region 命令
        private void Command_SaveText_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnSaveDoc.IsEnabled = false;
            Node node = this.DataContext as Node;
            node.Text = ThisTextEditor.Text;
            node.WordsCount = HelperEditor.CountWords(ThisTextEditor.Text);
            try
            {
                CSqlitePlus cSqlite = CSqlitePlus.PoolDict[node.OwnerName];
                string sql = string.Format("UPDATE {0} SET Text='{1}', Summary='{2}', WordsCount='{3}' WHERE Uid='{4}';", node.TabName, node.Text.Replace("'", "''"), node.Summary.Replace("'", "''"), node.WordsCount, node.Uid);
                cSqlite.ExecuteNonQuery(sql);

                //保持连接会导致文件占用，不能及时同步和备份，过多重新连接则是不必要的开销。
                //故此在数据库占用和重复连接之间选择了一个平衡，允许保存之后的数据库得以上传。
                cSqlite.Close();
                HandyControl.Controls.Growl.Success("本文档内容保存！");
            }
            catch (Exception ex)
            {
                HandyControl.Controls.Growl.Warning(String.Format("本次保存失败！\n{0}", ex));
            }
        }

        private void Command_Typesetting_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HelperEditor.TypeSetting(ThisTextEditor);
        }

        #endregion

        #region 按钮点击事件


        public void BtnSaveText_Click(object sender, RoutedEventArgs e)
        {
            Command_SaveText_Executed(null, null);
        }

        private void BtnTypesetting_Click(object sender, RoutedEventArgs e)
        {
            Command_Typesetting_Executed(null, null);
        }
        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnCopyTitle_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnPaste_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ThisTextEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ThisTextEditor.Document.Insert(ThisTextEditor.SelectionStart, "\n　　");
                ThisTextEditor.LineDown();
                Command_SaveText_Executed(null, null);
            }
            //逗号||句号的情况
            if (e.Key == Key.OemComma ||
                e.Key == Key.OemPeriod)
            {
                Command_SaveText_Executed(null, null);
            }
        }
        private void ThisTextEditor_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as HandyControl.Controls.TabItem).RaiseEvent(new RoutedEventArgs(HandyControl.Controls.TabItem.ClosingEvent));
        }

        #endregion

        private void ThisTextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            Node node = ThisControl.DataContext as Node;
            if (string.IsNullOrWhiteSpace(node.Text) == true)
            {
                node.Text = "　　";
            }
            ThisTextEditor.Text = node.Text;
            HelperEditor.SetThisEditorColorRules(ThisTextEditor);
            ThisTextEditor.Focus();
            HelperEditor.MoveToEnd(ThisTextEditor);

            BtnSaveDoc.IsEnabled = false;
        }


        private void ThisTextEditor_TextChanged(object sender, EventArgs e)
        {
            BtnSaveDoc.IsEnabled = true;
            LbWorksCount.Content = HelperEditor.CountWords(ThisTextEditor.Text);
            LbValueValue.Content = string.Format("{0:F}", Math.Round(Convert.ToDouble(LbWorksCount.Content) * Gval.CurrentBook.Price / 1000, 2, MidpointRounding.AwayFromZero));
            HelperEditor.RefreshStyleForCardsBox(ThisTextEditor);
        }


        ToolTip toolTip = new ToolTip();
        private void ThisTextEditor_MouseHover(object sender, MouseEventArgs e)
        {
            var pos = ThisTextEditor.GetPositionFromPoint(e.GetPosition(ThisTextEditor));
            if (pos != null)
            {
                //toolTip.PlacementTarget = this; // required for property inheritance
                //toolTip.Content = pos.ToString();
                //toolTip.IsOpen = true;
                //e.Handled = true;
            }
        }


        private void ThisTextEditor_MouseHoverStopped(object sender, MouseEventArgs e)
        {
            toolTip.IsOpen = false;
        }

        private void ThisTextEditor_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ThisTextEditor_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ThisControl_Loaded(object sender, RoutedEventArgs e)
        {
            HandyControl.Controls.TabItem tabItem = this.DataContext as HandyControl.Controls.TabItem;
        }


    }
}
