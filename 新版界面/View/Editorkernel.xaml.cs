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
using RootNS.Helper;
using RootNS.Model;

namespace RootNS.View
{
    /// <summary>
    /// MyEditor.xaml 的交互逻辑
    /// </summary>
    public partial class Editorkernel : UserControl
    {
        public Editorkernel()
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
            DependencyProperty.Register("MainText", typeof(string), typeof(Editorkernel), new PropertyMetadata("　　"));



        #region 命令
        private void Command_SaveText_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BtnSaveDoc.IsEnabled = false;
            Node node = this.DataContext as Node;
            node.Text = ThisTextEditor.Text;
            node.WordsCount = EditorHelper.CountWords(ThisTextEditor.Text);
            try
            {
                SqliteHelper cSqlite = SqliteHelper.PoolDict[node.OwnerName];
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
            EditorHelper.TypeSetting(ThisTextEditor);
        }


        private void Command_Find_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FindReplaceDialog.theDialog = FindReplaceDialog.ShowForReplace(ThisTextEditor);
            FindReplaceDialog.theDialog.TabFind.IsSelected = true;
        }

        private void Command_Replace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FindReplaceDialog.theDialog = FindReplaceDialog.ShowForReplace(ThisTextEditor);
            FindReplaceDialog.theDialog.TabReplace.IsSelected = true;
        }

        private void Command_MoveNext_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FindReplaceDialog.theDialog = FindReplaceDialog.GetOperateObject(ThisTextEditor);
            FindReplaceDialog.theDialog.cbSearchUp.IsChecked = false;
            FindReplaceDialog.theDialog.FindNext(ThisTextEditor.TextArea.Selection.GetText());

        }

        private void Command_MovePrevious_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FindReplaceDialog.theDialog = FindReplaceDialog.GetOperateObject(ThisTextEditor);
            FindReplaceDialog.theDialog.cbSearchUp.IsChecked = true;
            FindReplaceDialog.theDialog.FindNext(ThisTextEditor.TextArea.Selection.GetText());
        }

        private void Command_CloseTabItem_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HandyControl.Controls.TabItem tabItem = this.Parent as HandyControl.Controls.TabItem;
            CommandHelper.FindByName(tabItem.CommandBindings, "Close").Execute(tabItem);
        }

        private void Command_EditCard_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Card[] CardBoxs = { Gval.CurrentBook.CardRole, Gval.CurrentBook.CardOther, Gval.CurrentBook.CardWorld };
            foreach (Card rootCard in CardBoxs)
            {
                foreach (Card card in rootCard.ChildNodes)
                {
                    if (ThisTextEditor.SelectedText.Equals(card.Title) == true || card.IsEqualsNickNames(ThisTextEditor.SelectedText, card.NickNames))
                    {
                        CardWindow cw = new CardWindow(card);
                        cw.Left = ThisTextEditor.TranslatePoint(Mouse.GetPosition(this), Gval.View.MainWindow).X - 150;
                        cw.Top = ThisTextEditor.TranslatePoint(Mouse.GetPosition(this), Gval.View.MainWindow).Y + 20;
                        cw.ShowDialog();
                        return;
                    }
                }
            }
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
            Command_CloseTabItem_Executed(null, null);
        }




        #endregion



        private void ThisTextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            //因为在TabControl中，每次切换的时候都会触发这个事件，故而一些初始化步骤放在父容器
            EditorHelper.RefreshIsContainFlagForCardsBox(ThisTextEditor.Text);
            ThisTextEditor.Focus();

        }

        private void ThisTextEditor_TextChanged(object sender, EventArgs e)
        {
            BtnSaveDoc.IsEnabled = true;
            LbWorksCount.Content = EditorHelper.CountWords(ThisTextEditor.Text);
            LbValueValue.Content = string.Format("{0:F}", Math.Round(Convert.ToDouble(LbWorksCount.Content) * Gval.CurrentBook.Price / 1000, 2, MidpointRounding.AwayFromZero));
            EditorHelper.RefreshIsContainFlagForCardsBox(ThisTextEditor.Text);
        }


        ToolTip toolTip = new ToolTip();

        /// <summary>
        /// 鼠标悬浮提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThisTextEditor_MouseHover(object sender, MouseEventArgs e)
        {
            TextViewPosition? pos = ThisTextEditor.GetPositionFromPoint(e.GetPosition(ThisTextEditor));
            if (pos != null)
            {
                //toolTip.PlacementTarget = this; // required for property inheritance
                int offset = ThisTextEditor.Document.GetOffset(pos.Value.Location);
                foreach (HighlightingRule rule in ThisTextEditor.SyntaxHighlighting.MainRuleSet.Rules)
                {
                    System.Text.RegularExpressions.MatchCollection matches = rule.Regex.Matches(ThisTextEditor.Text);
                    if (matches.Count > 0)
                    {
                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            //注意偏移值的问题，第一个相等条件相当于左边多出半个字符宽度，第二个则是右边多出半个字符宽度……
                            if (match.Index <= offset && offset - match.Index <= match.Value.Length)
                            {
                                Card[] CardBoxs = { Gval.CurrentBook.CardRole, Gval.CurrentBook.CardOther, Gval.CurrentBook.CardWorld };
                                foreach (Card rootCard in CardBoxs)
                                {
                                    foreach (Card card in rootCard.ChildNodes)
                                    {
                                        if (match.Value.Equals(card.Title) || card.IsEqualsNickNames(match.Value, card.NickNames))
                                        {
                                            toolTip.Content = new CardHover(card);
                                            toolTip.IsOpen = true;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 停止悬浮提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


    }
}
