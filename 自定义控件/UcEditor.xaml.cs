using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using 脸滚键盘.信息卡和窗口;
using 脸滚键盘.公共操作类;
using static 脸滚键盘.公共操作类.TreeOperate;

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

        string TypeOfTree;
        string CurBookName;
        SearchPanel searchPanel;
        TreeOperate.TreeViewNode CurNode;

        /// <summary>
        /// 事件：控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Loaded(object sender, RoutedEventArgs e)
        {

            textEditor.TextArea.SelectionChanged += textEditor_TextArea_SelectionChanged;

            //快速搜索功能
            //searchPanel =  SearchPanel.Install(textEditor.TextArea);
            //searchPanel.UseRegex = true;
            //searchPanel.Visibility = Visibility.Hidden;
            //searchPanel.Open();
        }

        public void LoadChapter(string curBookName, string typeOfTree)
        {
            CurNode = this.DataContext as TreeOperate.TreeViewNode;
            TypeOfTree = typeOfTree;
            CurBookName = curBookName;
            textEditor.Load(EditorOperate.ConvertStringToStream(CurNode.NodeContent));

            //光标移动至文末          
            textEditor.ScrollToLine(textEditor.LineCount);
            textEditor.SelectionStart = textEditor.Text.Length;
            textEditor.ScrollToEnd();
            textEditor.ScrollToEnd();

            SetRules();
        }


        /// <summary>
        /// 根据文件设置语法规则
        /// </summary>
        public void SetRules()
        {
            textEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(Gval.Path.App, "Resourse/Text.xshd");
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
                    if (wp != null)
                    {
                        foreach (Button btn in wp.Children)
                        {
                            AddKeyword(btn.Content.ToString(), wp.Tag.ToString());
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 添加一个变色关键词
        /// </summary>
        /// <param name="keyword"></param>
        void AddKeyword(string keyword, string colorName)
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
            words = EditorOperate.WordCount(textEditor.Text);
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
        void SaveText()
        {
            TreeOperate.TreeViewNode CurNode = this.DataContext as TreeOperate.TreeViewNode;

            string tableName = TypeOfTree;
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, CurBookName + ".db");
            string sql = string.Format("UPDATE Tree_{0} set NodeContent='{1}', WordsCount={2} where Uid = '{3}';", tableName, textEditor.Text, words, CurNode.Uid);
            sqlConn.ExecuteNonQuery(sql);
            sqlConn.Close();


            CurNode.NodeContent = textEditor.Text;
            CurNode.WordsCount = words;

            btnSaveDoc.IsEnabled = false;
            textEditor.Tag = null;

            Console.WriteLine("保存至数据库");
        }

        private void chapterNameBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void chapterNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //CurNodeName = chapterNameBox.Text;
            //CurNode.NodeName = chapterNameBox.Text;
            //if (CurNode != null)
            //{
            //    Console.WriteLine("Editor>>>CurNodeName");
            //    string tableName = uc.CurBookName + "_" + uc.TypeOfTree;
            //    SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, uc.CurBookName + ".db");
            //    string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, CurNode.NodeName, CurNode.Uid);
            //    sqlConn.ExecuteNonQuery(sql);
            //    sqlConn.Close();
            //}
        }

        public void btnSaveText_Click(object sender, RoutedEventArgs e)
        {
            SaveText();
            HandyControl.Controls.Growl.Success("本文档内容保存！");
        }

        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            ShowTextInfo();

            if (this.IsEnabled == true && textEditor.TextArea.IsFocused == true)
            {
                btnSaveDoc.IsEnabled = true;
            }
            if (textEditor.Tag != null)
            {
                SaveText();
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
                EditorOperate.ReformatText(textEditor);
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

        /// <summary>
        /// 事件：编辑区按钮
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
                FindReplaceDialog.ShowForReplace(textEditor);
            }
            if (e.Key == Key.F3)
            {
                //如果被searchPanel占用就不要再设置
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

        private void textEditor_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut ||
                 e.Command == ApplicationCommands.Paste
             )
            {
                //textEditor.Tag = true;
            }
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textEditor.Text);
            HandyControl.Controls.Growl.Success("已复制本文档内容到剪贴板！");
        }

        private void BtnCopyTitle_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(CurNode.NodeName);
            HandyControl.Controls.Growl.Success("已复制本文档标题到剪贴板！");
        }

        void textEditor_TextArea_SelectionChanged(object sender, EventArgs e)
        {
            if (textEditor.SelectedText.Length > 0)
            {
                lb2.Content = "字数：" + EditorOperate.WordCount(textEditor.SelectedText) + "/" + words.ToString();
            }
            else
            {
                lb2.Content = "字数：" + words.ToString();
            }
        }
    }
}
