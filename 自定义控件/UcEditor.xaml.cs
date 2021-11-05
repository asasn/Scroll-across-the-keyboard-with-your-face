﻿using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using 脸滚键盘.公共操作类;

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
        readonly string TypeOfTree = "book";

        TreeOperate.TreeViewNode CurNode;

        /// <summary>
        /// 事件：控件载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uc_Loaded(object sender, RoutedEventArgs e)
        {
            

            
        }

        public void LoadChapter()
        {
            CurNode = this.DataContext as TreeOperate.TreeViewNode;
            uc.textEditor.Load(EditorOperate.ConvertStringToStream(CurNode.NodeContent));
        }


        /// <summary>
        /// 根据文件设置语法规则
        /// </summary>
        public void SetRules()
        {
            textEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(Gval.Path.App, "Text.xshd");
            Stream xshdStream = File.OpenRead(fullFileName);
            XmlTextReader xshdReader = new XmlTextReader(xshdStream);
            textEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);
            xshdReader.Close();
            xshdStream.Close();

            //if (Gval.ucRoleCard != null && Gval.ucFactionCard != null && Gval.ucGoodsCard != null && Gval.ucCommonCard != null)
            //{
            //    TreeView[] tvs = { Gval.ucRoleCard.tv, Gval.ucFactionCard.tv, Gval.ucGoodsCard.tv, Gval.ucCommonCard.tv };

            //    foreach (TreeView tv in tvs)
            //    {
            //        if (tv != null)
            //        {
            //            foreach (TreeViewItem item in tv.Items)
            //            {
            //                AddKeyword(item.Header.ToString(), tv.Tag.ToString());
            //            }
            //        }
            //    }
            //}

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
            if (lb1 != null && lb2 != null)
            {
                words = EditorOperate.WordCount(textEditor.Text);
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
            SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, Gval.CurrentBook.Name + ".db");
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
            //    string tableName = uc.Gval.CurrentBook.Name + "_" + uc.TypeOfTree;
            //    SqliteOperate sqlConn = new SqliteOperate(Gval.Path.Books, uc.Gval.CurrentBook.Name + ".db");
            //    string sql = string.Format("UPDATE Tree_{0} set NodeName='{1}' where Uid = '{2}';", tableName, CurNode.NodeName, CurNode.Uid);
            //    sqlConn.ExecuteNonQuery(sql);
            //    sqlConn.Close();
            //}
        }

        public void btnSaveText_Click(object sender, RoutedEventArgs e)
        {
            SaveText();
            HandyControl.Controls.Growl.Success("文件保存！");
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

    }
}
