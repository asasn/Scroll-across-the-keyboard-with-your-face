using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using RootNS.View;
using RootNS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace RootNS.Helper
{
    internal class EditorHelper
    {
        /// <summary>
        /// 刷新信息卡标志以在box中标记（是否包含在文章中）
        /// </summary>
        /// <param name="tEditor"></param>
        public static void RefreshIsContainFlagForCardsBox(string text)
        {
            Card[] CardBoxs = { Gval.CurrentBook.CardRole, Gval.CurrentBook.CardOther, Gval.CurrentBook.CardWorld };
            foreach (Card rootCard in CardBoxs)
            {
                foreach (Card card in rootCard.ChildNodes)
                {
                    card.IsContain = false;
                    if (card.IsDel == true)
                    {
                        continue;
                    }
                    if (text.Contains(card.Title.Trim()))
                    {
                        card.IsContain = true;
                        continue;
                    }
                    foreach (Card.Tip tip in card.NickNames.Tips)
                    {
                        if (text.Contains(tip.Title.Trim()))
                        {
                            card.IsContain = true;
                            continue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据node对象从编辑器容器当中选定并返回当前的TabItem对象
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static HandyControl.Controls.TabItem SelectItem(HandyControl.Controls.TabControl tabControl, Node node)
        {
            foreach (HandyControl.Controls.TabItem item in tabControl.Items)
            {
                if (item.Uid == node.Uid)
                {
                    item.IsSelected = true;
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 字符串转化为字节流
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static MemoryStream ConvertStringToStream(string strContent)
        {
            byte[] array = Encoding.UTF8.GetBytes(strContent);
            MemoryStream stream = new MemoryStream(array);
            return stream;
        }

        /// <summary>
        /// 光标移动至文末
        /// </summary>
        /// <param name="tEditor"></param>
        public static void MoveToEnd(TextEditor tEditor)
        {
            tEditor.ScrollToLine(tEditor.LineCount);
            tEditor.SelectionLength = 0;
            tEditor.SelectionStart = tEditor.Text.Length;
            for (int i = 0; i < 5; i++)
            {
                tEditor.ScrollToEnd();
            }
        }

        /// <summary>
        /// 字数统计
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int CountWords(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return 0;
            }
            int total = 0;
            char[] q = content.ToCharArray();
            for (int i = 0; i < q.Length; i++)
            {
                if (q[i] > 32 && q[i] != 0xA0 && q[i] != 0x3000) // 非空字符，Unicode编码0x3000为全角空格
                {
                    total += 1;
                }
            }
            return total;
        }


        /// <summary>
        /// 文字排版，并重新赋值给编辑框
        /// </summary>
        /// <param name="tEditor"></param>
        public static void TypeSetting(TextEditor tEditor)
        {
            string reText = "　　"; //开头是两个全角空格
            string[] sArray = tEditor.Text.Split(new char[] { '\r', '\n', '\t' });
            string[] sArrayNoEmpty = sArray.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            foreach (string lineStr in sArrayNoEmpty)
            {
                //当前段落非空时，注意，这里的长度需要-1才是最后一个索引号
                if (Array.IndexOf(sArrayNoEmpty, lineStr) != sArrayNoEmpty.Length - 1)
                {
                    //非末尾的情况
                    reText += lineStr.Trim() + "\n\n　　";
                }
                else
                {
                    //末尾时不添加新行
                    reText += lineStr.Trim();
                }
            }
            //排版完成，重新赋值给文本框
            tEditor.Text = reText;
            //光标移动至文末 
            tEditor.ScrollToLine(tEditor.LineCount);
            tEditor.SelectionLength = 0;
            tEditor.SelectionStart = tEditor.Text.Length;
            tEditor.ScrollToEnd();
            tEditor.ScrollToEnd();
        }


        /// <summary>
        /// 初始化配色方案（从文件）
        /// </summary>
        private static HighlightingRuleSet InitEditorColorRules(TextEditor tEditor, string xshdPath)
        {
            Uri uri = new Uri(xshdPath, UriKind.Relative);
            Stream xshdStream = Application.GetResourceStream(uri).Stream;
            XmlTextReader xshdReader = new XmlTextReader(xshdStream);
            tEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);
            xshdReader.Close();
            xshdStream.Close();
            return tEditor.SyntaxHighlighting.MainRuleSet;
        }

        /// <summary>
        /// 设置信息卡配色方案（当前书籍）
        /// </summary>
        /// <param name="tEditor"></param>
        public static void SetColorRulesForCards(TextEditor tEditor)
        {
            InitEditorColorRules(tEditor, Gval.Path.XshdPath);
            Card[] CardBoxs = { Gval.CurrentBook.CardRole, Gval.CurrentBook.CardOther, Gval.CurrentBook.CardWorld };
            foreach (Card rootCard in CardBoxs)
            {
                foreach (Card card in rootCard.ChildNodes)
                {
                    AddCardKeyWord(tEditor, card);
                }
            }
        }

        /// <summary>
        /// 设置信息卡配色方案（搜索结果）
        /// </summary>
        public static void SetColorRulesForSearchResult(TextEditor tEditor, string[] Matches)
        {
            HighlightingRuleSet mainRuleSet = InitEditorColorRules(tEditor, Gval.Path.XshdPath);
            //清空文件内的自带规则
            mainRuleSet.Rules.Clear();
            mainRuleSet.Spans.Clear();
            foreach (string keyword in Matches)
            {
                AddKeyWordForEditor(tEditor, keyword, "搜索");
            }
        }


        /// <summary>
        /// 遍历TabControl以刷新所有打开的文档配色方案
        /// </summary>
        /// <param name="card"></param>
        public static void RefreshKeyWordForAllEditor(Card card)
        {
            foreach (HandyControl.Controls.TabItem tabItem in Gval.EditorTabControl.Items)
            {
                if (((tabItem.Content as Editorkernel).DataContext as Node).OwnerName != card.OwnerName)
                {
                    continue;
                }
                TextEditor tEditor = (tabItem.Content as Editorkernel).ThisTextEditor;
                InitEditorColorRules(tEditor, "../Assets/Text.xshd");
                SetColorRulesForCards(tEditor);
            }
        }

        private static void AddCardKeyWord(TextEditor tEditor, Card card)
        {
            AddKeyWordForEditor(tEditor, card.Title, card.TabName);
            foreach (Card.Tip tip in card.NickNames.Tips)
            {
                AddKeyWordForEditor(tEditor, tip.Title, tip.TabName);
            }
        }

        /// <summary>
        /// 向编辑器添加变色关键词
        /// </summary>
        /// <param name="keyword"></param>
        private static void AddKeyWordForEditor(TextEditor textEdit, string keyword, string colorName)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return;
            }
            SetRules(textEdit, keyword, colorName);
        }

        private static IList<HighlightingRule> SetRules(TextEditor textEditor, string keyword, string colorTabName)
        {
            try
            {
                new Regex(keyword);
            }
            catch (Exception)
            {
                keyword = "\\" + keyword;
            }
            IList<HighlightingRule> rules = textEditor.SyntaxHighlighting.MainRuleSet.Rules;
            HighlightingRule rule = new HighlightingRule
            {
                Color = textEditor.SyntaxHighlighting.GetNamedColor(colorTabName),
                Regex = new Regex(keyword)
            };
            rules.Add(rule);
            return rules;
        }

        private static IList<HighlightingSpan> SetSpans(TextEditor textEditor, string keyword, string colorTabName)
        {
            try
            {
                new Regex(keyword);
            }
            catch (Exception)
            {
                keyword = "\\" + keyword;
            }
            IList<HighlightingSpan> spans = textEditor.SyntaxHighlighting.MainRuleSet.Spans;
            HighlightingSpan span = new HighlightingSpan();
            span.SpanColor = textEditor.SyntaxHighlighting.GetNamedColor(colorTabName);
            span.StartExpression = new Regex(keyword);
            span.EndExpression = new Regex("");
            span.SpanColorIncludesStart = true;
            span.SpanColorIncludesEnd = true;
            spans.Insert(0, span);
            return spans;
        }
    }
}