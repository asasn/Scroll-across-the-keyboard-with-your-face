using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using NSMain.Bricks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using static NSMain.Bricks.CTreeView;

namespace NSMain.Searcher
{
    /// <summary>
    /// uc_Searcher.xaml 的交互逻辑
    /// </summary>
    public partial class USearcher : UserControl
    {
        public USearcher()
        {
            InitializeComponent();
        }



        public string UcTitle
        {
            get { return (string)GetValue(UcTitleProperty); }
            set { SetValue(UcTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTitleProperty =
            DependencyProperty.Register("UcTitle", typeof(string), typeof(USearcher), new PropertyMetadata(null));



        public TreeViewNode TopNode
        {
            get { return (TreeViewNode)GetValue(TopNodeProperty); }
            set { SetValue(TopNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TopNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopNodeProperty =
            DependencyProperty.Register("TopNode", typeof(TreeViewNode), typeof(USearcher), new PropertyMetadata(null));




        public TreeViewNode CurNode
        {
            get { return (TreeViewNode)GetValue(CurNodeProperty); }
            set { SetValue(CurNodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurNode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurNodeProperty =
            DependencyProperty.Register("CurNode", typeof(TreeViewNode), typeof(USearcher), new PropertyMetadata(null));


        public string UcTag
        {
            get { return (string)GetValue(UcTagProperty); }
            set { SetValue(UcTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UcTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UcTagProperty =
            DependencyProperty.Register("UcTag", typeof(string), typeof(USearcher), new PropertyMetadata(null));

        /// <summary>
        /// 匹配结果数组（字符串数组）
        /// </summary>
        private string[] Matches;



        /// <summary>
        /// 搜索参数初始化
        /// </summary>
        void InitSearch()
        {
            //初始化列表框
            ListBoxOfResults.Items.Clear();

            if (CbMaterial.IsChecked == true)
            {
                //搜索资料库
                CurNode = GlobalVal.Uc.TreeMaterial.CurNode;
                TopNode = GlobalVal.Uc.TreeMaterial.TopNode;
            }
            else
            {
                //搜索当前书籍
                CurNode = GlobalVal.Uc.TreeBook.CurNode;
                TopNode = GlobalVal.Uc.TreeBook.TopNode;
            }
        }

        #region 执行搜索

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //搜索参数初始化
            InitSearch();

            //输入检校（空字符串或者为 * 时退出）
            if (string.IsNullOrEmpty(TbKeyWords.Text.Trim()))
            {
                return;
            }
            //输入检校（正则表达式错误）
            if (CbRegex.IsChecked == true && GetMatchRets("") == null)
            {
                return;
            }

            //生成搜索结果列表
            if (RadButton1.IsChecked == true)
            {
                GetResultMain(CurNode);
            }
            if (RadButton2.IsChecked == true)
            {
                GetResultMain(TopNode);
            }

        }

        private void TbKeyWords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        #endregion

        #region 在列表框中添加结果集

        /// <summary>
        /// 递归获取结果集主函数
        /// </summary>
        /// <param name="baseNode"></param>
        private void GetResultMain(TreeViewNode baseNode)
        {
            if (baseNode == null)
            {
                return;
            }
            if (baseNode.IsDir == true)
            {
                foreach (TreeViewNode node in baseNode.ChildNodes)
                {
                    GetResultMain(node);
                }
            }
            else
            {
                AddToListBox(baseNode);
            }
        }

        /// <summary>
        /// 把成功匹配的章节添加进列表框
        /// </summary>
        /// <param name="node"></param>
        void AddToListBox(TreeViewNode node)
        {
            if (string.IsNullOrEmpty(node.NodeContent))
            {
                return;
            }

            string ListItemName = String.Empty;

            //正则模式
            if (CbRegex.IsChecked == true)
            {
                Matches = GetMatchRets(node.NodeContent);
                ListItemName = string.Join(" ", Matches);
            }

            //与模式
            if (CbAnd.IsChecked == true)
            {
                Matches = GetModeAndRets(node.NodeContent);
                ListItemName = string.Join(" ", Matches);
            }

            //或模式
            if (CbOr.IsChecked == true)
            {
                Matches = GetModeOrRets(node.NodeContent);
                ListItemName = string.Join(" ", Matches);
            }


            if (Matches.Length > 99)
            {
                Console.WriteLine("匹配结果太多，无意义，所以丢弃结果不用");
                return;
            }

            //获取和定义列表项
            if (false == string.IsNullOrEmpty(ListItemName))
            {
                ListBoxItem lbItem = new ListBoxItem
                {
                    Content = node.NodeName + " >> " + ListItemName,
                    DataContext = node.NodeContent,
                    Tag = Matches
                };
                ListBoxOfResults.Items.Add(lbItem);
                SetItemToolTip(lbItem);
            }
        }

        /// <summary>
        /// 获取正则匹配结果列表
        /// </summary>
        /// <param name="NodeContent"></param>
        /// <returns></returns>
        string[] GetMatchRets(string NodeContent)
        {
            MatchCollection matchRets;
            try
            {
                matchRets = Regex.Matches(NodeContent, TbKeyWords.Text);
            }
            catch (Exception)
            {
                Console.WriteLine("正则表达式错误！");
                return null;
            }

            if (string.IsNullOrEmpty(NodeContent))
            {
                return new List<string>().ToArray();
            }


            if (matchRets.Count > 0)
            {
                List<string> listResult = new List<string>();
                foreach (Match item in matchRets)
                {
                    if (false == listResult.Contains(item.Value))
                    {
                        listResult.Add(item.Value);
                    }
                }
                return listResult.ToArray();
            }
            else
            {
                return new List<string>().ToArray();
            }
        }

        /// <summary>
        /// 获取与模式匹配结果列表
        /// </summary>
        /// <param name="NodeContent"></param>
        /// <returns></returns>
        string[] GetModeAndRets(string NodeContent)
        {
            List<string> rets = new List<string>();
            string[] keysArray = TbKeyWords.Text.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();

            bool ret = true;
            foreach (var mystr in keysArray)
            {
                if (false == string.IsNullOrEmpty(mystr))
                {
                    //不匹配时跳出
                    if (false == IsInText(NodeContent, mystr))
                    {
                        ret = false;
                        break;
                    }
                }
            }
            if (ret == true)
            {
                return TbKeyWords.Text.Split(' ');
            }
            else
            {
                return new List<string>().ToArray();
            }
        }

        /// <summary>
        /// 获取或模式匹配结果列表
        /// </summary>
        /// <param name="NodeContent"></param>
        /// <returns></returns>
        string[] GetModeOrRets(string NodeContent)
        {
            List<string> rets = new List<string>();
            string[] keysArray = TbKeyWords.Text.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();
            foreach (var mystr in keysArray)
            {
                if (false == string.IsNullOrEmpty(mystr))
                {
                    if (IsInText(NodeContent, mystr))
                    {
                        rets.Add(mystr);
                    }
                }
            }
            return rets.ToArray();
        }

        /// <summary>
        /// 判断文本当中是否包含某个字符串
        /// </summary>
        /// <param name="NodeContent"></param>
        /// <param name="mystr"></param>
        /// <returns></returns>
        private bool IsInText(string NodeContent, string mystr)
        {
            //空节点时返回空
            if (string.IsNullOrEmpty(NodeContent))
            {
                return false;
            }

            string text = NodeContent;
            if (text.Contains(mystr))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 关键词变色
        /// <summary>
        /// 关键词变色主函数
        /// </summary>
        private void SetKeyWordsColor()
        {
            TextEditor.SyntaxHighlighting = null;
            string fullFileName = System.IO.Path.Combine(GlobalVal.Path.App, "Resourses/Text.xshd");
            Stream xshdStream = File.OpenRead(fullFileName);
            XmlTextReader xshdReader = new XmlTextReader(xshdStream);
            TextEditor.SyntaxHighlighting = HighlightingLoader.Load(xshdReader, HighlightingManager.Instance);
            xshdReader.Close();
            xshdStream.Close();

            //读取文件内的自带规则
            //IList<HighlightingRule> rules = textEditor.SyntaxHighlighting.MainRuleSet.Rules;
            //IList<HighlightingSpan> spans = textEditor.SyntaxHighlighting.MainRuleSet.Spans;
            //清空文件内的自带规则
            TextEditor.SyntaxHighlighting.MainRuleSet.Rules.Clear();
            TextEditor.SyntaxHighlighting.MainRuleSet.Spans.Clear();

            foreach (string keyword in Matches)
            {
                CommonMethod.AddKeyWordForEditor(TextEditor, keyword, "搜索");
            }
        }

        #endregion

        TextEditor TextEditor = new TextEditor();
        /// <summary>
        /// </summary>
        /// 给列表项添加悬浮内容
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SetItemToolTip(ListBoxItem lbItem)
        {
            TextEditor tEdit = new TextEditor()
            {
                Width = 400,
                WordWrap = true,

                FontFamily = new FontFamily("宋体")
            };
            tEdit.Options.WordWrapIndentation = 4;
            tEdit.Options.InheritWordWrapIndentation = false;
            TextEditor = tEdit;
            string lines = GetStrOnLines(lbItem.DataContext.ToString());
            TextEditor.Text = lines;
            SetKeyWordsColor();

            ToolTip ttp = new ToolTip
            {
                Content = TextEditor
            };
            lbItem.ToolTip = ttp;
        }

        //从当前行当中判断字符串是否存在
        private string GetStrOnLines(string nodeContent)
        {
            int counter = 0;
            string lines = string.Empty;

            string[] sArray = nodeContent.Split(new char[] { '\n' });
            string[] sArrayNoEmpty = sArray.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            //正则模式
            if (CbRegex.IsChecked == true)
            {
                string[] keysArray = GetMatchRets(nodeContent);
                foreach (string line in sArrayNoEmpty)
                {
                    foreach (string mystr in keysArray)
                    {
                        if (line.Contains(mystr))
                        {
                            lines += line + "\n";
                            counter++;
                            break;
                        }
                    }
                }
            }

            //与模式
            if (CbAnd.IsChecked == true)
            {
                string[] keysArray = TbKeyWords.Text.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                foreach (string line in sArrayNoEmpty)
                {
                    bool isAll = false;
                    foreach (string mystr in keysArray)
                    {
                        if (line.Contains(mystr))
                        {
                            isAll = true;
                            break;
                        }
                    }
                    if (isAll == true)
                    {
                        lines += line + "\n";
                        counter++;
                    }
                }
            }

            //或模式
            if (CbOr.IsChecked == true)
            {
                string[] keysArray = TbKeyWords.Text.Split(new char[] { ' ' }).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                foreach (string line in sArrayNoEmpty)
                {
                    foreach (string mystr in keysArray)
                    {
                        if (line.Contains(mystr))
                        {
                            lines += line + "\n";
                            counter++;
                            break;
                        }
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxOfResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lbItem = ListBoxOfResults.SelectedItem as ListBoxItem;
            WSearchResult rtWin = new WSearchResult(lbItem, (string[])lbItem.Tag);
            rtWin.ShowDialog();
        }
    }
}
