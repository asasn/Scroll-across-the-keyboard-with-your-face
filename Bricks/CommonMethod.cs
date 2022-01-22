﻿
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace NSMain.Bricks
{
    /// <summary>
    /// 公共操作类：通用
    /// </summary>
    class CommonMethod
    {
        public static Thread CreateSplashWindow()
        {
            Thread t = new Thread(() =>
            {
                GlobalVal.Uc.SpWin = new SplashWindow();
                GlobalVal.Uc.SpWin.ShowDialog();//不能用Show
            });
            t.SetApartmentState(ApartmentState.STA);//设置单线程
            t.Start();
            return t;
        }

        private static IList<HighlightingRule> SetRules(TextEditor textEditor, string keyword, string colorName)
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
                Color = textEditor.SyntaxHighlighting.GetNamedColor(colorName),
                Regex = new Regex(keyword)
            };
            rules.Insert(0, rule);
            return rules;
        }

        private static IList<HighlightingSpan> SetSpans(TextEditor textEditor, string keyword, string colorName)
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
            span.SpanColor = textEditor.SyntaxHighlighting.GetNamedColor(colorName);
            span.StartExpression = new Regex(keyword);
            span.EndExpression = new Regex("");
            span.SpanColorIncludesStart = true;
            span.SpanColorIncludesEnd = true;
            spans.Insert(0, span);
            return spans;
        }

        /// <summary>
        /// 向编辑器添加变色关键词
        /// </summary>
        /// <param name="keyword"></param>
        public static void AddKeyWordForEditor(TextEditor textEdit, string keyword, string colorName)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return;
            }
            SetRules(textEdit, keyword, colorName);
        }
    }
}