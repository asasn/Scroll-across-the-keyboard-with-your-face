using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Search;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.公共操作类;
using static 脸滚键盘.控件方法类.UTreeView;
using static 脸滚键盘.控件方法类.UEditor;
using System.Windows.Media;

namespace 脸滚键盘.自定义控件
{
    /// <summary>
    /// UcEditor.xaml 的交互逻辑
    /// </summary>
    public partial class UcEditor : UserControl
    {
        public UcEditor()
        {
            InitializeComponent();
        }

        ToolTip toolTip = new ToolTip();

        /// <summary>
        /// 鼠标悬浮提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditor_MouseHover(object sender, MouseEventArgs e)
        {
            var pos = textEditor.GetPositionFromPoint(e.GetPosition(textEditor));
            if (pos != null)
            {
                //toolTip.PlacementTarget = this; // required for property inheritance
                //toolTip.Content = pos.ToString();
                //toolTip.IsOpen = true;
                //e.Handled = true;
            }
        }

        /// <summary>
        /// 停止悬浮提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditor_MouseHoverStopped(object sender, MouseEventArgs e)
        {
            toolTip.IsOpen = false;
        }

        string TypeOfTree;
        string CurBookName;
        SearchPanel searchPanel;
        TreeViewNode CurNode;

        /// <summary>
        /// 事件：控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Loaded(object sender, RoutedEventArgs e)
        {
            textEditor.TextArea.SelectionChanged += textEditor_TextArea_SelectionChanged;

            if (TypeOfTree == "book")
            {
                lbValue.Visibility = Visibility.Visible;
            }
            else
            {
                lbValue.Visibility = Visibility.Hidden;
            }

            //快速搜索功能
            //searchPanel =  SearchPanel.Install(textEditor.TextArea);
            //searchPanel.UseRegex = true;
            //searchPanel.Visibility = Visibility.Hidden;
            //searchPanel.Open();
        }

        public void LoadChapter(string curBookName, string typeOfTree)
        {
            CurNode = this.DataContext as TreeViewNode;
            Gval.CurrentBook.CurNode = this.DataContext as TreeViewNode;
            TypeOfTree = typeOfTree;
            CurBookName = curBookName;
            textEditor.Load(ConvertStringToStream(CurNode.NodeContent));

            //光标移动至文末       
            textEditor.ScrollToLine(textEditor.LineCount);
            textEditor.SelectionLength = 0;
            textEditor.SelectionStart = textEditor.Text.Length;
            for (int i = 0; i < 5; i++)
            {
                textEditor.ScrollToEnd();
            }

            SetRules();
        }


        /// <summary>
        /// 根据文件设置语法规则
        /// </summary>
        public void SetRules()
        {
            textEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(Gval.Path.App, "Resourses/Text.xshd");
            Stream xshdStream = File.OpenRead(fullFileName);
            XmlTextReader xshdReader = new XmlTextReader(xshdStream);
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);
            xshdReader.Close();
            xshdStream.Close();

            if (Gval.Uc.RoleCards != null && Gval.Uc.OtherCards != null)
            {
                WrapPanel[] wps = { Gval.Uc.RoleCards.WpCards, Gval.Uc.OtherCards.WpCards };

                foreach (WrapPanel wp in wps)
                {
                    string keyword;
                    string tableName = wp.Tag.ToString();
                    SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
                    SQLiteDataReader reader = sqlConn.ExecuteQuery(string.Format("SELECT 名称 FROM (SELECT 名称 FROM {0}主表 UNION SELECT 别称 FROM {0}别称表) ORDER BY LENGTH(名称) DESC;", tableName));
                    IList<HighlightingRule> rules = textEditor.SyntaxHighlighting.MainRuleSet.Rules;
                    while (reader.Read())
                    {
                        keyword = reader["名称"].ToString();
                        AddKeyword(rules, keyword, tableName);
                    }
                    reader.Close();
                    sqlConn.Close();
                }
            }

        }

        /// <summary>
        /// 添加一个变色关键词
        /// </summary>
        /// <param name="keyword"></param>
        void AddKeyword(IList<HighlightingRule> rules, string keyword, string colorName)
        {

            SetRules(rules, keyword, colorName);
        }

        void SetRules(IList<HighlightingRule> rules, string keyword, string colorName)
        {
            HighlightingRule rule = new HighlightingRule();
            rule.Color = textEditor.SyntaxHighlighting.GetNamedColor(colorName);
            rule.Regex = new Regex(keyword);
            rules.Add(rule);
        }

        void SetSpan(string keyword, string colorName)
        {
            var spans = textEditor.SyntaxHighlighting.MainRuleSet.Spans;
            HighlightingSpan span = new HighlightingSpan();
            span.SpanColor = textEditor.SyntaxHighlighting.GetNamedColor(colorName);
            span.StartExpression = new Regex(keyword);
            span.EndExpression = new Regex("");
            span.SpanColorIncludesStart = true;
            span.SpanColorIncludesEnd = true;
            spans.Add(span);
        }

        private void uc_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void chapterNameBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //转移焦点，触发标题栏失去焦点事件
                textEditor.Focus();

                //光标移动至文末
                textEditor.SelectionStart = textEditor.Text.Length;
                textEditor.ScrollToLine(textEditor.LineCount);
                textEditor.ScrollToEnd();
            }
        }

        public int words = 0;
        /// <summary>
        /// 方法：展示状态栏段落和字数信息
        /// </summary>
        void ShowTextInfo()
        {
            words = WordCount(textEditor.Text);
            if (lb1 != null && lb2 != null)
            {
                lb1.Content = "段落：" + textEditor.Document.Lines.Count.ToString();
                lb2.Content = "字数：" + words.ToString();
                lbValue.Content = "价值：" + ((double)words / 1000 * Gval.CurrentBook.Price).ToString() + "元";
            }
        }

        /// <summary>
        /// 方法：编辑区文字保存
        /// </summary>
        public void SaveText()
        {
            TreeViewNode CurNode = this.DataContext as TreeViewNode;

            if (CurNode == null)
            {
                return;
            }

            string tableName = TypeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}', isDir={2}, NodeContent='{3}', WordsCount={4}, IsExpanded={5}, IsChecked={6} where Uid = '{7}';", tableName, CurNode.NodeName.Replace("'", "''"), CurNode.IsDir, textEditor.Text.Replace("'", "''"), words, CurNode.IsExpanded, CurNode.IsChecked, CurNode.Uid);
            sqlConn.ExecuteNonQuery(sql);
            sqlConn.Close();


            CurNode.NodeContent = textEditor.Text;
            CurNode.WordsCount = words;

            btnSaveDoc.IsEnabled = false;

            Console.WriteLine("保存至数据库");
            Gval.Uc.RoleCards.MarkNamesInChapter();
        }





        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            ShowTextInfo();

            if (textEditor.TextArea.IsFocused == true && CurNode != null)
            {
                btnSaveDoc.IsEnabled = true;
            }
            if (PasteSign == true)
            {
                PasteSign = false;
                //连续粘贴可能会引起频繁保存的卡顿，所以注释掉了
                //SaveText();
            }
        }

        /// <summary>
        /// 事件：编辑区按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && textEditor.TextArea.IsFocused == true)
            {
                int a = textEditor.SelectionStart;
                int b = textEditor.Text.Length;
                string strA = textEditor.Text.Substring(0, a);
                string strB = textEditor.Text.Substring(a, b - a);
                string strM = "\n　　";

                //重新赋值给编辑区
                textEditor.Text = strA + strM + strB.ToString();

                //光标移动至正确的编辑位置
                textEditor.SelectionStart = a + strM.Length;
                textEditor.LineDown();//滚动，行+1
                textEditor.LineDown();//滚动，行+1

                //自动保存（回车时）
                SaveText();
            }
            if (e.Key == Key.F9)
            {
                //进行排版
                ReformatText(textEditor);
                SaveText();
            }
            //进行了删除之后（太过频繁）
            //if (e.Key == Key.Delete || e.Key == Key.Back)
            //{
            //    SaveText();
            //}
            //逗号的情况
            if (e.Key == Key.OemComma)
            {
                SaveText();
            }
            //句号的情况
            if (e.Key == Key.OemPeriod)
            {
                SaveText();
            }
        }
        FindReplaceDialog theDialog;
        /// <summary>
        /// 事件：编辑区快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                //同时按下了Ctrl + S键（S要最后按，因为判断了此次事件的e.Key）
                //修饰键只能按下Ctrl，如果还同时按下了其他修饰键，则不会进入
                btnSaveText_Click(null, null);
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                //如果被searchPanel占用就不要再设置
                theDialog = FindReplaceDialog.ShowForReplace(textEditor);
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
            {

            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Y)
            {

            }
            if (e.Key == Key.F3)
            {
                //如果被searchPanel占用就不要再设置
                if (theDialog == null)
                {
                    theDialog = new FindReplaceDialog(textEditor);
                }
                theDialog.FindNext(textEditor.TextArea.Selection.GetText());
            }
            if (e.Key == Key.F4)
            {
                if (searchPanel != null)
                {
                    searchPanel.SearchPattern = textEditor.SelectedText;
                    searchPanel.FindPrevious();
                }
            }
            if (e.Key == Key.F5)
            {
                if (searchPanel != null)
                {
                    searchPanel.SearchPattern = textEditor.SelectedText;
                    searchPanel.FindPrevious();
                    textEditor.Document.Replace(textEditor.SelectionStart, textEditor.SelectionLength, "测试");
                    searchPanel.FindNext();
                }
            }
            if (e.Key == Key.F6)
            {
                if (searchPanel != null)
                {
                    searchPanel.SearchPattern = textEditor.SelectedText;
                    searchPanel.FindPrevious();
                    textEditor.Document.Replace(textEditor.SelectionStart, textEditor.SelectionLength, "测试");
                    searchPanel.FindPrevious();
                }
            }
        }

        private void textEditor_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        bool PasteSign;
        private void textEditor_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut ||
                 e.Command == ApplicationCommands.Paste
             )
            {
                //预览剪切，粘贴
                PasteSign = true;
            }
        }

        #region 右侧按钮面板
        public void btnSaveText_Click(object sender, RoutedEventArgs e)
        {
            if (CurNode == null)
            {
                return;
            }
            SaveText();
            HandyControl.Controls.Growl.Success("本文档内容保存！");
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textEditor.Text);
            HandyControl.Controls.Growl.Success("已复制本文档内容到剪贴板！");
        }

        private void BtnCopyTitle_Click(object sender, RoutedEventArgs e)
        {
            if (CurNode == null)
            {
                return;
            }
            Clipboard.SetText(CurNode.NodeName);
            HandyControl.Controls.Growl.Success("已复制本文档标题到剪贴板！");
        }

        private void BtnPaste_Click(object sender, RoutedEventArgs e)
        {
            string temp = textEditor.Text;
            textEditor.Text = Clipboard.GetText();
            BtnUndo.DataContext =temp;
            BtnUndo.IsEnabled = true;
            ReformatText(textEditor);
            if (CurNode != null)
            {
                btnSaveDoc.IsEnabled = true;
            }
            
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Text = BtnUndo.DataContext.ToString();
            ReformatText(textEditor);
            BtnUndo.IsEnabled = false;
            if (CurNode != null)
            {
                btnSaveDoc.IsEnabled = true;
            }
        }

        private void BtnFormat_Click(object sender, RoutedEventArgs e)
        {
            ReformatText(textEditor);
            SaveText();
        }

        private void BtnMark_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion


        void textEditor_TextArea_SelectionChanged(object sender, EventArgs e)
        {
            if (textEditor.SelectedText.Length > 0)
            {
                lb2.Content = "字数：" + WordCount(textEditor.SelectedText) + "/" + words.ToString();
            }
            else
            {
                lb2.Content = "字数：" + words.ToString();
            }
        }


    }
}
