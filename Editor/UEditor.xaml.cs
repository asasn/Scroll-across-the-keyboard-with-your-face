using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using NSMain.Bricks;
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using static NSMain.TreeViewPlus.CNodeModule;

namespace NSMain.Editor
{
    /// <summary>
    /// UcEditor.xaml 的交互逻辑
    /// </summary>
    public partial class UEditor : UserControl
    {
        public UEditor()
        {
            InitializeComponent();
        }

        ToolTip toolTip = new ToolTip();

        /// <summary>
        /// 鼠标悬浮提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditor_MouseHover(object sender, MouseEventArgs e)
        {
            var pos = TextEditor.GetPositionFromPoint(e.GetPosition(TextEditor));
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
        private void TextEditor_MouseHoverStopped(object sender, MouseEventArgs e)
        {
            toolTip.IsOpen = false;
        }

        string TypeOfTree;
        string CurBookName;
        TreeViewNode CurNode;

        SearchPanel searchPanel;
        private SearchPanel SearchObj()
        {
            //快速搜索功能
            SearchPanel searchPanel = SearchPanel.Install(TextEditor.TextArea);
            searchPanel.Visibility = Visibility.Hidden;
            return searchPanel;
        }

        /// <summary>
        /// 事件：控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Uc_Loaded(object sender, RoutedEventArgs e)
        {
            TextEditor.TextArea.SelectionChanged += TextEditor_TextArea_SelectionChanged;

            if (TypeOfTree == "book")
            {
                lbValue.Visibility = Visibility.Visible;
            }
            else
            {
                lbValue.Visibility = Visibility.Hidden;
            }

            //是否启用AvalonEdit自带的搜索面板
            //searchPanel = SearchObj();
        }

        public void LoadChapter(string curBookName, string typeOfTree)
        {
            CurNode = this.DataContext as TreeViewNode;
            GlobalVal.CurrentBook.CurNode = this.DataContext as TreeViewNode;
            TypeOfTree = typeOfTree;
            CurBookName = curBookName;
            TextEditor.Load(CEditor.ConvertStringToStream(CurNode.NodeContent));

            if (TypeOfTree == "book")
            {
                //光标移动至文末
                TextEditor.ScrollToLine(TextEditor.LineCount);
                TextEditor.SelectionLength = 0;
                TextEditor.SelectionStart = TextEditor.Text.Length;
                for (int i = 0; i < 5; i++)
                {
                    TextEditor.ScrollToEnd();
                }
            }


            SetEditorColorRules();
        }


        /// <summary>
        /// 根据文件设置语法规则
        /// </summary>
        public void SetEditorColorRules()
        {
            TextEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(GlobalVal.Path.App, "Resourses/Text.xshd");
            Stream xshdStream = File.OpenRead(fullFileName);
            XmlTextReader xshdReader = new XmlTextReader(xshdStream);
            TextEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);
            xshdReader.Close();
            xshdStream.Close();

            //读取文件内的自带规则
            //IList<HighlightingRule> rules = TextEditor.SyntaxHighlighting.MainRuleSet.Rules;
            //IList<HighlightingSpan> spans = TextEditor.SyntaxHighlighting.MainRuleSet.Spans;
            //清空文件内的自带规则
            //textEditor.SyntaxHighlighting.MainRuleSet.Rules.Clear();
            //textEditor.SyntaxHighlighting.MainRuleSet.Spans.Clear();

            if (GlobalVal.Uc.RoleCards != null && GlobalVal.Uc.OtherCards != null && GlobalVal.Uc.WorldCards != null)
            {
                //这里的顺序决定着着色的最终效果，应把角色放在后面
                WrapPanel[] wps = { GlobalVal.Uc.WorldCards.WpCards, GlobalVal.Uc.OtherCards.WpCards, GlobalVal.Uc.RoleCards.WpCards };

                foreach (WrapPanel wp in wps)
                {
                    string keyword;
                    string tableName = wp.Tag.ToString();
                    CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                    SQLiteDataReader reader = cSqlite.ExecuteQuery(string.Format("SELECT 名称 FROM (SELECT 名称 FROM {0}主表 UNION SELECT Text FROM {0}从表 where Tid=(select Uid from {0}属性表 where Text='别称')) ORDER BY LENGTH(名称) DESC;", tableName));
                    while (reader.Read())
                    {
                        keyword = reader["名称"].ToString();
                        CommonMethod.AddKeyWordForEditor(TextEditor, keyword, tableName);
                    }
                    reader.Close();
                }
            }

        }


        private void Uc_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void ChapterNameBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //转移焦点，触发标题栏失去焦点事件
                TextEditor.Focus();

                //光标移动至文末
                TextEditor.SelectionStart = TextEditor.Text.Length;
                TextEditor.ScrollToLine(TextEditor.LineCount);
                TextEditor.ScrollToEnd();
            }
        }

        public int words = 0;
        /// <summary>
        /// 方法：展示状态栏段落和字数信息
        /// </summary>
        void ShowTextInfo()
        {
            words = CEditor.CountWords(TextEditor.Text);
            if (lb1 != null && lb2 != null)
            {
                lb1.Content = "段落：" + TextEditor.Document.Lines.Count.ToString();
                lb2.Content = "字数：" + words.ToString();
                lbValue.Content = "价值：" + ((double)words / 1000 * GlobalVal.CurrentBook.Price).ToString() + "元";
            }
        }

        /// <summary>
        /// 方法：编辑区文字保存
        /// </summary>
        public void 方法丨编辑区文字保存()
        {
            TreeViewNode CurNode = this.DataContext as TreeViewNode;

            if (CurNode == null)
            {
                return;
            }
            CurNode.NodeContent = TextEditor.Text;
            CurNode.WordsCount = words;
            GlobalVal.CurrentBook.CurNode = CurNode;
            BtnSaveDoc.IsEnabled = false;
            GlobalVal.Uc.RoleCards.MarkNamesInChapter();
            try
            {
                //Console.WriteLine("保存至数据库");
                string tableName = TypeOfTree;
                CSqlitePlus cSqlite = GlobalVal.SQLClass.Pools[CurBookName];
                string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}', isDir={2}, NodeContent='{3}', WordsCount={4}, IsExpanded={5}, IsChecked={6} where Uid = '{7}';", tableName, CurNode.NodeName.Replace("'", "''"), CurNode.IsDir, TextEditor.Text.Replace("'", "''"), words, CurNode.IsExpanded, CurNode.IsChecked, CurNode.Uid);
                cSqlite.ExecuteNonQuery(sql);
                //在数据库占用和重复连接之间选择了一个平衡。保持连接会导致文件占用，不能及时同步和备份，过多重新连接则是不必要的开销。
                cSqlite.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("本次保存失败！\n{0}", ex));
            }
        }





        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            ShowTextInfo();

            if (TextEditor.TextArea.IsFocused == true && CurNode != null)
            {
                BtnSaveDoc.IsEnabled = true;
            }
            if (FlagPaste == true)
            {
                FlagPaste = false;
                //连续粘贴可能会引起频繁保存的卡顿，所以注释掉了
                //SaveText();
            }
        }

        /// <summary>
        /// 事件：编辑区按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && TextEditor.TextArea.IsFocused == true)
            {
                TextEditor.Document.Insert(TextEditor.SelectionStart, "\n　　");
                TextEditor.LineDown();//滚动，行+1
                方法丨编辑区文字保存();
            }
            if (e.Key == Key.F9)
            {
                CEditor.TypeSetting(TextEditor);
                方法丨编辑区文字保存();
            }
            //进行了删除之后（太过频繁）
            //if (e.Key == Key.Delete || e.Key == Key.Back)
            //{
            //    SaveText();
            //}
            //逗号的情况
            if (e.Key == Key.OemComma)
            {
                方法丨编辑区文字保存();
            }
            //句号的情况
            if (e.Key == Key.OemPeriod)
            {
                方法丨编辑区文字保存();
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                //如果searchPanel面板存在，将会占用Ctrl+F的KeyDown事件，所以需要放在KeyUp来调用
                //theDialog = FindReplaceDialog.ShowForReplace(TextEditor);
                //theDialog.TabFind.IsSelected = true;
            }
        }
        FindReplaceDialog theDialog;
        /// <summary>
        /// 事件：编辑区快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                //同时按下了Ctrl + S键（S要最后按，因为判断了此次事件的e.Key）
                //修饰键只能按下Ctrl，如果还同时按下了其他修饰键，则不会进入
                BtnSaveText_Click(null, null);
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                //如果searchPanel面板存在，将会占用此一快捷键，需要的功能放在KeyUp去调用
                theDialog = FindReplaceDialog.ShowForReplace(TextEditor);
                theDialog.TabFind.IsSelected = true;
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.H)
            {
                theDialog = FindReplaceDialog.ShowForReplace(TextEditor);
                theDialog.TabReplace.IsSelected = true;
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
                    theDialog = new FindReplaceDialog(TextEditor);
                }
                theDialog.cbSearchUp.IsChecked = false;
                theDialog.FindNext(TextEditor.TextArea.Selection.GetText());

                //searchPanel.SearchPattern = TextEditor.SelectedText;
                //searchPanel.FindNext();
                //if (searchPanel.IsClosed == true && string.IsNullOrEmpty(searchPanel.SearchPattern) == false)
                //{
                //    searchPanel.Open();
                //}
            }
            if (e.Key == Key.F4)
            {
                //如果被searchPanel占用就不要再设置
                if (theDialog == null)
                {
                    theDialog = new FindReplaceDialog(TextEditor);
                }
                theDialog.cbSearchUp.IsChecked = true;
                theDialog.FindNext(TextEditor.TextArea.Selection.GetText());

                //searchPanel.SearchPattern = TextEditor.SelectedText;
                //searchPanel.FindPrevious();
                //if (searchPanel.IsClosed == true && string.IsNullOrEmpty(searchPanel.SearchPattern) == false)
                //{
                //    searchPanel.Open();
                //}
            }
        }

        private void TextEditor_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        bool FlagPaste;
        private void TextEditor_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut ||
                 e.Command == ApplicationCommands.Paste
             )
            {
                //预览剪切，粘贴
                FlagPaste = true;
            }
        }

        #region 右侧按钮面板
        public void BtnSaveText_Click(object sender, RoutedEventArgs e)
        {
            if (CurNode == null)
            {
                return;
            }
            方法丨编辑区文字保存();
            HandyControl.Controls.Growl.Success("本文档内容保存！");
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TextEditor.Text);
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
            string temp = TextEditor.Text;
            TextEditor.Text = Clipboard.GetText();
            BtnUndo.DataContext = temp;
            BtnUndo.IsEnabled = true;
            CEditor.TypeSetting(TextEditor);
            if (CurNode != null)
            {
                BtnSaveDoc.IsEnabled = true;
            }

        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            TextEditor.Text = BtnUndo.DataContext.ToString();
            CEditor.TypeSetting(TextEditor);
            BtnUndo.IsEnabled = false;
            if (CurNode != null)
            {
                BtnSaveDoc.IsEnabled = true;
            }
        }

        private void BtnFormat_Click(object sender, RoutedEventArgs e)
        {
            CEditor.TypeSetting(TextEditor);
            方法丨编辑区文字保存();
        }


        #endregion


        void TextEditor_TextArea_SelectionChanged(object sender, EventArgs e)
        {
            if (TextEditor.SelectedText.Length > 0)
            {
                lb2.Content = "字数：" + CEditor.CountWords(TextEditor.SelectedText) + "/" + words.ToString();
            }
            else
            {
                lb2.Content = "字数：" + words.ToString();
            }
        }
    }
}
