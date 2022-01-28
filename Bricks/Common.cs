
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NSMain.Bricks
{
    /// <summary>
    /// 公共操作类：通用
    /// </summary>
    class Common
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


        /// <summary>
        /// 滚动条
        /// </summary>
        public static class Scroll
        {
            /// <summary>
            /// 接受鼠标滚动事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public static void ScrollIt(object sender, MouseWheelEventArgs e)
            {
                ScrollViewer scrollviewer = sender as ScrollViewer;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,

                    Source = sender
                };

                scrollviewer.RaiseEvent(eventArg);
            }

            /// <summary>
            /// 根据鼠标滚动上下卷动
            /// </summary>
            /// <param name="sender"></param>
            public static void ScrollUD(object sender, MouseWheelEventArgs e)
            {
                ScrollViewer scrollviewer = sender as ScrollViewer;
                if (e.Delta > 0)
                {
                    scrollviewer.LineUp();
                    scrollviewer.LineUp();
                    scrollviewer.LineUp();
                }
                else
                {
                    scrollviewer.LineDown();
                    scrollviewer.LineDown();
                    scrollviewer.LineDown();
                }
                e.Handled = true;
            }

            /// <summary>
            /// 根据鼠标滚动左右卷动
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public static void ScrollLR(object sender, MouseWheelEventArgs e)
            {
                ScrollViewer scrollviewer = sender as ScrollViewer;
                if (e.Delta > 0)
                {
                    scrollviewer.LineLeft();
                    scrollviewer.LineLeft();
                    scrollviewer.LineLeft();
                }
                else
                {
                    scrollviewer.LineRight();
                    scrollviewer.LineRight();
                    scrollviewer.LineRight();
                }
                e.Handled = true;
            }
        }
    }
}
